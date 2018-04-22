using System.Collections.Generic;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UFLT.Editor
{
    [ScriptedImporter(1, ".flt", 10)]
    public class FltImporter : ScriptedImporter
    {
        // Labels
        GUIContent fltFileLbl = new GUIContent("File(.flt)", "The root openflight database file.");
        GUIContent outputDirLbl = new GUIContent("Output Directory", "Where to save the converted file and its dependencies(Materials/Textures). Must be inside the Unity project");

        // Our import settings.
        public ImportSettings settings = new ImportSettings();

        public override void OnImportAsset(AssetImportContext ctx)
        {
            UFLT.Records.Database db = new Records.Database(ctx.assetPath);
            db.Parse();
            db.PrepareForImport();
            db.ImportIntoScene();

            // Create our assets
            var meshFilters = db.UnityGameObject.GetComponentsInChildren<MeshFilter>();
            if (meshFilters.Length > 0)
            {
                Mesh mainMeshAsset = meshFilters[0].sharedMesh;
                for (int i = 1; i < meshFilters.Length; ++i)
                {
                    ctx.AddObjectToAsset(meshFilters[i].sharedMesh.name, meshFilters[i].sharedMesh);
                }
            }

            Dictionary<int, Material> savedMaterials = new Dictionary<int, Material>();
            var meshRenderers = db.UnityGameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var renderer in meshRenderers)
            {
                foreach (var mat in renderer.sharedMaterials)
                {
                    if (savedMaterials.ContainsKey(mat.GetInstanceID()))
                        continue;

                    savedMaterials[mat.GetInstanceID()] = mat;
                    ctx.AddObjectToAsset(mat.name, mat);
                }
            }

            ctx.AddObjectToAsset(db.UnityGameObject.name, db.UnityGameObject);
            ctx.SetMainObject(db.UnityGameObject);
        }
    }
}