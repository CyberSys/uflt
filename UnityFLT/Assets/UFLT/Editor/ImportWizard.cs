using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace UFLT.Editor
{
	public class ImportWizard : ScriptableWizard
	{
		#region Properties

		// Labels
		GUIContent fltFileLbl = new GUIContent("File(.flt)", "The root openflight database file.");
		GUIContent outputDirLbl = new GUIContent("Output Directory", "Where to save the converted file and its dependencies(Materials/Textures). Must be inside the Unity project");

		// The full file path to the flt file
		string openflightFile = EditorPrefs.GetString("uflt-importWiz-lastFile", "");
		string exportDirectory = "Assets/";

		// Our import settings.
		public ImportSettings settings = new ImportSettings();

		Vector2 logScroll;
		string log;

		#endregion

		[MenuItem("Assets/Import OpenFlight(.flt)")]
		static void CreateWizard()
		{
			ScriptableWizard.DisplayWizard<ImportWizard>("Import Openflight", "Import");
		}

		private void OnGUI()
		{
			// Title
			GUILayout.Label("Import & Convert Openflight Settings", EditorStyles.boldLabel);
			GUILayout.Space(20);

			// Settings
			EditorGUILayout.BeginVertical(GUI.skin.box);

			FileSelectionField();
			FileExportDirectoryField();

			GUILayout.Space(10);
			EditorGUILayout.EndVertical();

			if (!string.IsNullOrEmpty(log))
			{
				logScroll = EditorGUILayout.BeginScrollView(logScroll);
				GUILayout.Label(log);
				EditorGUILayout.EndScrollView();
			}
			else if (GUILayout.Button("Start Import"))
				OnWizardCreate();
		}

		private void FileSelectionField()
		{
			// Store the default GUI color so we can revert back.
			Color defaultGUICol = GUI.contentColor;

			EditorGUILayout.BeginHorizontal();
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.LabelField(fltFileLbl, GUILayout.Width(100));
			openflightFile = EditorGUILayout.TextField(openflightFile, GUILayout.ExpandWidth(true));

			// Select file dlg
			if (GUILayout.Button("...", EditorStyles.toolbarButton, GUILayout.Width(50)))
			{
				string selectedFile = EditorUtility.OpenFilePanel("Import OpenFlight", Application.dataPath, "flt");
				if (!string.IsNullOrEmpty(selectedFile))
				{
					openflightFile = selectedFile;
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		private void FileExportDirectoryField()
		{
			// Store the default GUI color so we can revert back.
			Color defaultGUICol = GUI.contentColor;

			EditorGUILayout.BeginHorizontal();
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.LabelField("Output Directory", GUILayout.Width(100));
			exportDirectory = EditorGUILayout.TextField(exportDirectory, GUILayout.ExpandWidth(true));

			// Select dir dlg
			if (GUILayout.Button("...", EditorStyles.toolbarButton, GUILayout.Width(50)))
			{
				string outDir = EditorUtility.SaveFolderPanel("Save Asset", Application.dataPath, "Converted OpenFlight");
				if (!string.IsNullOrEmpty(outDir))
				{
					exportDirectory = outDir;
					AssetDatabase.Refresh();
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		void OnWizardCreate()
		{
			UFLT.Records.Database db = new Records.Database(openflightFile);
			db.ParsePrepareAndImport();
			log = "Loading completed.\nDetails:\n" + db.Log.ToString();

			EditorPrefs.SetString("uflt-importWiz-lastFile", openflightFile);
        }


	}
}