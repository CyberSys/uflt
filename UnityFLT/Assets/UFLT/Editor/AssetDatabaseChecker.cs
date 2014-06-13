using UnityEngine;
using UnityEditor;
using UFLT.Editor.Importer;

namespace UFLT.Editor
{
	/// <summary>
	/// Forwards AssetDatabase events to our custom importers.
	/// </summary>
	public class AssetDatabaseChecker : AssetPostprocessor
	{
		static void OnPostprocessAllAssets( string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths )
	    {
			foreach( var file in importedAssets )
			{                
                string guid = AssetDatabase.AssetPathToGUID( file );
                CustomImporter ci = CustomImporter.FindOrCreateImporter( guid );
                if( ci != null )
                    ci.OnSourceFileImported();
			}

            foreach( var file in deletedAssets )
            {
                string guid = AssetDatabase.AssetPathToGUID( file );
                CustomImporter ci = CustomImporter.FindOrCreateImporter( guid );
                if( ci != null )
                    ci.OnSourceFileDeleted();
            }

            foreach( var file in movedAssets )
            {
                string guid = AssetDatabase.AssetPathToGUID( file );
                CustomImporter ci = CustomImporter.FindOrCreateImporter( guid );
                if( ci != null )
                    ci.OnSourceFileMoved();
            }
		}
	}
}