using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UFLT.DataTypes.Enums;

namespace UFLT.Records
{
    /// <summary>
    /// A vertex with color data.
    /// </summary>
	public class Object : InterRecord
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
        public Object( Record parent ) :
			base( parent, parent.Header )
		{
            RootHandler.Handler[Opcodes.PushLevel] = HandlePush;
            RootHandler.Handler[Opcodes.LongID] = HandleLongID;
            RootHandler.Handler[Opcodes.Comment] = HandleComment; 
            
            // TODO: Matrix

            RootHandler.ThrowBacks.UnionWith( RecordHandler.ThrowBackOpcodes );

            // TODO: Handle children
		}

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses binary stream.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public override void Parse()
        {
            // TODO: You are here
        }
	}
}