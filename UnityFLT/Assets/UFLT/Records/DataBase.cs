using UFLT.DataTypes.Enums;
using System.IO;
using System.Text;
using UFLT.Streams;
using UnityEngine;
using System.Collections.Generic;
using UFLT.Utils;

namespace UFLT.Records
{
    /// <summary>
    /// Primary record for an OpenFlight file.
    /// </summary>
    public class Database : InterRecord
    {
        #region Properties

        /// <summary>
        /// Stream for this file
        /// </summary>
        public InStream Stream
        {
            get;
            set;
        }

        #region Palettes

        /// <summary>
        /// Color palette for this database.
        /// </summary>
        public ColorPalette ColorPalette
        {
            get;
            set;
        }

        /// <summary>
        /// Vertex palette for this database.
        /// </summary>
        public VertexPalette VertexPalette
        {
            get;
            set;
        }
        
        /// <summary>
        /// Texture palettes with index as key.
        /// </summary>
        public Dictionary<int, TexturePalette> TexturePalettes
        {
            get;
            set;
        }

        /// <summary>
        /// Material palettes with index as key.
        /// </summary>
        public Dictionary<int, MaterialPalette> MaterialPalettes
        {
            get;
            set;
        }


        #endregion Palettes

        #region Header

        /// <summary>
        /// The version of OpenFlight, e.g 1640 = 16.4
        /// </summary>
        public int FormatRevisionLevel
        {
            get;
            set;
        }

        /// <summary>
        /// The edit revision level
        /// </summary>
        public int EditRevisionLevel
        {
            get;
            set;
        }

        /// <summary>
        /// The date and time the file was last updated.
        /// </summary>
        public string DateTimeLastRevision
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next group node.
        /// </summary>
        public short NextGroupNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next Level Of Detail node.
        /// </summary>
        public short NextLODNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next object node.
        /// </summary>
        public short NextObjectNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next face node.
        /// </summary>
        public short NextFaceNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// Unit multiplier.
        /// </summary>
        public short UnitMultiplier
        {
            get;
            set;
        }

        /// <summary>
        /// Units of measurement used for vertex coordinates. 
        /// </summary>
        public VertexCoordinateUnits VertexCoordinateUnits
        {
            get;
            set;
        }

        /// <summary>
        /// Should new faces be white?
        /// </summary>
        public bool TexWhite
        {
            get;
            set;
        }

        /// <summary>
        /// Flags (bits, from left to right)
        ///   0 = Save vertex normals
        ///   1 = Packed Color mode
        ///   2 = CAD View mode
        ///   3-31 = Spare
        /// </summary>
        public int Flags
        {
            get;
            set;
        }

        /// <summary>
        /// Flags value
        /// </summary>
        public bool FlagsSaveVertexNormals
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
        public bool FlagsPackedColorMode
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
        public bool FlagsCADViewMode
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
        /// Projection, only really applies if the OpenFlight file is a terrain.
        /// </summary>
        public Projection ProjectionType
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next DOF node.
        /// </summary>
        public short NextDegreeOfFreedomNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// Vertex storage type, always double(1).
        /// </summary>
        public VertexStorageType VertexStorageType
        {
            get;
            set;
        }

        /// <summary>
        /// Origin of the db.
        /// </summary>
        public DatabaseOrigin DatabaseOrigin
        {
            get;
            set;
        }

        /// <summary>
        ///  Southwest Database Coordinate (x,y).
        /// </summary>
        public double[] SouthwestDatabaseCoordinate
        {
            get;
            set;
        }

        /// <summary>
        ///  Delta to place the database (x,y,z).
        /// </summary>
        public double[] DeltaToPlaceDatabase
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next sound node.
        /// </summary>
        public short NextSoundNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next path node.
        /// </summary>
        public short NextPathNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next clip node.
        /// </summary>
        public short NextClipNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next text node.
        /// </summary>
        public short NextTextNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next BSP node.
        /// </summary>
        public short NextBSPNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next switch node.
        /// </summary>
        public short NextSwitchNodeID
        {
            get;
            set;
        }
        
        /// <summary>
        /// South west corner latitude and longitude.
        /// </summary>
        public double[] SouthWestCornerLatLon
        {
            get;
            set;
        }

        /// <summary>
        /// North east corner latitude and longitude.
        /// </summary>
        public double[] NorthEastCornerLatLon
        {
            get;
            set;
        }

        /// <summary>
        /// Origin latitude and longitude.
        /// </summary>
        public double[] OriginLatLon
        {
            get;
            set;
        }

        /// <summary>
        /// Lambert upper latitude and longitude.
        /// </summary>
        public double[] LambertLatLon
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next light source node.
        /// </summary>
        public short NextLightSourceNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next light point node.
        /// </summary>
        public short NextLightPointNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next road node.
        /// </summary>
        public short NextRoadNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next Continuously Adaptive Terrain node.
        /// </summary>
        public short NextCATNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// Earth Ellipsoid Model.
        /// </summary>
        public EarthEllipsoidModel EarthEllipsoidModel
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next adaptive node.
        /// </summary>
        public short NextAdaptiveNodeID
        {
            get;
            set;
        }
        
        /// <summary>
        /// ID number of the next curve node.
        /// </summary>
        public short NextCurveNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// UTM zone (for UTM projections - negative value means Southern hemisphere)
        /// </summary>
        public short UTMZone
        {
            get;
            set;
        }

        /// <summary>
        /// Radius (distance from database origin to farthest corner)
        /// </summary>
        public double Radius
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next curve node.
        /// </summary>
        public ushort NextMeshNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// ID number of the next curve node.
        /// </summary>
        public ushort NextLightPointSystemNodeID
        {
            get;
            set;
        }

        /// <summary>
        /// Earth major and minor axis (for user defined ellipsoid) in meters.
        /// </summary>
        public double[] EarthAxis
        {
            get;
            set;
        }

        #endregion Header
        
        #endregion Properties

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctor
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public Database( string file ) :
            this( file, null )
        {
            // TODO: Options class. 
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctor, a DataBase may have a parent if it is part of an external reference node.
        /// </summary>
        /// <param name="file"></param>
        //////////////////////////////////////////////////////////////////
        public Database( string file, Record parent )            
        {			
            Stream = new InStream( file );
            Header = this;
            Parent = parent;
            if( parent != null )
            {
                parent.Children.Add( this );
            }
			else
			{
				// Init log
				Log.Init();				
			}
			
			Log.Write( "Loading file: " + file );
            
            MaterialPalettes = new Dictionary<int, MaterialPalette>();
            TexturePalettes = new Dictionary<int, TexturePalette>();

            Opcode = Opcodes.DB;

            // Record the path for when we need to search for textures etc.
            FileFinder.Instance.AddPath( file );

            // Register handlers for this record type
            RootHandler.Handler[Opcodes.Header] = HandleHeader;
            RootHandler.Handler[Opcodes.PushLevel] = HandlePush;
            RootHandler.Handler[Opcodes.LongID] = HandleLongID;
            RootHandler.Handler[Opcodes.Comment] = HandleComment;
            RootHandler.Handler[Opcodes.ColorPalette] = HandleColorPalette;
            RootHandler.Handler[Opcodes.TexturePalette] = HandleTexturePalette;
            RootHandler.Handler[Opcodes.VertexPalette] = HandleVertexPalette;
            RootHandler.Handler[Opcodes.MaterialPalette] = HandleMaterialPalette;

            ChildHandler.Handler[Opcodes.PushLevel] = HandlePush;
            ChildHandler.Handler[Opcodes.PopLevel] = HandlePop;
            ChildHandler.Handler[Opcodes.Object] = HandleObject;                        
            ChildHandler.Handler[Opcodes.Switch] = HandleUnhandled;
            ChildHandler.Handler[Opcodes.Sound] = HandleUnhandled;
            ChildHandler.Handler[Opcodes.ClipRegion] = HandleUnhandled;
            ChildHandler.Handler[Opcodes.DegreeOfFreedom] = HandleUnhandled;
            ChildHandler.Handler[Opcodes.Group] = HandleGroup;

            ChildHandler.Handler[Opcodes.LevelOfDetail] = HandleUnhandled;

            // TODO: lod, external ref
        }
        
        /*
        public IEnumerator ParseAsynchronously()
        {
            // TODO: seperate thread to load file, callback function or flag to indicate finished, maybe work as a Coroutine?
        }
        */
		
		public override void ImportIntoScene ()
		{
			base.ImportIntoScene();
			
			if( Parent == null )
			{
				// TODO: Convert between left and right hand coordinate systems
				// Rotate so z is up
				Object.transform.Rotate( new Vector3( 270, 180, 0 ) );
			}
		}

        #region Record Handlers        

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a header record.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        private bool HandleHeader()
        {            
            ID                          = Encoding.ASCII.GetString( Stream.Reader.ReadBytes( 8 ) );
            FormatRevisionLevel         = Stream.Reader.ReadInt32();
            EditRevisionLevel           = Stream.Reader.ReadInt32();
            DateTimeLastRevision        = Encoding.ASCII.GetString( Stream.Reader.ReadBytes( 32 ) );
            NextGroupNodeID             = Stream.Reader.ReadInt16();
            NextLODNodeID               = Stream.Reader.ReadInt16();
            NextObjectNodeID            = Stream.Reader.ReadInt16();
            NextFaceNodeID              = Stream.Reader.ReadInt16();
            UnitMultiplier              = Stream.Reader.ReadInt16();
            VertexCoordinateUnits       = ( VertexCoordinateUnits )Stream.Reader.ReadByte();
            TexWhite                    = Stream.Reader.ReadBoolean();
            Flags                       = Stream.Reader.ReadInt32();
            /* Skip reserved bytes*/      Stream.Reader.BaseStream.Seek( 24, SeekOrigin.Current ); 
            ProjectionType              = ( Projection )Stream.Reader.ReadInt32();
            /* Skip reserved bytes*/    Stream.Reader.BaseStream.Seek( 28, SeekOrigin.Current ); 
            NextDegreeOfFreedomNodeID   = Stream.Reader.ReadInt16();
            VertexStorageType           = ( VertexStorageType )Stream.Reader.ReadInt16();
            DatabaseOrigin              = ( DatabaseOrigin )Stream.Reader.ReadInt32();
            SouthwestDatabaseCoordinate = new double[] { Stream.Reader.ReadDouble(), Stream.Reader.ReadDouble() };
            DeltaToPlaceDatabase        = new double[] { Stream.Reader.ReadDouble(), Stream.Reader.ReadDouble(), 0 }; // z is read later in the stream
            NextSoundNodeID             = Stream.Reader.ReadInt16();
            NextPathNodeID              = Stream.Reader.ReadInt16();
            /* Skip reserved bytes*/    Stream.Reader.BaseStream.Seek( 8, SeekOrigin.Current ); 
            NextClipNodeID              = Stream.Reader.ReadInt16();
            NextTextNodeID              = Stream.Reader.ReadInt16();
            NextBSPNodeID               = Stream.Reader.ReadInt16();
            NextSwitchNodeID            = Stream.Reader.ReadInt16();
            /* Skip reserved bytes*/    Stream.Reader.BaseStream.Seek( 4, SeekOrigin.Current ); 
            SouthWestCornerLatLon       = new double[] { Stream.Reader.ReadDouble(), Stream.Reader.ReadDouble() };
            NorthEastCornerLatLon       = new double[] { Stream.Reader.ReadDouble(), Stream.Reader.ReadDouble() };
            OriginLatLon                = new double[] { Stream.Reader.ReadDouble(), Stream.Reader.ReadDouble() };
            LambertLatLon               = new double[] { Stream.Reader.ReadDouble(), Stream.Reader.ReadDouble() };
            NextLightSourceNodeID       = Stream.Reader.ReadInt16();
            NextLightPointNodeID        = Stream.Reader.ReadInt16();
            NextRoadNodeID              = Stream.Reader.ReadInt16();
            NextCATNodeID               = Stream.Reader.ReadInt16();
            /* Skip reserved bytes*/    Stream.Reader.BaseStream.Seek( 8, SeekOrigin.Current );
            EarthEllipsoidModel         = ( EarthEllipsoidModel )Stream.Reader.ReadInt32();
            NextAdaptiveNodeID          = Stream.Reader.ReadInt16();
            NextCurveNodeID             = Stream.Reader.ReadInt16();
            UTMZone                     = Stream.Reader.ReadInt16();
            /* Skip reserved bytes*/    Stream.Reader.BaseStream.Seek( 6, SeekOrigin.Current ); 
            DeltaToPlaceDatabase[2]     = Stream.Reader.ReadDouble(); // Read z
            NextMeshNodeID              = Stream.Reader.ReadUInt16();
            NextLightPointSystemNodeID  = Stream.Reader.ReadUInt16();
            /* Skip reserved bytes*/    Stream.Reader.BaseStream.Seek( 4, SeekOrigin.Current ); 
            EarthAxis                   = new double[] { Stream.Reader.ReadDouble(), Stream.Reader.ReadDouble() };  
            return true;
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handles the color palette.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        private bool HandleColorPalette()
        {
            ColorPalette = new ColorPalette( this );
            ColorPalette.Parse();
            return true;
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handle a texture palette, adds it to our collection.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        private bool HandleTexturePalette()
        {
            TexturePalette t = new TexturePalette( this );
            t.Parse();
            TexturePalettes[t.Index] = t;
            return true;
        }
                
        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handle a vertex palette.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        private bool HandleVertexPalette()
        {
            VertexPalette = new VertexPalette( this );
            VertexPalette.Parse();            
            return true;
        }       

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handle a material palete, adds it to our collection.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        private bool HandleMaterialPalette()
        {
            MaterialPalette m = new MaterialPalette( this );
            m.Parse();
            MaterialPalettes[m.Index] = m;
            return true;
        }

        #endregion Record Handlers
    }
}