using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace UFLT.Editor
{
    /// <summary>
    /// Base class for UFLT defined custom importers.
    /// We have to import things slightly different than Unity's built in importers.
    /// The original file is kept and we create an other file for the converted data.
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

        #endregion Properties

        /// <summary>
        /// Finds or creates a new importer for the file.
        /// </summary>
        /// <param name="sourceGuid">guid of the file which needs an importer.</param>
        /// <returns></returns>
        //public static CustomImporter FindOrCreateImporter( string sourceGuid )
        //{
        //    var foundObjects = Resources.FindObjectsOfTypeAll( typeof( CustomImporter ) );            
        //    foreach( var obj in foundObjects )
        //    {
        //        CustomImporter ci = obj as CustomImporter;
        //        if( ci.guid == sourceGuid )
        //        {
        //            return ci;
        //        }
        //    }







        //    string filePath = AssetDatabase.GUIDToAssetPath( openFlightGuid );
        //    string extension = Path.GetExtension( filePath );
        //    if( extension.ToLower() != ".flt" )
        //    {
        //        Debug.LogError( "Not an OpenFlight file, must have .flt as an extension." );
        //        return null;
        //    }

        //    FLTImportSettings createdSettings = ScriptableObject.CreateInstance<FLTImportSettings>();
        //    createdSettings.guid = openFlightGuid;

        //    string filePathNoExt = filePath.Replace( extension, "" );
        //    string convertedFilePath = filePathNoExt + "(Converted).asset";
        //    AssetDatabase.CreateAsset( createdSettings, convertedFilePath );

        //    return createdSettings;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceGuid"></param>
        /// <returns></returns>
        static CustomEditor CreateImporter( string sourceGuid )
        {
            // Find an importer for this file type
            string filePath = AssetDatabase.GUIDToAssetPath( sourceGuid );
            string fileExtension = Path.GetExtension( filePath );
            Type importerType = FindImporter( fileExtension );
            if( importerType == null ) return null;

            var importerInstance = ScriptableObject.CreateInstance( importerType.Name );
            if( importerInstance == null ) return null;


            return null;
            // TODO: Save to file. YOU ARE HERE
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        static Type FindImporter( string fileExt )
        {
            // Find an implmentation of CustomImporter that supports our file type.
            Type typeToFind = typeof( CustomImporter );
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();            
            foreach( var currentType in types )
            {                
                if( currentType.IsSubclassOf( typeToFind ) )
                {
                    var pi = currentType.GetProperty( "Extensions" );
                    if( pi == null ) break;
                    
                    string[] supportedExts = ( string[] )pi.GetValue( null, null );
                    if( supportedExts == null ) break;
                                        
                    foreach( var currentExt in supportedExts )
                    {
                        if( currentExt == fileExt )                        
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
        public abstract void OnSourceFileMoved();

        /// <summary>
        /// Called when the source file is deleted.
        /// </summary>
        public abstract void OnSourceFileDeletes();
    }
}