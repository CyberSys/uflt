using UFLT.DataTypes.Enums;
using System.IO;
using System.Text;
using UFLT.Streams;

namespace UFLT.Records
{
    /// <summary>
    /// Primary record for an OpenFlight file.
    /// </summary>
    public class DataBase : InterRecord
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

        #endregion
        
        #endregion Properties

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctor
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public DataBase( string file ) :
            this( file, null )
        {
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctor, a DataBase may have a parent if it is part of an external reference node.
        /// </summary>
        /// <param name="file"></param>
        //////////////////////////////////////////////////////////////////
        public DataBase( string file, Record parent ) 
        {
            Stream = new InStream( file );
            Header = this;
            Parent = parent;
            Opcode = Opcodes.DB;
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Decode binary data.
        /// </summary>
        /// <param name="br"></param>
        //////////////////////////////////////////////////////////////////
        public override void Decode( BinaryReader br )
        {
            base.Decode( br );
            //Name = Encoding.ASCII.GetString( br.ReadBytes( 8 ) );
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Encode binary data writing to file
        /// </summary>
        /// <param name="bw"></param>
        //////////////////////////////////////////////////////////////////
        public override void Encode( BinaryWriter bw )
        {
            base.Encode( bw );
            //bw.Write( Encoding.ASCII.GetBytes( FixedString.GenerateFixedString( Name, 8 ) ) );
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        public override string ToString()
        {
            return "";
        }
    }
}