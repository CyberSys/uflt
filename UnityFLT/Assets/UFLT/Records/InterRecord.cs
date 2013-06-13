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
    }
}