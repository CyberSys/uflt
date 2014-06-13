﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace UFLT.Editor.Importer
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
        public static CustomImporter FindOrCreateImporter( string sourceGuid )
        {
            var foundObjects = Resources.FindObjectsOfTypeAll( typeof( CustomImporter ) );
            foreach( var obj in foundObjects )
            {
                CustomImporter ci = obj as CustomImporter;
                if( ci.guid == sourceGuid )
                {
                    return ci;
                }
            }

            return CreateImporter( sourceGuid );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceGuid"></param>
        /// <returns></returns>
        static CustomImporter CreateImporter( string sourceGuid )
        {
            // Find an importer for this file type
            string sourceFilePath = AssetDatabase.GUIDToAssetPath( sourceGuid );
            string sourceFileExtension = Path.GetExtension( sourceFilePath );
            Type importerType = FindImporter( sourceFileExtension );
            if( importerType == null ) return null;

            var objInstance = ScriptableObject.CreateInstance( importerType.Name );
            if( objInstance == null )
            {
                Debug.LogError( "Failed to create instance of type: " + importerType.Name );
                return null;
            }

            CustomImporter importerInstance = objInstance as CustomImporter;
            ( importerInstance as CustomImporter ).guid = sourceGuid;

            // Save to asset file.
            string assetFilePath = sourceFilePath.Replace( sourceFileExtension, "(Importer).asset" );
            AssetDatabase.CreateAsset( importerInstance, assetFilePath );

            return importerInstance;            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        static Type FindImporter( string fileExt )
        {
            // Find an implementation of CustomImporter that supports our file type.
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
        public abstract void OnSourceFileMoved();

        /// <summary>
        /// Called when the source file is deleted.
        /// </summary>
        public abstract void OnSourceFileDeleted();
    }
}