using System.Collections.Generic;
using UFLT.DataTypes.Enums;

namespace UFLT.Records
{
    /// <summary>
    /// The record handler keeps track of what records are handled by a record, if a record does not
    /// handle it then it can determine if the record should throw back for its parent to handle the record.
    /// This approach is taken from blight - http://sourceforge.net/projects/blight
    /// </summary>
    public class RecordHandler
    {
        #region Properties

        public delegate bool HandleRecordDelegate();

        /// <summary>
        /// Dictionary of opcodes to handler methods.
        /// </summary>
        public Dictionary<Opcodes, HandleRecordDelegate> Handler
        {
            get;
            set;
        }

        /// <summary>
        /// Send all opcodes not handled to the parent node?
        /// </summary>
        public bool ThrowBackUnhandled
        {
            get;
            set;
        }

        /// <summary>
        /// If throw back unhandled is false then throwback will only occur if the record is in this set.
        /// </summary>
        public HashSet<Opcodes> ThrowBacks
        {
            get;
            set;
        }

        #endregion Properties

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create a new record handler.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public RecordHandler()
        {
            Handler = new Dictionary<Opcodes, HandleRecordDelegate>();
            ThrowBacks = new HashSet<Opcodes>();            
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Attempts to handle an opcode. Returns true if successful.
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        public bool Handle( Opcodes opcode )
        {
            return Handler[opcode]();
        }
    }
}