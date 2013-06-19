using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

namespace UFLT.Records
{
    /// <summary>
    /// Record for each texture pattern referenced in the database. A texture
    /// palette is made up of 256 patterns. The pattern index for the first
    /// palette is 0 - 255, for the second palette 256 - 511, etc. 
    /// The x and y palette locations are used to store offset locations in 
    /// the palette for display
    /// </summary>
	public class TexturePalette : Record
	{
		#region Properties

        /// <summary>
        /// Texture file path.
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// Index, position of texture in list.
        /// </summary>
        public int Index
        {
            get;
            set;
        }

        /// <summary>
        /// Offset location in the palette (x,y).
        /// </summary>
        public int[] Location
        {
            get;
            set;
        }

		#endregion Properties

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctr
        /// </summary>
        /// <param name="parent"></param>
        //////////////////////////////////////////////////////////////////
        public TexturePalette( Record parent ) :
			base( parent, parent.Header )
		{            
		}

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses binary stream.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public override void Parse()
        {
            FileName = Encoding.ASCII.GetString( Header.Stream.Reader.ReadBytes( 200 ) ); 
            Index = Header.Stream.Reader.ReadInt32();
            Location = new int[] { Header.Stream.Reader.ReadInt32(), Header.Stream.Reader.ReadInt32() };            
        }
	}
}