using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UFLT.DataTypes.Enums;
using UFLT.Utils;

namespace UFLT.Records
{
    /// <summary>
    /// An externally referenced OpenFlight database file.
    /// </summary>
	public class ExternalReference : InterRecord
	{
		#region Properties
		
		/// <summary>
		/// Path to the referenced file.
		/// </summary>		
		public string Path
		{
			get;
			set;
		}
		
		/// <summary>
		/// An absolute path to the file generated if the file can be found.
		/// </summary>		
		public string AbsolutePath
		{
			get;
			set;
		}
		
      	/// <summary>
        /// Flags (bits, from left to right)
        ///  0 = Color palette override
		///  1 = Material palette override
		///  2 = Texture and texture mapping palette override
		///  3 = Line style palette override
		///  4 = Sound palette override
		///  5 = Light source palette override
		///  6 = Light point palette override
		///  7 = Shader palette override
		///  8-31 = Spare
        /// </summary>
        public int Flags
        {
            get;
            set;
        }

        /// <summary>
        /// Flags value
        /// Is the color palette overriden by the parent db?
        /// </summary>
        public bool FlagsColorPaletteOverridden
        {
            get
            {
                return ( Flags & -2147483648 ) != 0 ? true : false;
            }
        }

        /// <summary>
        /// Flags value
        /// Is the material palette overriden by the parent db?
        /// </summary>
        public bool FlagsMaterialPaletteOverridden
        {
            get
            {
                return ( Flags & 0x40000000 ) != 0 ? true : false;
            }
        }

        /// <summary>
        /// Flags value
        /// Is the texture palette & texture mapping overridden by the parent db.
        /// </summary>
        public bool FlagsTexturePaletteOverridden
        {
            get
            {
                return ( Flags & 0x20000000 ) != 0 ? true : false;
            }
          }

        /// <summary>
        /// Flags value        
        /// Is the line style palette overriden by the parent db.
        /// </summary>
        public bool FlagsLineStylePaletteOverridden
        {
            get
            {
                return ( Flags & 0x10000000 ) != 0 ? true : false;
            }
		}
        
        /// <summary>
        /// Flags value        
        /// Is the sound palette overridden by the parent db?
        /// </summary>
        public bool FlagsSoundPaletteOverridden
        {
            get
            {
                return ( Flags & 0x8000000 ) != 0 ? true : false;
            }     
		}        

        /// <summary>
        /// Flags value
        /// Hidden face, not drawn.
        /// </summary>
        public bool FlagsLightSourcePalette
        {
            get
            {
                return ( Flags & 0x4000000 ) != 0 ? true : false;
            }
        }

        /// <summary>
        /// Flags value
        /// </summary>
        public bool FlagsRoofline
        {
            get
            {
                return ( Flags & 0x2000000 ) != 0 ? true : false;
            }
        }		
		
		/// <summary>
		/// View external reference as bounding box(true) or normal(false).
		/// </summary>		
		public bool ViewAsBoundingBox
		{
			get;
			set;
		}
		
		/// <summary>
		/// The external reference.
		/// </summary>		
		public Database Reference
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
        public ExternalReference( Record parent ) :
			base( parent, parent.Header )
		{
            RootHandler.Handler[Opcodes.Matrix] = HandleMatrix;             
            RootHandler.ThrowBacks.UnionWith( RecordHandler.ThrowBackOpcodes );
		}

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses binary stream.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public override void Parse()
        {            
			Path = NullTerminatedString.GetAsString( Header.Stream.Reader.ReadBytes( 200 ) );
			Header.Stream.Reader.BaseStream.Seek( 4, SeekOrigin.Current ); // Skip reserved.
			Flags = Header.Stream.Reader.ReadInt32();
			ViewAsBoundingBox = Header.Stream.Reader.ReadInt16() == 1 ? true : false;
			
			// Find the file
			AbsolutePath = FileFinder.Instance.Find( Path );
			if( AbsolutePath != string.Empty )
			{
				ID = "Ref: " + Path;
				Reference = new Database( AbsolutePath, this, Header.Settings );
				
				// Override 
				if( FlagsColorPaletteOverridden ) Reference.ColorPalette = Header.ColorPalette;
				if( FlagsMaterialPaletteOverridden ) Reference.MaterialPalettes = Header.MaterialPalettes;
				if( FlagsTexturePaletteOverridden ) Reference.TexturePalettes = Header.TexturePalettes;
				// TODO: Implment overrides for other records that are not currently implemented.		
				
				if( FlagsMaterialPaletteOverridden || FlagsTexturePaletteOverridden )
				{
					Reference.MaterialBank = Header.MaterialBank; // Share material bank.	
				}													
				
				Reference.Parse();
			}
			else
			{
				ID = "Broken Ref: " + Path;
				Log.WriteError( "Could not find external reference: " + Path );						
			}
			
            base.Parse();
        }
	}
}