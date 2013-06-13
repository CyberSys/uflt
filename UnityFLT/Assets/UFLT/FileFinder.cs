using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UFLT
{
/// <summary>
/// Stores all file paths used in an openflight in order to help locate files that are not where they are supposed to be, e.g textures.
/// Some file references are absolute so as soon as the file is moved they are invalid, this helps find those files.
/// </summary>
    public class FileFinder 
    {
        #region Properties

        #region Private

        /// <summary>
        /// All known file paths to be checked.
        /// </summary>
        private static List<string> Paths
        {
            get;
            set;
        }

        private static FileFinder instance;

        #endregion

        /// <summary>
        /// Singleton instance
        /// </summary>        
        public static FileFinder Instance
        {
            get
            {
                if( instance == null )
                {
                    instance = new FileFinder();
                }

                return instance;
            }
        }

        #endregion Properties

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctor
        /// </summary>
        //////////////////////////////////////////////////////////////////
        private FileFinder()
        {
            Paths = new List<string>();            
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds a path to the file finder, this path will now be checked when searching for missing files.
        /// </summary>
        /// <param name="fileName"></param>
        //////////////////////////////////////////////////////////////////
        public void AddPath( string fileName )
        {
            string dir = Path.GetDirectoryName( fileName );
            if( !Paths.Contains( dir ) ) // Dont add duplicates
            {
                Paths.Add( dir );
            }            
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Checks if the file exists, if not searches for a file with the same name using all known paths.
        /// Returns the path if found else returns an empty string.
        /// </summary>
        /// <param name="full_path"></param>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        public string Find( string fullPath )
        {        
            string file = Path.GetFileName( fullPath );
            
            // Is the path absolute?
            if( Path.IsPathRooted( fullPath ) )
            {               
                if( File.Exists( fullPath ) )
                {
                    AddPath( fullPath );
                    return fullPath;
                }                
            }
            else
            {
                // Try the relative path against our list of paths.
                foreach( string currentPath in Paths )
                {
                    string combPath = Path.Combine( currentPath, fullPath );
                    if( File.Exists( combPath ) )
                    {
                        AddPath( combPath );
                        return combPath;
                    }
                }
            }

            // Search previous directories that have worked.
            foreach( string currentPath in Paths )
            {
                string combPath = Path.Combine( currentPath, file );
                if( File.Exists( combPath ) )
                {
                    AddPath( combPath );
                    return combPath;
                }
            }

            Debug.LogWarning( "Could not find file: " + fullPath );

            return string.Empty;
        }
    }
}