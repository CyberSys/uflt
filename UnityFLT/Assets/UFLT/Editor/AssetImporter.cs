using UnityEngine;
using UnityEditor;
using UFLT.Records;
using Object = UnityEngine.Object;

namespace UFLT.Editor
{
	public class AssetImporter
	{
		[MenuItem( "Assets/Import OpenFlight(.flt)" )]
		public static void AssetsImportOpenFlight()
		{
			// TODO: USe collect depends, then save each materials and texture out from the depends. Save it all into a folder with a structure like unity uses.e.g materials folder.
			
			
			
			
			
			
			string lastPath = EditorPrefs.GetString( "last_flt_dir", Application.dataPath );			
			string fltPath = EditorUtility.OpenFilePanel( "Import OpenFlight", lastPath, "flt" );
			
			Database db = null;
			
			if( fltPath.Length != 0 )
			{
				EditorPrefs.SetString( "last_flt_dir", fltPath );
				db = new Database( fltPath );
				db.ParsePrepareAndImport();
			}			
			
			string assetPath = EditorUtility.SaveFilePanelInProject( "Save Prefab", "OpenFlight File", "asset", "Where to save the prefab version of your OpenFlight file?" );
			if( assetPath.Length != 0 )
			{					
				Object[] depends = EditorUtility.CollectDeepHierarchy( new Object[]{ db.UnityGameObject } );
				
				AssetDatabase.CreateAsset( db.UnityGameObject, assetPath );
				//PrefabUtility.CreatePrefab( path, db.UnityGameObject );
				
				foreach( Object o in depends )
				{
					if( o != db.UnityGameObject )
					{
						Debug.Log( o );
						AssetDatabase.AddObjectToAsset( o, db.UnityGameObject );
					}
				}
			}
		}
	}
}