using UnityEngine;
using System.Collections.Generic;
using UFLT.Records;

namespace UFLT.Utils
{
    /// <summary>
    /// Class for storing materials that are created based on multiple records from the OpenFlight file/s.
    /// We cannot rely on the Material palette to provide our materials, it does not take into account the texture or lighting used.
    /// Advanced materials such as relfective ones require additional data from extended materials.
    /// All of these different records/fields can impact the type of material we need, this class brings it all together.
    /// Helps reduce the number of materials used by re-using materials.
    /// </summary>
    public class MaterialBank
    {
        #region Properties
		
        private static MaterialBank instance;

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static MaterialBank Instance
        {
            get
            {
                if( instance == null )
                {
                    instance = new MaterialBank();
                }
                return instance;
            }
        }
        
        #endregion Properties

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Private constructor to enforce singleton.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        private MaterialBank()
        {
        }
		
    }
}