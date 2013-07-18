using UnityEngine;
using UnityEditor;
using UFLT.Records;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using System.Text;
using System.IO;

namespace UFLT.Editor
{
	public class AssetImporter
	{
		static StringBuilder sb = new StringBuilder();		
				
		static public void CollectDependanciesRecursive( UnityEngine.Object obj, ref Dictionary<int, Object> dependencies)
   		{
	        if (!dependencies.ContainsKey( obj.GetHashCode() ) )
	        {
	            dependencies.Add( obj.GetHashCode(), obj );      
	            SerializedObject objSO = new SerializedObject( obj );
	            SerializedProperty property = objSO.GetIterator();
	            do 
	            {
					sb.AppendLine( property.name );
	                if( ( property.propertyType == SerializedPropertyType.ObjectReference ) && 
	                    ( property.objectReferenceValue != null ) && 
	                    ( property.name != "m_PrefabParentObject" ) && //Don't follow prefabs
	                    ( property.name != "m_PrefabInternal" ) && //Don't follow prefab internals
	                    ( property.name != "m_Father" ) ) //Don't go back up the hierarchy in transforms
	                {
	                    CollectDependanciesRecursive( property.objectReferenceValue, ref dependencies );
	                }
	            } 
				while( property.Next( true ) );	
        	}
    	}			
		
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
			
			Dictionary<int, Object> depends = new Dictionary<int, Object>();
			CollectDependanciesRecursive( db.UnityGameObject, ref depends );
					
			AssetDatabase.CreateAsset( db.UnityGameObject, "Assets/TEST/db.asset"/*assetPath*/ );
			foreach( KeyValuePair<int, Object> kvp in depends )
			{
				if( kvp.Value != db.UnityGameObject )
				{
					AssetDatabase.AddObjectToAsset( kvp.Value, db.UnityGameObject );	
				}
			}					
						
			Object o = PrefabUtility.CreateEmptyPrefab( "Assets/TEST/dbpr.prefab" );			
			PrefabUtility.ReplacePrefab( db.UnityGameObject, o );
			AssetDatabase.SaveAssets();	
			
			TextWriter writer = File.CreateText("perl.txt");
			writer.Write( sb.ToString() );
			//Debug.Log( sb );
		}		
	}
}