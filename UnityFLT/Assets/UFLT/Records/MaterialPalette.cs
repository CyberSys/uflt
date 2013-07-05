using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UFLT.Utils;

namespace UFLT.Records
{
    /// <summary>
    /// The material palette contains descriptions of “standard” materials used while drawing geometry.
    /// </summary>
	public class MaterialPalette : Record
	{
		#region Properties

        /// <summary>
        /// Index, position of material in list.
        /// </summary>
        public int Index
        {
            get;
            set;
        }

        /// <summary>
        /// Flags (bits, from left to right)
        ///   0 = Used
        ///   1-31 = Spare
        /// </summary>
        public int Flags
        {
            get;
            set;
        }

        /// <summary>
        /// Flags value, indicates the material is used.
        /// </summary>
        public bool FlagsUsedMaterial
        {
            get
            {
                return ( Flags & -2147483648 ) != 0 ? true : false;
            }
        }

        /// <summary>
        /// Ambient color.
        /// </summary>
        public Color Ambient
        {
            get;
            set;
        }

        /// <summary>
        /// Diffuse color.
        /// </summary>
        public Color Diffuse
        {
            get;
            set;
        }

        /// <summary>
        /// Specular color. 
        /// </summary>
        public Color Specular
        {
            get;
            set;
        }
        
        /// <summary>
        /// Emissive color. 
        /// </summary>
        public Color Emissive
        {
            get;
            set;
        }

        /// <summary>
        /// Specular highlights are tighter, with higher shininess values.
        /// </summary>
        public float Shininess
        {
            get;
            set;
        }

        /// <summary>
        /// An alpha of 1.0 is fully opaque, while 0.0 is fully transparent.
        /// Final alpha = material alpha * (1.0- (geometry transparency / 65535))
        /// </summary>
        public float Alpha
        {
            get;
            set;
        }
		
		/// <summary>
		/// The Unity material.
		/// </summary>		
		public Material UnityMaterial
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
        public MaterialPalette( Record parent ) :
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
            Index = Header.Stream.Reader.ReadInt32();
            ID = NullTerminatedString.GetAsString( Header.Stream.Reader.ReadBytes( 12 ) ); // Name of material
            Flags = Header.Stream.Reader.ReadInt32();
            Ambient = new Color( Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle() );
            Diffuse = new Color( Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle() );
            Specular = new Color( Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle() );
            Emissive = new Color( Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle() );
			
			Shininess = Header.Stream.Reader.ReadSingle(); // Also apply it to the alpha channel.
			Specular = new Color( Specular.r, Specular.g, Specular.b, Shininess / 128f ); // Use the shininess in the specular alpha channel.
			
            Alpha = Header.Stream.Reader.ReadSingle();
        }
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Determines whether the specified <see cref="MaterialPalette"/> is equal to the current <see cref="UFLT.Records.MaterialPalette"/>.
		/// </summary>
		/// <param name='other'>
		/// The <see cref="MaterialPalette"/> to compare with the current <see cref="UFLT.Records.MaterialPalette"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="MaterialPalette"/> is equal to the current
		/// <see cref="UFLT.Records.MaterialPalette"/>; otherwise, <c>false</c>.
		/// </returns>
		//////////////////////////////////////////////////////////////////
		public bool Equals( MaterialPalette other )
		{
			// First check index, if it matches and we both belong to the same db then we are the same.
			if( Index  != other.Index  )return false;
			if( Header == other.Header )return true; // Same index and same db, no need to check the rest.
			
			// Name is not important, if 2 materials have different names but the same values it makes more sense to combine them at this stage.
			// Flags also has no impact on appearance so lets ignore that too.
			
			// Check color fields
			if( !Ambient.Equals( other.Ambient )   ) return false;
			if( !Diffuse.Equals( other.Diffuse )   ) return false;
			if( !Specular.Equals( other.Specular ) ) return false;
			if( !Emissive.Equals( other.Emissive ) ) return false;
			
			if( Mathf.Approximately( Shininess, other.Shininess ) ) return false;
			if( Mathf.Approximately( Alpha, other.Alpha )         ) return false;			
				
			return true;					
		}		
	}
}