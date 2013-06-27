using UnityEngine;
using System.Collections;

namespace UFLT
{
    /// <summary>
    /// Truncates/Pads a string so it is a fixed size
    /// </summary>
    public class FixedString
    {
        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a string of a fixed length.
        /// </summary>
        /// <param name="s">The string to fix.</param>
        /// <param name="fixedLength">Length of output string in chars.</param>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        public static string GenerateFixedString( string s, int fixedLength )
        {            
            if( s.Length > fixedLength )
            {
                // Truncate
                s = s.Substring( 0, fixedLength );
            }
            else if( s.Length != fixedLength )
            {
                // Pad
                s = s.PadRight( fixedLength );
            }

            return s;
        }
    }    
}