using UnityEngine;
using System.Collections.Generic;
using UFLT.DataTypes.Enums;
using System.IO;
using System.Text;

// TODO: extensions

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
        /// How deep this record is in the tree.
        /// </summary>
        public int Level
        {
            get;
            set;
        }
        
        /// <summary>
        /// The name/id of the record. 8 chars in length or a long id record will need to be included. Not all nodes have id's.
        /// </summary>
        public string ID
        {
            get;
            set;
        }

        /// <summary>
        /// Optional value included as a comment record.
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

        /// <summary>
        /// The current active handler for this record, initially this will be the RootHandler however when a
        /// push record is hit the active handler will become the ChildHandler and revert back when a pop occurs.
        /// </summary>
        protected RecordHandler ActiveHandler
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
            ActiveHandler = RootHandler;
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
        /// Parses the streams records.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public virtual void Parse()
        {
            while( Header.Stream.BeginRecord() )
            {
                Opcodes op = Header.Stream.Opcode;

                if( GlobalHandler.Handles( op ) ) // Check global handler
                {
                    if( !GlobalHandler.Handle( op ) )
                    {
                        return;
                    }
                }
                else if( ActiveHandler.Handles( op ) ) // Try the active handler
                {
                    if( !ActiveHandler.Handle( op ) )
                    {
                        return;
                    }
                }
                else
                {
                    if( ActiveHandler.ThrowBackUnhandled || ActiveHandler.ThrowBacks.Contains( op ) ) // Do we throw back the record to be handled by the parent?
                    {
                        // Mark the record to be repeated by our parent.
                        Header.Stream.Repeat = true;
                        return;
                    }
                    else
                    {
                        // Just ignore the record.
                        Debug.Log( "Ignored Record - " + op );
                    }
                }
            }
        }

        #region Record Handlers

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handle push records.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        protected bool HandlePush()
        {
            Header.Stream.Level++;
            ActiveHandler = ChildHandler; // Don't do child records that might overwrite parent info. eg. longid.
            return true;
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handle pop records.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        protected bool HandlePop()
        {
            Header.Stream.Level--;
            if( Header.Stream.Level == Level )
            {
                return false;
            }            
            return true;
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a long id record.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        protected bool HandleLongID()
        {
            ID = Encoding.ASCII.GetString( Header.Stream.Reader.ReadBytes( Header.Stream.Length - 4 ) ); // The id is the length of the record minus its header of 4 bytes.
            return true;
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses a long id record.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        protected bool HandleComment()
        {
            Comment = Encoding.ASCII.GetString( Header.Stream.Reader.ReadBytes( Header.Stream.Length - 4 ) ); // The comment is the length of the record minus its header of 4 bytes.
            return true;
        }

        #endregion Record Handlers
    }
}