using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace UFLT.Editor
{
	public class ImportWizard : ScriptableWizard
	{
		#region Properties
	
		// The full file path to the flt file
		public string openflightFile;
		private bool validFile = false;
		
		// The relative file path to where the converted file will be stored.
		public string exportDirectory = "Assets/";
		private bool validDir = true;
			
		// Our import settings.
		public ImportSettings settings = new ImportSettings();
		
		//
		// Editor only features
		//
		
		// Generate UV2.
		public bool generateLightmapUVs = false;
		public UnwrapParam lightmapParams = new UnwrapParam();
		
		private Texture2D fltIcon = AssetPreview.GetMiniTypeThumbnail( typeof( Light ) );
			    	
	    #endregion
		
	    [MenuItem( "Assets/Import OpenFlight(.flt) Advanced" )]
	    static void CreateWizard()
	    {
	        ScriptableWizard.DisplayWizard<ImportWizard>( "Import Openflight", "Import" );
	    }
	
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Custom GUI.
		/// </summary>
		//////////////////////////////////////////////////////////////////
	    private void OnGUI()
		{			
			// Title
			GUILayout.Label( "Import & Convert Openflight Settings", EditorStyles.boldLabel );			
            GUILayout.Space( 20 );
			
			// Settings
			EditorGUILayout.BeginVertical( GUI.skin.box );
			
			FileSelectionField();
			FileExportDirectoryField();
						
			GUILayout.Space( 10 );
            EditorGUILayout.EndVertical();
		}
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Field to display the selected flt file, validates if the file exists and if its ext is flt.
		/// </summary>
		//////////////////////////////////////////////////////////////////
		private void FileSelectionField()
		{
			// Store the default GUI color so we can revert back.
			Color defaultGUICol = GUI.contentColor;
						
			EditorGUILayout.BeginHorizontal();
			EditorGUI.BeginChangeCheck();			
			EditorGUILayout.LabelField( "File", GUILayout.Width( 100 ) );
			openflightFile = EditorGUILayout.TextField( openflightFile, GUILayout.ExpandWidth( true ) );									
			
			// Select file dlg
			if( GUILayout.Button( "...", EditorStyles.toolbarButton, GUILayout.Width( 50 ) ) )
			{
				string selectedFile = EditorUtility.OpenFilePanel( "Import OpenFlight", Application.dataPath, "flt" );	
				if( !string.IsNullOrEmpty( selectedFile ) )
				{
					openflightFile  = selectedFile;	
				}
			}			
			
			// Validate the file
			if( EditorGUI.EndChangeCheck() )
			{				
				validFile = File.Exists( openflightFile );		
				if( validFile )
				{
					if( !Path.GetExtension( openflightFile ).Equals( ".flt", System.StringComparison.OrdinalIgnoreCase ) )
					{
						validFile = false;
					}
				}
			}
			
			// Indicate if the file is good
			GUI.contentColor = validFile ? Color.green : Color.red;
			EditorGUILayout.LabelField( new GUIContent( fltIcon ), GUILayout.Width( 50 ) );			
			GUI.contentColor = defaultGUICol;
			EditorGUILayout.EndHorizontal();
		}
		
		private void FileExportDirectoryField()
		{
			// Store the default GUI color so we can revert back.
			Color defaultGUICol = GUI.contentColor;
						
			EditorGUILayout.BeginHorizontal();
			EditorGUI.BeginChangeCheck();			
			EditorGUILayout.LabelField( "Output Directory", GUILayout.Width( 100 ) );
			exportDirectory = EditorGUILayout.TextField( exportDirectory, GUILayout.ExpandWidth( true ) );									
			
			// Select dir dlg
			if( GUILayout.Button( "...", EditorStyles.toolbarButton, GUILayout.Width( 50 ) ) )
			{
				string outDir = EditorUtility.SaveFolderPanel( "Save Asset", Application.dataPath, "Converted OpenFlight" );
				if( !string.IsNullOrEmpty( outDir ) )
				{
					exportDirectory  = outDir;	
					AssetDatabase.Refresh();
				}
			}			
			
			// Validate the file
			if( EditorGUI.EndChangeCheck() )
			{				
				// Make sure the path is inside the unity project assets folder
				if( !exportDirectory.Contains( Application.dataPath ) )
				{
					validDir = false;					
				}			
				else
				{
					validDir = true;
				}
		
				if( validDir )
				{												
					// Make relative
					exportDirectory = "Assets" + exportDirectory.Replace( Application.dataPath, "" );						
					validDir = true;
				}
			}
			
			// Indicate if the file is good
			GUI.contentColor = validDir ? Color.green : Color.red;
			EditorGUILayout.LabelField( new GUIContent( fltIcon ), GUILayout.Width( 50 ) );			
			GUI.contentColor = defaultGUICol;
			EditorGUILayout.EndHorizontal();
		}		
		
		void OnWizardCreate()
	    {
	       
	    }  
		
		
	}
}