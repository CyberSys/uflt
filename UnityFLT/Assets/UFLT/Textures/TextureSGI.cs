using UnityEngine;
using System.Collections;
using UFLT.Streams;
using System;
using System.IO;
using System.Text;
using UFLT.Utils;

//SGI or RGB: 3 colour channels
//RGBA: 3 colour channels and alpha
//BW or INT: black and white
//INTA: black and white and alpha

namespace UFLT.Textures
{
	/// <summary>
	/// Texture SG.
	/// File format taken from http://paulbourke.net/dataformats/sgirgb/
	/// </summary>
	public class TextureSGI 
	{
		#region Properties
		
		/// <summary>
		/// Is this a valid rgb file?
		/// </summary>	
		public bool Valid
		{
			get;
			set;
		}
		
		/// <summary>
		/// Does the file use run length encoding?
		/// </summary>	
		public bool RLE
		{
			get;
			set;
		}		
		
		/// <summary>
		/// Bytes per channel. 1 or 2.
		/// </summary>	
		public short BPC
		{
			get;
			set;
		}		
		
		/// <summary>
		/// 1,2 or 3
		///  1 means a single row, XSIZE long
		///  2 means a single 2D image
		///  3 means multiple 2D images
		/// </summary>
		public ushort Dimension
		{
			get;
			set;
		}		
		
		/// <summary>
		/// x,y,z
		///  x,y - size of image in pixels
		///  z - Number of channels
	    ///   1 indicates greyscale
	    ///   3 indicates RGB
	  	///   4 indicates RGB and Alpha
		/// </summary>	
		public ushort[] Size
		{
			get;
			set;
		}		
		
		/// <summary>
		/// Min & Max pixel value.
		/// </summary>	
		public int[] PixMinMax
		{
			get;
			set;
		}		
		
		/// <summary>
		/// Image name, max 79 chars.
		/// </summary>	
		public string Name
		{
			get;
			set;
		}		
		
		/// <summary>
		/// Colormap ID
		///  0 - normal mode
	    ///  1 - dithered, 3 mits for red and green, 2 for blue, obsolete
	    ///	 2 - index colour, obsolete
	    ///	 3 - not an image but a colourmap
		/// </summary>
		public int ColorMapID
		{
			get;
			set;
		}
		
		/// <summary>
		/// File reader.
		/// </summary>	
		private BinaryReader Reader
		{
			get;
			set;
		}		
		
		/// <summary>
		/// RLE data. The start file positions for each scanline.
		/// </summary>		
		public int[] RowStart
		{
			get;
			set;
		}		
		
		/// <summary>
		/// RLE data. The size of each scanline.
		/// </summary>		
		public int[] RowSize
		{
			get;
			set;
		}		
		
		/// <summary>
		/// Pixels if the BPC is 1.
		/// </summary>		
		public Color32[] PixelsBPC1
		{
			get;
			set;
		}
		
		/// <summary>
		/// Pixels if the bpc is 2.
		/// </summary>		
		public Color[] PixelsBPC2
		{
			get;
			set;
		}
				
		/// <summary>
		/// Gets the texture.
		/// </summary>		
		public Texture2D Texture
		{
			get
			{
				if( texture == null && Valid )
				{
					texture = new Texture2D( Size[0], Size[1] );	
					texture.hideFlags = HideFlags.DontSave;
					if( BPC == 1 )texture.SetPixels32( PixelsBPC1 );
					else texture.SetPixels( PixelsBPC2 );
					texture.Apply();
					texture.Compress( true ); // Compress.
					texture.name = Name;
				}
				return texture;										
			}
		}
		private Texture2D texture;
		
		#endregion Properties
		
		//////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Loads a SGI rgb, rgba or int texture file.
		/// </summary>
		/// <param name='file'>File path</param>
		//////////////////////////////////////////////////////////////////////
		public TextureSGI( string file )		
		{
			Valid = false;
			
			// Load file
			Stream s = new FileStream( file, FileMode.Open );
			Reader = BitConverter.IsLittleEndian ? new BinaryReaderBigEndian( s ) : new BinaryReader( s );			
			ReadHeader(); 			
			ReadPixels();
		}	
		
		//////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Reads the file header.
		/// </summary>
		//////////////////////////////////////////////////////////////////////
		private void ReadHeader()
		{
			Debug.Log( "Reading header" );
			// Magic number
			short magic = Reader.ReadInt16();		
			if( magic != 474 )
			{				
				Reader.Close();
				Debug.LogError( "Invalid file, the file header does not contain the correct magic number(474)" );
				return;
			}
			
			Valid = true;			
			RLE = Reader.ReadSByte() == 1 ? true : false;		
			BPC = Reader.ReadSByte();
			Dimension = Reader.ReadUInt16();
			Size = new ushort[]{ Reader.ReadUInt16(), Reader.ReadUInt16(), Reader.ReadUInt16() };
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
		}		
		
		//////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Reads the pixels.
		/// </summary>
		//////////////////////////////////////////////////////////////////////
		private void ReadPixels()		
		{
			//If BPC is 2, there is one short (2 bytes) per pixel. In this case the RLE data should be read into an 
			//array of shorts. To expand data, the low order seven bits of the first short: bits[6..0] are used to form 
			//a count. If bit[7] of the first short is 1, then the count is used to specify how many shorts to copy from the
			//RLE data buffer to the destination. Otherwise, if bit[7] of the first short is 0, then the count is used to specify 
			//how many times to repeat the value of the following short, in the destination. This process proceeds until a count of
			//0 is found. This should decompress exactly XSIZE pixels. Note that the byte order of short data in the input file 
			//should be used, as described above.
	
			// Read in each row
			//Color32[][] pixels = new Color32[Size[1]][];
			PixelsBPC1 = new Color32[Size[0] * Size[1]];		
			for( int y = 0; y < Size[1]; ++y )
			{					
				for( int channel = 0; channel < Size[2]; ++channel )
				{
					ReadRowBPC1( y, channel );	
				}
			}	
			
			if( Size[2] == 1 )			
			{
				// Grayscale
				for( int i = 0; i < PixelsBPC1.Length; ++i )					
				{
					PixelsBPC1[i].b = PixelsBPC1[i].g = PixelsBPC1[i].r;
					PixelsBPC1[i].a = 0xff;
				}
			}
			else if( Size[2] == 3 )
			{				
				for( int i = 0; i < PixelsBPC1.Length; ++i )					
				{				
					PixelsBPC1[i].a = 0xff;
				}	
			}			
		}
		
		//////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Reads the row for 1 byte per channel.
		/// </summary>
		/// <param name='pixels'>Pixels.</param>
		/// <param name='row'>Row.</param>
		/// <param name='channel'>Channel.</param>
		//////////////////////////////////////////////////////////////////////
		private void ReadRowBPC1( int row, int channel )
		{
			// Where to start writing to in the pixel array.
			int currentPixel = row * Size[0];			
			
			if( RLE )
			{
				int index = row + channel * Size[1];
				
				// Seek to start of row				
				Reader.BaseStream.Seek( RowStart[index], SeekOrigin.Begin ); 		    
				byte[] rowData = Reader.ReadBytes( RowSize[index] );
				
				int currentRowData = 0;					
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
							switch( channel )
							{
								case 0: PixelsBPC1[currentPixel++].r = rowData[currentRowData++]; break;							
								case 1: PixelsBPC1[currentPixel++].g = rowData[currentRowData++]; break;							
								case 2: PixelsBPC1[currentPixel++].b = rowData[currentRowData++]; break;							
								case 3: PixelsBPC1[currentPixel++].a = rowData[currentRowData++]; break;							
							}
						}
					}
					else // specify how many times to repeat the value of the following byte
					{
						pixel = rowData[currentRowData++];		
						while( pixelCount-- > 0 )
						{
							switch( channel )
							{
								case 0: PixelsBPC1[currentPixel++].r = pixel; break;							
								case 1: PixelsBPC1[currentPixel++].g = pixel; break;							
								case 2: PixelsBPC1[currentPixel++].b = pixel; break;							
								case 3: PixelsBPC1[currentPixel++].a = pixel; break;							
							}							
						}
					}				
				}
			}
			else
			{
				long readPos = 512 + ( row * Size[0] ) + ( channel * Size[0] * Size[1] );
								
				// Seek to start of row			
				Reader.BaseStream.Seek( readPos, SeekOrigin.Begin ); 		
				
				// Read pixels
				byte[] rowData = Reader.ReadBytes( Size[0] );
				
				for( int i = 0; i < Size[0]; ++i )
				{
					switch( channel )
					{
						case 0: PixelsBPC1[currentPixel++].r = rowData[i]; break;							
						case 1: PixelsBPC1[currentPixel++].g = rowData[i]; break;							
						case 2: PixelsBPC1[currentPixel++].b = rowData[i]; break;							
						case 3: PixelsBPC1[currentPixel++].a = rowData[i]; break;							
					}
				}									
			}
		}		
	}
}