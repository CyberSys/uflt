using UnityEngine;
using System.Collections;
using UFLT.Streams;
using System;
using System.IO;
using System.Text;
using UFLT.Utils;

// TODO: Clean me up. Support 2 bpc. support none rle. file ext .bw, .rgb, .rgba, .sgi. .int?

//SGI or RGB: 3 colour channels
//RGBA: 3 colour channels and alpha
//BW or INT: black and white
//INTA: black and white and alpha

/// <summary>
/// Texture SG.
/// File format taken from http://paulbourke.net/dataformats/sgirgb/
/// </summary>
public class TextureSGI : MonoBehaviour
{
	#region Properties
	
	// HACK: Remove me after testing.
	public string file = "";
	
	/// <summary>
	/// Is this a valid rgb file?
	/// </summary>	
	public bool Valid;
	//{
	//	get;
	//	set;
	//}
	
	/// <summary>
	/// Does the file use run length encoding?
	/// </summary>	
	public bool RLE;
	//{
	//	get;
	//	set;
	//}
	
	/// <summary>
	/// Bytes per channel. 1 or 2.
	/// </summary>	
	public short BPC;
	//{
	//	get;
	//	set;
	//}
	
	/// <summary>
	/// 1,2 or 3
	///  1 means a single row, XSIZE long
	///  2 means a single 2D image
	///  3 means multiple 2D images
	/// </summary>
	public /*ushort*/ int Dimension;
	//{
	//	get;
	//	set;
	//}
	
	/// <summary>
	/// x,y,z
	///  x,y - size of image in pixels
	///  z - Number of channels
    ///   1 indicates greyscale
    ///   3 indicates RGB
  	///   4 indicates RGB and Alpha
	/// </summary>	
	public int /*ushort*/[] Size;
	//{
	//	get;
	//	set;
	//}
	
	/// <summary>
	/// Min & Max pixel value.
	/// </summary>	
	public int[] PixMinMax;
	//{
	//	get;
	//	set;
	//}
	
	/// <summary>
	/// Image name, max 79 chars.
	/// </summary>	
	public string Name;
	//{
	//	get;
	//	set;
	//}
	
	/// <summary>
	/// Colormap ID
	///  0 - normal mode
    ///  1 - dithered, 3 mits for red and green, 2 for blue, obsolete
    ///	 2 - index colour, obsolete
    ///	 3 - not an image but a colourmap
	/// </summary>
	public int ColorMapID;
	//{
	//	get;
	//	set;
	//}
	
	/// <summary>
	/// File reader.
	/// </summary>	
	private BinaryReader Reader;
	//{
	//	get;
	//	set;
	//}
	
	public int[] RowStart;
	//{
	//	get;
	//	set;
	//}
	
	public int[] RowSize;	
	//{
	//	get;
	//	set;
	//}
	
	#endregion Properties
	
	//////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Loads a SGI rgb, rgba or int texture file.
	/// </summary>
	/// <param name='file'>File path</param>
	//////////////////////////////////////////////////////////////////////
	//public TextureSGI( string file )
	void Start()
	{
		// Load file
		Valid = true;
		Stream s = new FileStream( file, FileMode.Open );
		Reader = BitConverter.IsLittleEndian ? new BinaryReaderBigEndian( s ) : new BinaryReader( s );
		
		ReadHeader(); 				
	}	
	
	//////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Reads the file header.
	/// </summary>
	//////////////////////////////////////////////////////////////////////
	private void ReadHeader()
	{
		// Magic number
		short magic = Reader.ReadInt16();		
		if( magic != 474 )
		{
			Valid = false;
			Reader.Close();
			Debug.LogError( "Invalid file, the file header does not contain the correct magic number(474)" );
			return;
		}
		
		RLE = Reader.ReadSByte() == 1 ? true : false;		
		BPC = Reader.ReadSByte();
		Dimension = Reader.ReadUInt16();
		Size = new int[]{ Reader.ReadUInt16(), Reader.ReadUInt16(), Reader.ReadUInt16() };
		PixMinMax = new int[]{ Reader.ReadInt32(), Reader.ReadInt32() };
				
		// Skip dummy data
		Reader.BaseStream.Seek( 4, SeekOrigin.Current ); 
		
		// Null terminated name.
		Name = NullTerminatedString.GetAsString( Reader.ReadBytes( 80 ) );
		ColorMapID = Reader.ReadInt32();
				
		// Skip dummy data
		Reader.BaseStream.Seek( 404, SeekOrigin.Current );
		
		if( RLE )
		{
			ReadOffsets();	
		}
	}
	
	public Texture2D text;
	
	//////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Reads the offsets fields if using rle.
	/// </summary>
	//////////////////////////////////////////////////////////////////////
	private void ReadOffsets()
	{
		int count = Size[0] * Size[2]; // Scanline len * num channels
		RowStart = new int[count];
		RowSize = new int[count];
		
		for( int i = 0; i < count; ++i )
		{
			RowStart[i] = Reader.ReadInt32();
		}
		
		for( int i = 0; i < count; ++i )
		{
			RowSize[i] = Reader.ReadInt32();
		}	
		
		//If BPC is 2, there is one short (2 bytes) per pixel. In this case the RLE data should be read into an 
		//array of shorts. To expand data, the low order seven bits of the first short: bits[6..0] are used to form 
		//a count. If bit[7] of the first short is 1, then the count is used to specify how many shorts to copy from the
		//RLE data buffer to the destination. Otherwise, if bit[7] of the first short is 0, then the count is used to specify 
		//how many times to repeat the value of the following short, in the destination. This process proceeds until a count of
		//0 is found. This should decompress exactly XSIZE pixels. Note that the byte order of short data in the input file 
		//should be used, as described above.

		// Read in each row
		//Color32[][] pixels = new Color32[Size[1]][];
		Color32[] pixels = new Color32[Size[0] * Size[1]];		
		for( int y = 0; y < Size[1]; ++y )
		{	
			ReadRowRed( ref pixels, y ); 	
			if( Size[2] > 1 )ReadRowGreen( ref pixels, y );
			if( Size[2] > 2 )ReadRowBlue( ref pixels, y );
			if( Size[2] > 3 )ReadRowAlpha( ref pixels, y );
		}				
	
		text = new Texture2D( Size[0], Size[1] );
		text.SetPixels32( pixels );
		text.Apply();
	}		
	
	private void ReadRowRed( ref Color32[] pixels, int row )
	{
		// Seek to start of row
		int index = row + 0 * Size[1];
		Reader.BaseStream.Seek( RowStart[index], SeekOrigin.Begin ); 		    
		byte[] rowData = Reader.ReadBytes( RowSize[index] );
		
		int currentRowData = 0;
		int currentPixel = row * Size[0];		
		byte pixel = 0, pixelCount = 0;		
		
		while( true )
		{				
			pixel = rowData[currentRowData++];				
			pixelCount = ( byte )( pixel & 0x7F ); // bits 0-6				
			if( pixelCount == 0 )
			{
				break;	
			}
							
			if( ( pixel & 0x80 ) == 1 ) // specify how many bytes to copy from the RLE data buffer to the destination
			{					
				while( pixelCount-- > 0 )
				{
					pixels[currentPixel++].r = rowData[currentRowData++];							
				}
			}
			else // specify how many times to repeat the value of the following byte
			{
				pixel = rowData[currentRowData++];		
				while( pixelCount-- > 0 )
				{
					pixels[currentPixel++].r = pixel;	
				}
			}				
		}
	}
	
	private void ReadRowGreen( ref Color32[] pixels, int row )
	{
		// Seek to start of row
		int index = row + 1 * Size[1];
		Reader.BaseStream.Seek( RowStart[index], SeekOrigin.Begin ); 		    
		byte[] rowData = Reader.ReadBytes( RowSize[index] );
		
		int currentRowData = 0;
		int currentPixel = row * Size[0];		
		byte pixel = 0, pixelCount = 0;		
		
		while( true )
		{				
			pixel = rowData[currentRowData++];				
			pixelCount = ( byte )( pixel & 0x7F ); // bits 0-6				
			if( pixelCount == 0 )
			{
				break;	
			}
							
			if( ( pixel & 0x80 ) == 1 ) // specify how many bytes to copy from the RLE data buffer to the destination
			{					
				while( pixelCount-- > 0 )
				{
					pixels[currentPixel++].g = rowData[currentRowData++];							
				}
			}
			else // specify how many times to repeat the value of the following byte
			{
				pixel = rowData[currentRowData++];		
				while( pixelCount-- > 0 )
				{
					pixels[currentPixel++].g = pixel;	
				}
			}				
		}
	}	
	
	private void ReadRowBlue( ref Color32[] pixels, int row )
	{
		// Seek to start of row
		int index = row + 2 * Size[1];
		Reader.BaseStream.Seek( RowStart[index], SeekOrigin.Begin ); 		    
		byte[] rowData = Reader.ReadBytes( RowSize[index] );
		
		int currentRowData = 0;
		int currentPixel = row * Size[0];		
		byte pixel = 0, pixelCount = 0;		
		
		while( true )
		{				
			pixel = rowData[currentRowData++];				
			pixelCount = ( byte )( pixel & 0x7F ); // bits 0-6				
			if( pixelCount == 0 )
			{
				break;	
			}
							
			if( ( pixel & 0x80 ) == 1 ) // specify how many bytes to copy from the RLE data buffer to the destination
			{					
				while( pixelCount-- > 0 )
				{
					pixels[currentPixel++].b = rowData[currentRowData++];							
				}
			}
			else // specify how many times to repeat the value of the following byte
			{
				pixel = rowData[currentRowData++];		
				while( pixelCount-- > 0 )
				{
					pixels[currentPixel++].b = pixel;	
				}
			}				
		}
	}	
	
	private void ReadRowAlpha( ref Color32[] pixels, int row )
	{
		// Seek to start of row
		int index = row + 3 * Size[1];
		Reader.BaseStream.Seek( RowStart[index], SeekOrigin.Begin ); 		    
		byte[] rowData = Reader.ReadBytes( RowSize[index] );
		
		int currentRowData = 0;
		int currentPixel = row * Size[0];		
		byte pixel = 0, pixelCount = 0;		
		
		while( true )
		{				
			pixel = rowData[currentRowData++];				
			pixelCount = ( byte )( pixel & 0x7F ); // bits 0-6				
			if( pixelCount == 0 )
			{
				break;	
			}
							
			if( ( pixel & 0x80 ) == 1 ) // specify how many bytes to copy from the RLE data buffer to the destination
			{					
				while( pixelCount-- > 0 )
				{
					pixels[currentPixel++].a = rowData[currentRowData++];							
				}
			}
			else // specify how many times to repeat the value of the following byte
			{
				pixel = rowData[currentRowData++];		
				while( pixelCount-- > 0 )
				{
					pixels[currentPixel++].a = pixel;	
				}
			}				
		}
	}		
}
