using UnityEngine;
using System.Collections;
using UFLT.Streams;
using System;
using System.IO;
using System.Text;

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
		Size = new ushort[]{ Reader.ReadUInt16(), Reader.ReadUInt16(), Reader.ReadUInt16() };
		PixMinMax = new int[]{ Reader.ReadInt32(), Reader.ReadInt32() };
				
		// Skip dummy data
		Reader.BaseStream.Seek( 4, SeekOrigin.Current ); 
		
		// Null terminated name.
		Name = Encoding.ASCII.GetString( Reader.ReadBytes( 80 ) );
		ColorMapID = Reader.ReadInt32();
				
		// Skip dummy data
		Reader.BaseStream.Seek( 404, SeekOrigin.Current );
	}
}
