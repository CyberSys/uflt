using UnityEngine;
using System.Collections;
using UFLT.DataTypes.Enums;
using System.IO;
using System.Text;

namespace UFLT.Records
{
    /// <summary>
    /// The base class for all OpenFlight records that have a id/name field.
    /// </summary>
    public class InterRecord : Record
    {
        #region Properties

        /// <summary>
        /// The name/id of the record. Max 8 chars, if set value is longer it will be truncated during encode.
        /// </summary>
        public string Name
        {
            get;
            set;
        }   
        
        #endregion Properties

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctr
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public InterRecord()
        {
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctr
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="header"></param>
        //////////////////////////////////////////////////////////////////
        public InterRecord( Record parent, DataBase header ) :
            base( parent, header )
        {    
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
            Name = Encoding.ASCII.GetString( br.ReadBytes( 8 ) );
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
            bw.Write( Encoding.ASCII.GetBytes( FixedString.GenerateFixedString( Name, 8 ) ) );
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns a string representation.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        public override string ToString()
        {
            return string.Format( "{0}\nName: {1}\n", base.ToString(), Name );
        }
    }
}