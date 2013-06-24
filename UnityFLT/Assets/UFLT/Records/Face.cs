using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UFLT.DataTypes.Enums;

namespace UFLT.Records
{
    /// <summary>
    /// A face contains attributes describing the visual state of its child vertices.
    /// Only vertex and morph vertex nodes may be children of faces
    /// </summary>
	public class Face : Record
	{
		#region Properties

        /// <summary>
        /// IR Color Code
        /// </summary>
        public int IRColorCode
        {
            get;
            set;
        }
        
        /// <summary>
        /// Relative priority specifies a fixed ordering of the Face relative to its sibling nodes. Ordering is 
        /// from left (lesser values) to right (higher values). Nodes of equal priority may be arbitrarily ordered.
        /// All nodes have an implicit (default) value of zero.
        /// </summary>
        public int RelativePriority
        {
            get;
            set;
        }

        /// <summary>
        /// Face draw type.
        /// </summary>
        public DrawType DrawType
        {
            get;
            set;
        }

        /// <summary>
        /// If TRUE, draw textured face white.
        /// </summary>
        public bool TexWhite
        {
            get;
            set;
        }

        /// <summary>
        /// Color name index.
        /// </summary>
        public ushort ColorNameIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Color name index or -1 if none.
        /// </summary>
        public ushort AlternativeColorNameIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Template (Billboard)
        /// </summary>
        public TemplateBillboard TemplateBillboard
        {
            get;
            set;
        }

        /// <summary>
        /// Detail texture pattern or -1 if none.
        /// </summary>
        public short DetailTexturePattern
        {
            get;
            set;
        }
        
        /// <summary>
        /// Texture pattern or -1 if none.
        /// </summary>
        public short TexturePattern
        {
            get;
            set;
        }

        /// <summary>
        /// Material index or -1 if none.
        /// </summary>
        public short MaterialIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Surface material code (for DFAD)
        /// </summary>
        public short SurfaceMaterialCode
        {
            get;
            set;
        }

        /// <summary>
        /// Feature ID(for DFAD)
        /// </summary>
        public short FeatureID
        {
            get;
            set;
        }

        /// <summary>
        /// IR material code.
        /// </summary>
        public int IRMaterialCode
        {
            get;
            set;
        }

        /// <summary>
        /// Transparency
        /// 0 = Opaque
        /// 65535 = Totally clear
        /// </summary>
        public ushort Transperancey
        {
            get;
            set;
        }

        /// <summary>
        /// Level Of Detail Generation Control.
        /// </summary>
        public byte LODGenerationControl
        {
            get;
            set;
        }

        /// <summary>
        /// Line style index.
        /// </summary>
        public byte LineStyleIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Flags (bits, from left to right)
        ///  0 = Terrain
        ///  1 = No color
        ///  2 = No alternate color
        ///  3 = Packed color
        ///  4 = Terrain culture cutout (footprint)
        ///  5 = Hidden, not drawn
        ///  6 = Roofline
        ///  7-31 = Spare
        /// </summary>
        public int Flags
        {
            get;
            set;
        }

        /// <summary>
        /// Flags value
        /// Is the face part of the terrain?
        /// </summary>
        public bool FlagsTerrain
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
        /// Flags value
        /// </summary>
        public bool FlagsNoColor
        {
            get
            {
                return ( Flags & 0x40000000 ) != 0 ? true : false;
            }
            set
            {
                Flags = ( int )( value ? ( Flags | 0x40000000 ) : ( Flags & ~0x40000000 ) );
            }
        }

        /// <summary>
        /// Flags value
        /// </summary>
        public bool FlagsNoAlternateColor
        {
            get
            {
                return ( Flags & 0x20000000 ) != 0 ? true : false;
            }
            set
            {
                Flags = ( int )( value ? ( Flags | 0x20000000 ) : ( Flags & ~0x20000000 ) );
            }
        }

        /// <summary>
        /// Flags value        
        /// </summary>
        public bool FlagsPackedColor
        {
            get
            {
                return ( Flags & 0x10000000 ) != 0 ? true : false;
            }
            set
            {
                Flags = ( int )( value ? ( Flags | 0x10000000 ) : ( Flags & ~0x10000000 ) );
            }
        }
        
        /// <summary>
        /// Flags value        
        /// Terrain culture cutout (footprint)
        /// </summary>
        public bool FlagsTerrainCultureCutout
        {
            get
            {
                return ( Flags & 0x8000000 ) != 0 ? true : false;
            }
            set
            {
                Flags = ( int )( value ? ( Flags | 0x8000000 ) : ( Flags & ~0x8000000 ) );
            }
        }        

        /// <summary>
        /// Flags value
        /// Hidden face, not drawn.
        /// </summary>
        public bool FlagsHidden
        {
            get
            {
                return ( Flags & 0x4000000 ) != 0 ? true : false;
            }
            set
            {
                Flags = ( int )( value ? ( Flags | 0x4000000 ) : ( Flags & ~0x4000000 ) );
            }
        }

        /// <summary>
        /// Flags value
        /// </summary>
        public bool FlagsRoofline
        {
            get
            {
                return ( Flags & 0x4000000 ) != 0 ? true : false;
            }
            set
            {
                Flags = ( int )( value ? ( Flags | 0x4000000 ) : ( Flags & ~0x4000000 ) );
            }
        }
               
        /// <summary>
        /// Light mode.
        /// </summary>
        public LightMode LightMode
        {
            get;
            set;
        }

        /// <summary>
        /// Packed color primary - only b, g, r used
        /// </summary>
        public Color32 PackedColorPrimary
        {
            get;
            set;
        }

        /// <summary>
        /// Packed color alternate - only b, g, r used.
        /// </summary>
        public Color32 PackedColorAlternate
        {
            get;
            set;
        }

        // TODO: You are here
   



		#endregion Properties

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctr
        /// </summary>
        /// <param name="parent"></param>
        //////////////////////////////////////////////////////////////////
        public Face( Record parent ) :
			base( parent, parent.Header )
		{
            RootHandler.Handler[Opcodes.PushLevel] = HandlePush;
            RootHandler.Handler[Opcodes.Comment] = HandleComment;
            
            RootHandler.ThrowBacks.UnionWith( RecordHandler.ThrowBackOpcodes );
            
            ChildHandler.Handler[Opcodes.PushLevel] = HandlePush;
            ChildHandler.Handler[Opcodes.PopLevel] = HandlePop;
            ChildHandler.Handler[Opcodes.VertexList] = HandleVertexList;
		}

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses binary stream.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public override void Parse()
        {
            //ID = Encoding.ASCII.GetString( Header.Stream.Reader.ReadBytes( 8 ) );
            //Flags = Header.Stream.Reader.ReadInt32();
            //RelativePriority = Header.Stream.Reader.ReadInt16();
            //Transparency = Header.Stream.Reader.ReadUInt16();
            //SpecialEffectID1 = Header.Stream.Reader.ReadInt16();
            //SpecialEffectID2 = Header.Stream.Reader.ReadInt16();
            //// Ignore last 2 reserved bytes.            
        }
	}
}