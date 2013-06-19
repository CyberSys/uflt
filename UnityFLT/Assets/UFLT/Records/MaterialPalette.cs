using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

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
            set
            {
                Flags = ( int )( value ? ( Flags | -2147483648 ) : ( Flags & ~-2147483648 ) );
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
            ID = Encoding.ASCII.GetString( Header.Stream.Reader.ReadBytes( 12 ) ); // Name of material
            Flags = Header.Stream.Reader.ReadInt32();
            Ambient = new Color( Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle() );
            Diffuse = new Color( Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle() );
            Specular = new Color( Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle() );
            Emissive = new Color( Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle() );
            Shininess = Header.Stream.Reader.ReadSingle();
            Alpha = Header.Stream.Reader.ReadSingle();
        }
	}
}