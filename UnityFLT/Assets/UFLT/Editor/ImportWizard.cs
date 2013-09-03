using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UFLT.Editor
{
	public class ImportWizard : ScriptableWizard
	{
		#region Properties
	
		public string openflightFile;
		
		public string exportDirectory;
		
		public bool generateLightmapUVs = false;
		
		public ModelImporterTangentSpaceMode normals = ModelImporterTangentSpaceMode.Import;		
	    	
	    #endregion
		
	    [MenuItem( "Assets/Import OpenFlight(.flt) Advanced" )]
	    static void CreateWizard()
	    {
	        ScriptableWizard.DisplayWizard<ImportWizard>( "Import Openflight", "Import" );
	    }
	
	    void OnWizardCreate()
	    {
	       
	    }  	
	}
}