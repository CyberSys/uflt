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
			
			//string assetPath = EditorUtility.SaveFilePanelInProject( "Save Prefab", "OpenFlight File", "asset", "Where to save the prefab version of your OpenFlight file?" );
			//if( assetPath.Length != 0 )
			{		
					Object[] depends = EditorUtility.CollectDependencies( new Object[]{ db.UnityGameObject } );
				foreach( Object o in depends )
				{
					if( !o.Equals( db.UnityGameObject ) )
					{
						if( o is MeshRenderer )
						{	
							foreach( Material m in ( o as MeshRenderer ).sharedMaterials )
							{
								//AssetDatabase.AddObjectToAsset( m, db.UnityGameObject );						
								if( AssetDatabase.GetAssetPath( m ) == null )
								{
									AssetDatabase.CreateAsset( m, "Assets/TEST/" + m.name + ".mat" );
								}
								if( m.mainTexture != null )
								{
								}
							}
						}
					}
				}
				
				
							
				AssetDatabase.CreateAsset( db.UnityGameObject, "Assets/TEST/test.asset"/*assetPath*/ );
				
				foreach( Object o in depends )
				{
					if( !o.Equals( db.UnityGameObject ) )
					{
						if( o is GameObject )continue;
						if( o is MeshRenderer )
						{	
					
						}
						
						if( o is MeshFilter )
						{
							AssetDatabase.AddObjectToAsset( ( o as MeshFilter ).sharedMesh, db.UnityGameObject );							
						}						
						
						if( o is Transform )
						{
							continue;	
						}
						
						Debug.Log( o );
						//AssetDatabase.AddObjectToAsset( o, db.UnityGameObject );	
					}
				}
				
				AssetDatabase.SaveAssets();
				//var ob = PrefabUtility.CreateEmptyPrefab( "Assets/empty.prefab" );
				//PrefabUtility.ReplacePrefab( db.UnityGameObject, ob, ReplacePrefabOptions.ReplaceNameBased );
				
				//AssetDatabase.SaveAssets();
				
				
				//PrefabUtility.CreatePrefab( path, db.UnityGameObject );
				/*
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
				*/		
			}							
		}

		
	}
}