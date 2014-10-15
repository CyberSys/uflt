using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;

namespace UFLT.Editor.Importer
{
    /// <summary>
    /// Base class for UFLT defined custom importers.
    /// We have to import things slightly different than Unity's built in importers.
    /// The original file is kept and we create a file for the converted data.
    /// This file is linked to the original file via its guid, this way we can be    
    /// notified when the file changes, moves or is deleted.
    /// </summary>
    public abstract class CustomImporter : ScriptableObject
    {
        #region Properties

        // AssetDatabase guid of the original file.        
        [HideInInspector]
        public string guid;

        /// <summary>
        /// File extension/s supported by this importer.        
        /// </summary>
        public static string[] Extensions
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Import priority. Files are imported in order of priority(highest to lowest).
        /// This allows us to ensure that textures are imported first when importing multiple files at once.
        /// </summary>
        public virtual int Priority
        {
            get
            {
                return 0;
            }
        }

        #endregion Properties

        /// <summary>
        /// Finds the new importer for the file or returns null.
        /// </summary>
        /// <param name="sourceGuid">guid of the file which needs an importer.</param>
        /// <returns></returns>
        public static CustomImporter FindImporter( string sourceGuid )
        {
            var foundObjectImporters = Resources.FindObjectsOfTypeAll( typeof( CustomImporter ) );
            foreach( var obj in foundObjectImporters )
            {
                CustomImporter ci = obj as CustomImporter;
                if( ci.guid == sourceGuid )
                    return ci;
            }

            return null;
        }

        /// <summary>
        /// Finds or creates a new importer for the file.
        /// </summary>
        /// <param name="sourceGuid">guid of the file which needs an importer.</param>
        /// <returns></returns>
        public static CustomImporter FindOrCreateImporter( string sourceGuid )
        {
            CustomImporter found = FindImporter( sourceGuid );
            if( found != null )
                return found;

            return CreateImporter( sourceGuid );
        }

        /// <summary>
        /// Creates a new importer 
        /// </summary>
        /// <param name="sourceGuid"></param>
        /// <returns></returns>
        static CustomImporter CreateImporter( string sourceGuid )
        {
            // Find an importer for this file type
            string sourceFilePath = AssetDatabase.GUIDToAssetPath( sourceGuid );
            string sourceFileExtension = Path.GetExtension( sourceFilePath );
            Type importerType = FindImporterType( sourceFileExtension );
            if( importerType == null ) return null;

            var objInstance = ScriptableObject.CreateInstance( importerType.Name );
            if( objInstance == null )
            {
                Debug.LogError( "Failed to create instance of type: " + importerType.Name );
                return null;
            }

            CustomImporter importerInstance = objInstance as CustomImporter;
            importerInstance.guid = sourceGuid;

            // Save to asset file.
            string assetFilePath = sourceFilePath.Replace( sourceFileExtension, "(Importer Settings).asset" );
            AssetDatabase.CreateAsset( importerInstance, assetFilePath );

            return importerInstance;            
        }

        /// <summary>
        /// Attempts to destroy the asset file for this importer, returns true if successful. 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool DeleteImporter()
        {
            string sourceFilePath = AssetDatabase.GUIDToAssetPath( guid );
            string importerFilePath = sourceFilePath.Replace( Path.GetExtension( sourceFilePath ), "(Importer Settings).asset" );
            return AssetDatabase.DeleteAsset( importerFilePath );
        }

        /// <summary>
        /// Returns a derived CustomImporter class that supports the file extension. 
        /// </summary>
        /// <param name="fileExt">File extension including the dot. E.G ".rgb", ".sgi".</param>
        /// <returns></returns>
        static Type FindImporterType( string fileExt )
        {
            // Find an implementation of CustomImporter that supports our file type.
            Type typeToFind = typeof( CustomImporter );
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();            
            foreach( var currentType in types )
            {                
                if( currentType.IsSubclassOf( typeToFind ) )
                {
                    var pi = currentType.GetProperty( "Extensions" );
                    if( pi == null )
                    {
                        Debug.LogWarning( currentType.ToString() + ": class does not contain a property called 'Extensions'" );
                        break;
                    }

                    string[] supportedExts = ( string[] )pi.GetValue( null, null );
                    if( supportedExts == null ) break;                    
                                        
                    foreach( var currentExt in supportedExts )
                    {
                        if( currentExt.Equals( fileExt, StringComparison.OrdinalIgnoreCase ) )                        
                            return currentType;                                                            
                    }                                     
                }
            }

            return null;         
        }

        /// <summary>
        /// Called when the source file is importer/re-imported or updated.
        /// </summary>
        public abstract void OnSourceFileImported();

        /// <summary>
        /// Called when the source file has been moved to an other section of the Assets directory.
        /// </summary>
        public virtual void OnSourceFileMoved()
        {            
            // Move the importer settings asset file.
            string currentPath = AssetDatabase.GetAssetPath( this );
            
            string sourceFilePath = AssetDatabase.GUIDToAssetPath( guid );
            string newPath = sourceFilePath.Replace( Path.GetExtension( sourceFilePath ), "(Importer Settings).asset" );
            AssetDatabase.MoveAsset( currentPath, newPath );            
        }
            
        /// <summary>
        /// Called when the source file is deleted, removes the importer settings asset file.
        /// </summary>
        public virtual void OnSourceFileDeleted()
        {
            DeleteImporter();
        }
    }
}