using UnityEngine;
using UnityEditor;
using UFLT.Records;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using System.Text;
using System.IO;
using UFLT.Utils;

namespace UFLT.Editor
{
	public class AssetImporter
	{				
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Collects the dependancies recursive.
		/// </summary>
		/// <param name='obj'>Object.</param>
		/// <param name='dependencies'>Dependencies</param>
		//////////////////////////////////////////////////////////////////
		static public void CollectDependanciesRecursive( UnityEngine.Object obj, ref Dictionary<int, Object> dependencies)
   		{
	        if (!dependencies.ContainsKey( obj.GetHashCode() ) )
	        {
	            dependencies.Add( obj.GetHashCode(), obj );      
	            SerializedObject objSO = new SerializedObject( obj );
	            SerializedProperty property = objSO.GetIterator();
	            do 
	            {			
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
		
		static Texture SaveToDisc( Texture t, string dir )
		{
			Texture2D tex2D = t as Texture2D;
			if( tex2D )
			{				
				string file = Path.Combine( dir, ( string.IsNullOrEmpty( t.name ) ? t.GetHashCode().ToString() : t.name ) ) + ".png";							
				string outFileRelative = MakePathRelative( file );		
				if( !File.Exists( file ) ) // Does the file already exist?
				{					
					byte[] bytes = tex2D.EncodeToPNG();				
					File.WriteAllBytes( file, bytes );
					AssetDatabase.ImportAsset( outFileRelative );
				}				
				
				Object o = AssetDatabase.LoadAssetAtPath( outFileRelative, typeof( Texture ) );
				if( o != null )
				{
					//Object.DestroyImmediate( t, true );
					return o as Texture;	
				}				
			}
			
			return t;		
		}
		
		static string MakePathRelative( string abs )
		{
			return "Assets" + abs.Replace( Application.dataPath, "" );			
		}
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Imports an OpenFlight file.
		/// </summary>
		//////////////////////////////////////////////////////////////////
		[MenuItem( "Assets/Import OpenFlight(.flt)" )]
		public static void ImportOpenFlight()
		{					
			// Select the flt file
			string fltPath = EditorUtility.OpenFilePanel( "Import OpenFlight", Application.dataPath, "flt" );
			string fltName = Path.GetFileNameWithoutExtension( fltPath );
			if( fltPath.Length == 0 )return;				
			
			// Load the file
			Database db = new Database( fltPath );
			db.ParsePrepareAndImport();
			
			// Select where to save the asset to.
			string outDir = EditorUtility.SaveFolderPanel( "Save Asset", Application.dataPath, "Converted OpenFlight" );
			if( outDir.Length == 0 )return;
			
			// Make sure the path is inside the unity project assets folder
			if( !outDir.Contains( Application.dataPath ) )
			{
				Debug.LogError( "Asset must be saved within the Unity project Assets directory" );
				return;
			}			
			
			// Make relative
			string outDirRelative = MakePathRelative( outDir );		
			string materialsDir = AssetDatabase.CreateFolder( outDirRelative, "Materials" ); // Create materials dir
			AssetDatabase.Refresh(); // Refresh for new directories that may have been created.
			
			// Collect depenancies.
			Dictionary<int, Object> depends = new Dictionary<int, Object>();
			CollectDependanciesRecursive( db.UnityGameObject, ref depends );			
			
			// Create our asset/s
			AssetDatabase.CreateAsset( db.UnityGameObject, Path.Combine( outDirRelative, fltName + ".asset" ) );
			foreach( KeyValuePair<int, Object> kvp in depends )
			{
				if( kvp.Value != db.UnityGameObject )
				{									
					if( kvp.Value is Shader )continue;
					if( kvp.Value is MonoScript )continue;	
					if( kvp.Value is Texture )continue;
					if( kvp.Value is Material )
					{									
						// TODO: Check if materials dir exists and it material already exists.
						Material m = kvp.Value as Material;
						Texture t = m.mainTexture; // The connection to the texture will be lost when we create the asset so we will need to re-assign it.
						string name = string.IsNullOrEmpty( m.name ) ? "material.mat" : m.name + ".mat";
						string fileName = AssetDatabase.GenerateUniqueAssetPath( Path.Combine( materialsDir, name ) );					
						AssetDatabase.CreateAsset( kvp.Value, fileName );			
						m.mainTexture = SaveToDisc( t, outDir );
						continue;
					}			
					
					// Add to the main asset			
					AssetDatabase.AddObjectToAsset( kvp.Value, db.UnityGameObject );	
				}
			}					
				
			// Create a prefab from the asset			
			Object o = PrefabUtility.CreatePrefab( Path.Combine( outDirRelative, fltName + ".prefab" ).Replace( "\\", "/" ), db.UnityGameObject );
			PrefabUtility.ReplacePrefab( db.UnityGameObject, o );
				
			// Remove from the scene.
			Object.DestroyImmediate( db.UnityGameObject, true );
			
			// Refresh
			AssetDatabase.SaveAssets();	
			
			// TODO: destroy old materials
		}		
	}
}