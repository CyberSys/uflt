using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UFLT.Records;

namespace UFLT.Editor
{
	public class FLTImportSettings : ScriptableObject
	{
        // AssetDatabase guid of the OpenFlight file.
        //[HideInInspector]
        public string guid;

		[Header( "OpenFlight Import Settings" )]
		[Space( 30 )]        
		public string test = "TEST STRING";

        /// <summary>
        /// Finds or creates a new settings file for an OpenFlight file. 
        /// </summary>
        /// <param name="openFlightGuid"></param>
        /// <returns></returns>
        public static FLTImportSettings FindOrCreate( string openFlightGuid )
        {            
            // TODO: Check a file exists for these settings, if not save them.

            var foundObjects = Resources.FindObjectsOfTypeAll<FLTImportSettings>();
            foreach( var obj in foundObjects )
            {
                if( obj.guid == openFlightGuid )
                {
                    return obj;
                }
            }            

            // Create a new settings file
            string filePath = AssetDatabase.GUIDToAssetPath( openFlightGuid );
            string extension = Path.GetExtension( filePath );
            if( extension.ToLower() != ".flt" )
            {
                Debug.LogError( "Not an OpenFlight file, must have .flt as an extension." );
                return null;
            }            

            FLTImportSettings createdSettings = ScriptableObject.CreateInstance<FLTImportSettings>();
            createdSettings.guid = openFlightGuid;

            string filePathNoExt = filePath.Replace( extension, "" );
            string convertedFilePath = filePathNoExt + "(Converted).asset";
            AssetDatabase.CreateAsset( createdSettings, convertedFilePath );

            return createdSettings;
        }

        /// <summary>
        /// Imports the flt file and generates a Unity prefab from the data.
        /// </summary>
        public void Import()
        {
            string filePath = AssetDatabase.GUIDToAssetPath( guid );            
            Database openFlightDB = new Database( filePath );
            openFlightDB.ParsePrepareAndImport();           
        }
	}
}
