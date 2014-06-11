using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace UFLT.Editor
{
	/// <summary>
	/// Looks for AssetDatabase events that involve .flt files so we can handle them.
	/// </summary>
	public class AssetDatabaseChecker : AssetPostprocessor
	{
		static void OnPostprocessAllAssets( string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths )
	    {
			foreach (var str in importedAssets)
			{
				if( Path.GetExtension( str ).ToLower() == ".flt" ) 
				{
                    string guid = AssetDatabase.AssetPathToGUID( str );

                    FLTImportSettings importSettings =  FLTImportSettings.FindOrCreate( guid );
                    importSettings.Import();
				}
			}

            // TODO: Check for operations on .asset files that contain flt settings, if deleeted then destroy any instances associated.

            
	        //foreach( var str in deletedAssets )
			//	Debug.Log("Deleted Asset: " + str);
			
			//for (int i=0;i<movedAssets.Length;i++)
			//	Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
		}
	}
}