using UnityEngine;
using System.Collections.Generic;
using UFLT.DataTypes.Enums;
using System.IO;

namespace UFLT.Records
{
    /// <summary>
    /// The base class for all OpenFlight records.
    /// </summary>
    public class Record
    {
        #region Properties

        /// <summary>
        /// The type of record
        /// </summary>
        public Opcodes Opcode
        {
            get;
            set;
        }

        /// <summary>
        /// The size of the entire record in bytes.
        /// </summary>
        public ushort Length
        {
            get;
            set;
        }

        /// <summary>
        /// Optional value, some records may have comments included. E.G DIS Enums for DOF records etc.
        /// </summary>
        public string Comment
        {
            get;
            set;
        }

        /// <summary>
        /// Record parent if one exists.
        /// </summary>
        public Record Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Child nodes of this record.
        /// </summary>
        public List<Record> Children
        {
            get;
            set;
        }

        /// <summary>
        /// The database that this record belongs to.
        /// </summary>
        public DataBase Header
        {
            get;
            set;
        }
        
        #endregion Properties

        #region Handlers

        /// <summary>
        /// Handler for processing records that belong to this record.
        /// </summary>
        protected RecordHandler RootHandler
        {
            get;
            set;
        }

        /// <summary>
        /// Handler for processing records that can be a child of this record.
        /// </summary>
        protected RecordHandler ChildHandler
        {
            get;
            set;
        }

        /// <summary>
        /// Handler for processing extensions
        /// </summary>
        protected RecordHandler ExtensionHandler
        {
            get;
            set;
        }

        /// <summary>
        /// Handles records globally, not specific to a type of record.
        /// </summary>
        protected RecordHandler GlobalHandler
        {
            get;
            set;
        }

        #endregion Handlers

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctr
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public Record()
        {
            RootHandler = new RecordHandler();
            ChildHandler = new RecordHandler();
            ExtensionHandler = new RecordHandler();
            GlobalHandler = new RecordHandler();
            Children = new List<Record>();
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctr
        /// </summary>
        /// <param name="parent">The record creating this record.</param>
        /// <param name="header">The file header including palettes etc.</param>
        //////////////////////////////////////////////////////////////////
        public Record( Record parent, DataBase header ) :
            this()
        {
            Header = header;
            Parent = parent;
            Opcode = Header.Opcode;
            Length = Header.Length;
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Decode binary data.
        /// </summary>
        /// <param name="br"></param>
        //////////////////////////////////////////////////////////////////
        public virtual void Decode( BinaryReader br )
        {
            Opcode = ( Opcodes )br.ReadInt16();
            Length = br.ReadUInt16();
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Encode binary data writing to file
        /// </summary>
        /// <param name="bw"></param>
        //////////////////////////////////////////////////////////////////
        public virtual void Encode( BinaryWriter bw )
        {
            bw.Write( ( short )Opcode );
            bw.Write( Length );
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        public override string ToString()
        {
            return string.Format( "Opcode: {0}\n Length: {1}\n", Opcode, Length );
        }
    }
}