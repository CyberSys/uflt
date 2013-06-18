using UFLT.DataTypes.Enums;
using System.IO;
using System.Text;
using UFLT.Streams;
using UnityEngine;

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
        ///  Delta to place the database (x,y).
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

            Opcode = Opcodes.DB;

            // Record the path for when we need to search for textures etc.
            FileFinder.Instance.AddPath( file );

            // Register handlers for this record type
            RootHandler.Handler[Opcodes.Header] = HandleHeader;
            RootHandler.Handler[Opcodes.PushLevel] = HandlePush;
            RootHandler.Handler[Opcodes.LongID] = HandleLongID;
            RootHandler.Handler[Opcodes.Comment] = HandleComment;

            ChildHandler.Handler[Opcodes.PushLevel] = HandlePush;
            ChildHandler.Handler[Opcodes.PopLevel] = HandlePop;

        }
        
        /*
        public IEnumerator ParseAsynchronously()
        {
            // TODO: seperate thread to load file, callback function or flag to indicate finished, maybe work as a Coroutine?
        }
        */

        #region Record Handlers

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a header record.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        private bool HandleHeader()
        {            
            ID = Encoding.ASCII.GetString( Stream.Reader.ReadBytes( 8 ) );
            FormatRevisionLevel = Stream.Reader.ReadInt32();
            EditRevisionLevel = Stream.Reader.ReadInt32();
            DateTimeLastRevision = Encoding.ASCII.GetString( Stream.Reader.ReadBytes( 32 ) );
            NextGroupNodeID = Stream.Reader.ReadInt16();
            NextLODNodeID = Stream.Reader.ReadInt16();
            NextObjectNodeID = Stream.Reader.ReadInt16();
            NextFaceNodeID = Stream.Reader.ReadInt16();
            UnitMultiplier = Stream.Reader.ReadInt16();
            VertexCoordinateUnits = ( VertexCoordinateUnits )Stream.Reader.ReadByte();
            TexWhite = Stream.Reader.ReadBoolean();
            Flags = Stream.Reader.ReadInt32();
            Stream.Reader.BaseStream.Seek( 24, SeekOrigin.Current ); // Skip reserved bytes
            ProjectionType = ( Projection )Stream.Reader.ReadInt32();
            Stream.Reader.BaseStream.Seek( 28, SeekOrigin.Current ); // Skip reserved bytes
            NextDegreeOfFreedomNodeID = Stream.Reader.ReadInt16();
            VertexStorageType = ( VertexStorageType )Stream.Reader.ReadInt16();
            DatabaseOrigin = ( DatabaseOrigin )Stream.Reader.ReadInt32();
            SouthwestDatabaseCoordinate = new double[] { Stream.Reader.ReadDouble(), Stream.Reader.ReadDouble() };
            DeltaToPlaceDatabase = new double[] { Stream.Reader.ReadDouble(), Stream.Reader.ReadDouble() };
            NextSoundNodeID = Stream.Reader.ReadInt16();
            NextPathNodeID = Stream.Reader.ReadInt16();
            Stream.Reader.BaseStream.Seek( 8, SeekOrigin.Current ); // Skip reserved bytes
            NextClipNodeID = Stream.Reader.ReadInt16();
            NextTextNodeID = Stream.Reader.ReadInt16();
            NextBSPNodeID = Stream.Reader.ReadInt16();
            NextSwitchNodeID = Stream.Reader.ReadInt16();
            Stream.Reader.BaseStream.Seek( 4, SeekOrigin.Current ); // Skip reserved bytes


            // TODO: sw corner lat/lon


            // TODO: Check it is safe to create a Vector in a seperate thread, we want the parsing part to be thread safe for future performance improvements. E.G load async etc.
  
            return true;
        }

        #endregion Record Handlers
    }
}