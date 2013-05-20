using UnityEngine;
using System.Collections;
using UFLT.DataTypes.Enums;
using System.IO;

namespace UFLT.Records
{
    /// <summary>
    /// The base class for all OpenFlight records.
    /// </summary>
    public class RecordBase
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
        
        #endregion Properties
        
        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Decode binary data.
        /// </summary>
        /// <param name="br"></param>
        //////////////////////////////////////////////////////////////////
        public void Decode( BinaryReader br )
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
        public void Encode( BinaryWriter bw )
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
            return string.Format( "Opcode: {0}\n Length: {1}", Opcode, Length );
        }
    }
}