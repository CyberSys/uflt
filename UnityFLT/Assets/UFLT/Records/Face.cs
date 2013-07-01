using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UFLT.DataTypes.Enums;
using System.Linq;
using UFLT.Utils;

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
        public ushort Transparency
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

        /// <summary>
        /// Texture mapping index or -1 if none.
        /// </summary>
        public short TextureMappingIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Primary color index or -1 if none.
        /// </summary>
        public uint PrimaryColorIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Alternate color index or -1 if none.
        /// </summary>
        public uint AlternateColorIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Shader index or -1 if none.
        /// </summary>
        public short ShaderIndex
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
            ID                        = Encoding.ASCII.GetString( Header.Stream.Reader.ReadBytes( 8 ) );
            IRColorCode               = Header.Stream.Reader.ReadInt32();
            RelativePriority          = Header.Stream.Reader.ReadInt16();
            DrawType                  = ( DrawType )Header.Stream.Reader.ReadSByte();
            TexWhite                  = Header.Stream.Reader.ReadBoolean();
            ColorNameIndex            = Header.Stream.Reader.ReadUInt16();
            AlternateColorIndex       = Header.Stream.Reader.ReadUInt16();
            /* Skip reserved bytes*/  Header.Stream.Reader.BaseStream.Seek( 1, SeekOrigin.Current );
            TemplateBillboard         = ( TemplateBillboard )Header.Stream.Reader.ReadSByte();
            DetailTexturePattern      = Header.Stream.Reader.ReadInt16();
            TexturePattern            = Header.Stream.Reader.ReadInt16();
            MaterialIndex             = Header.Stream.Reader.ReadInt16();
            SurfaceMaterialCode       = Header.Stream.Reader.ReadInt16();
            FeatureID                 = Header.Stream.Reader.ReadInt16();
            IRMaterialCode            = Header.Stream.Reader.ReadInt32();
            Transparency             = Header.Stream.Reader.ReadUInt16();
            LODGenerationControl      = Header.Stream.Reader.ReadByte();
            LineStyleIndex            = Header.Stream.Reader.ReadByte();
            Flags                     = Header.Stream.Reader.ReadInt32();
            LightMode                 = ( LightMode )Header.Stream.Reader.ReadByte();
            /* Skip reserved bytes*/  Header.Stream.Reader.BaseStream.Seek( 7, SeekOrigin.Current );
            Color32 c                 = new Color32();                        
            c.a                       = Header.Stream.Reader.ReadByte();
            c.b                       = Header.Stream.Reader.ReadByte();
            c.g                       = Header.Stream.Reader.ReadByte();
            c.r                       = Header.Stream.Reader.ReadByte();
            PackedColorPrimary        = c;
            c.a                       = Header.Stream.Reader.ReadByte();
            c.b                       = Header.Stream.Reader.ReadByte();
            c.g                       = Header.Stream.Reader.ReadByte();
            c.r                       = Header.Stream.Reader.ReadByte();
            PackedColorAlternate      = c;
            TextureMappingIndex       = Header.Stream.Reader.ReadInt16();
            /* Skip reserved bytes*/  Header.Stream.Reader.BaseStream.Seek( 2, SeekOrigin.Current );
            PrimaryColorIndex         = Header.Stream.Reader.ReadUInt32();
            AlternateColorIndex       = Header.Stream.Reader.ReadUInt32();
            /* Skip reserved bytes*/  Header.Stream.Reader.BaseStream.Seek( 2, SeekOrigin.Current );
            ShaderIndex               = Header.Stream.Reader.ReadInt16();

            // Parse children
            base.Parse();
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Prepares the vertices and triangulates the faces ready for creating a mesh.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public override void PrepareForImport()
		{
            if( Parent is InterRecord )
            {
				// Do we draw this face?
				if( FlagsHidden )
				{
					return;	
				}

                // TODO: Have a seperate set of triangles for each material.
				// TODO: Calc face normal. Normal = sum of points normals then normalised.
                




                InterRecord ir = Parent as InterRecord;
                                
                if( ir.VertexPositions == null )
                {
                    ir.VertexPositions = new List<Vector3>();
                }

                if( ir.Triangles == null )
                {
                    ir.Triangles = new List<int>();
                }

                // Find vertex list
                VertexList vl = Children.Find( o => o is VertexList ) as VertexList;
                if( vl != null )
                {
                    int startIndex = ir.VertexPositions.Count;

                    // Collect verts from the palette                    
                    foreach( int offset in vl.Offsets )
                    {
                        VertexWithColor vwc = Header.VertexPalette.Vertices[offset];

                        // Extract position
                        ir.VertexPositions.Add( new Vector3( ( float )vwc.Coordinate[0], ( float )vwc.Coordinate[1], ( float )vwc.Coordinate[2] ) ); // We lose precision here 

                        // TODO: uv, etc
                    }                    

                    if( vl.Offsets.Count != 3 )
                    {
                        // Its not a triangle, trianglulate it                        

                        Triangulator triangulator = new Triangulator();

                        triangulator.initTriangulator( ir.VertexPositions.GetRange( startIndex, vl.Offsets.Count ), Vector3.one );
                        ir.Triangles.AddRange( triangulator.Triangulate( startIndex ) );

                        //ir.Triangles.AddRange( new int[] { startIndex, startIndex + 1, startIndex + 2, startIndex + 2, startIndex + 3, startIndex } );                        
                    }
                    else
                    {
                        ir.Triangles.AddRange( new int[] { startIndex, startIndex + 1, startIndex + 2 } );                        
                    }

                    // TODO: Trianglulate. Can we do most of the work in a seperate thread?
                    // TODO: Create verts and triangles first and then finalise the mesh in the object or do it all in the object?
                    
                }
                else
                {
					Log.WriteWarning( "Could not find vertex list for face" );
                }                              
            }
            else
            {
				Log.WriteWarning( "Face is not a child of a InterRecord, can not create face." );
            }


            //base.ImportIntoScene();
        }
	}
}