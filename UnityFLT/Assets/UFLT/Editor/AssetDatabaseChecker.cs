using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace UFLT.Editor
{
	/// <summary>
	/// Looks for AssetDatabase events that involve .flt files so we can import and convert them.
	/// </summary>
	public class AssetDatabaseChecker : AssetPostprocessor
	{
		static void OnPostprocessAllAssets( string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths )
	    {
			foreach (var str in importedAssets)
			{
				if( Path.GetExtension( str ).ToLower() == ".flt" ) 
				{
					FLTImportSettings importSettings = FLTImportSettings.CreateInstance<FLTImportSettings>();				

					var convertedFilePath = Path.ChangeExtension( str, ".asset" );
					AssetDatabase.CreateAsset( importSettings, convertedFilePath );
				}
			}


	        //foreach( var str in deletedAssets )
			//	Debug.Log("Deleted Asset: " + str);
			
			//for (int i=0;i<movedAssets.Length;i++)
			//	Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
		}
	}
}