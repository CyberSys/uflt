using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

namespace UFLT.Records
{
    /// <summary>
    /// A vertex with color data.
    /// </summary>
	public class VertexWithColor : Record
	{
		#region Properties

        /// <summary>
        /// Color name index.
        /// </summary>
        public ushort ColorNameIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Flags (bits, from left to right)
        ///   0 = Start hard edge
        ///   1 = Normal frozen
        ///   2 = No color
        ///   3 = Packed color
        ///   4-15 = Spare
        /// </summary>
        public short Flags
        {
            get;
            set;
        }

        /// <summary>
        /// Flags value
        /// </summary>
        public bool FlagsStartHardEdge
        {
            get
            {
                return ( Flags & -0x8000 ) != 0 ? true : false;
            }
            set
            {
                Flags = ( short )( value ? ( Flags | -0x8000 ) : ( Flags & ~-0x8000 ) );
            }
        }

        /// <summary>
        /// Flags value
        /// </summary>
        public bool FlagsNormalFrozen
        {
            get
            {
                return ( Flags & 0x4000 ) != 0 ? true : false;
            }
            set
            {
                Flags = ( short )( value ? ( Flags | 0x4000 ) : ( Flags & ~0x4000 ) );
            }
        }

        /// <summary>
        /// Flags value
        /// </summary>
        public bool FlagsNoColor
        {
            get
            {
                return ( Flags & 0x2000 ) != 0 ? true : false;
            }
            set
            {
                Flags = ( short )( value ? ( Flags | 0x2000 ) : ( Flags & ~0x2000 ) );
            }
        }

        /// <summary>
        /// Flags value
        /// </summary>
        public bool FlagsPackedColor
        {
            get
            {
                return ( Flags & 0x1000 ) != 0 ? true : false;
            }
            set
            {
                Flags = ( short )( value ? ( Flags | 0x1000 ) : ( Flags & ~0x1000 ) );
            }
        }

        /// <summary>
        /// x,y,z position of vertex.
        /// </summary>
        public double[] Coordinate
        {
            get;
            set;
        }

        /// <summary>
        /// Packed color - always specified when the vertex has color.
        /// </summary>
        public Color32 PackedColor
        {
            get;
            set;
        }

        /// <summary>
        /// valid only if vertex has color and Packed color flag is not set.
        /// </summary>
        public uint VertexColorIndex
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
        public VertexWithColor( Record parent ) :
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
            ColorNameIndex = Header.Stream.Reader.ReadUInt16();
            Flags = Header.Stream.Reader.ReadInt16();
            Coordinate = new double[] { Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble() };
            
            Color32 c = new Color32();
            c.a = Header.Stream.Reader.ReadByte();
            c.b = Header.Stream.Reader.ReadByte();
            c.g = Header.Stream.Reader.ReadByte();
            c.r = Header.Stream.Reader.ReadByte();
            PackedColor = c;

            VertexColorIndex = Header.Stream.Reader.ReadUInt32();
        }
	}
}