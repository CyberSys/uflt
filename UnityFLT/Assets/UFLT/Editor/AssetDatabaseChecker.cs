using UnityEngine;
using UnityEditor;
using UFLT.Editor.Importer;
using System.Linq;
using System.Collections.Generic;

namespace UFLT.Editor
{
	/// <summary>
	/// Forwards AssetDatabase events to our custom importers.
	/// </summary>
	public class AssetDatabaseChecker : AssetPostprocessor
	{
        /// <summary>
        /// Collects custom importers and executes them in order of priority (highest to lowest). 
        /// </summary>
        /// <param name="importedAssets"></param>
        /// <param name="deletedAssets"></param>
        /// <param name="movedAssets"></param>
        /// <param name="movedFromAssetPaths"></param>
		static void OnPostprocessAllAssets( string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths )
	    {
            List<CustomImporter> importerTasks = GenerateCustomImportList( importedAssets );
            List<CustomImporter> deleteTasks = GenerateCustomImportList( deletedAssets );
            List<CustomImporter> moveTasks = GenerateCustomImportList( movedFromAssetPaths );
            
            importerTasks.ForEach( o => o.OnSourceFileImported() );
            
            foreach( var o in deleteTasks )
            {
                o.OnSourceFileDeleted();
                CustomImporter.DestroyImmediate( o );
            }

            moveTasks.ForEach( o => o.OnSourceFileMoved() );
		}
        
        /// <summary>
        /// Checks for a custom importer for each file and returns any found custom importers sorted by priority (highest to lowest).
        /// </summary>
        /// <param name="assets"></param>
        /// <returns></returns>
        static List<CustomImporter> GenerateCustomImportList( string[] assets )
        {
            List<CustomImporter> importers = new List<CustomImporter>();
            foreach( var file in assets )
            {
                string guid = AssetDatabase.AssetPathToGUID( file );
                CustomImporter ci = CustomImporter.FindOrCreateImporter( guid );
                if( ci != null )
                    importers.Add( ci );
            }
            return importers.OrderByDescending( o => o.Priority ).ToList();            
        }
	}
}