using UnityEngine;
using System.Collections;
using UFLT.DataTypes.Enums;
using System.IO;
using System.Text;

namespace UFLT.Records
{
    /// <summary>
    /// Header record
    /// </summary>
    public class Header : NamedRecord
    {
        #region Properties

  
        #endregion Properties
        
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