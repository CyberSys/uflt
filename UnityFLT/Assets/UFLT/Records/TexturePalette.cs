using UnityEngine;
using System.Collections.Generic;
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
		
		/// <summary>
		/// Materials that use this texture. Key is material id, -1 for default material.
		/// </summary>		
		private Dictionary<int, Material> Materials
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
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns a material that uses the texture. Material id of -1 indicates a default material.
		/// </summary>		
		/// <param name='materialID'>Material ID</param>
		//////////////////////////////////////////////////////////////////
		public Material GetOfCreateMaterial( int materialID )
		{
			// Does one exist?
			if( Materials.ContainsKey( materialID ) )
			{
				return Materials[materialID];	
			}
			
			// Create a new material for this texture
			// TODO: you are here
			return null;
			
			
			
			
			
			
			
			
			
			
			
			
			
			
			
		}
	}
}