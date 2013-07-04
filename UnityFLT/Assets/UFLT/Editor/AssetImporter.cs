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
		
			string fltPath = EditorUtility.OpenFilePanel( "Import OpenFlight", Application.dataPath, "flt" );
			
			Database db = null;
			
			if( fltPath.Length != 0 )
			{
				db = new Database( fltPath );
				db.ParsePrepareAndImport();
			}			
			
			string assetPath = EditorUtility.SaveFilePanelInProject( "Save Prefab", "OpenFlight File", "asset", "Where to save the prefab version of your OpenFlight file?" );
			if( assetPath.Length != 0 )
			{					
				Object[] depends = EditorUtility.CollectDeepHierarchy( new Object[]{ db.UnityGameObject } );
				
				AssetDatabase.CreateAsset( db.UnityGameObject, "Assets/test.asset"/*assetPath*/ );
				//PrefabUtility.CreatePrefab( path, db.UnityGameObject );
				
				foreach( Object o in depends )
				{
					if( o != db.UnityGameObject )
					{
						if( o is MeshRenderer )
						{							 
							foreach( Material m in ( o as MeshRenderer ).materials )
							{
								AssetDatabase.AddObjectToAsset( m, db.UnityGameObject );						
								
								if( m.mainTexture != null )
								{
									//AssetDatabase.AddObjectToAsset( m.mainTexture, db.UnityGameObject );							
								}
							}
						}
						
						if( o is MeshFilter )
						{
							AssetDatabase.AddObjectToAsset( ( o as MeshFilter ).mesh, db.UnityGameObject );							
						}
						
						AssetDatabase.AddObjectToAsset( o, db.UnityGameObject );
					}
				}
				
				PrefabUtility.CreatePrefab( "Assets/testp.prefab", db.UnityGameObject );			
			}
		}
	}
}