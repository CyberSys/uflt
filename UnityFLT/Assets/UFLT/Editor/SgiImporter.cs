using UFLT.Textures;
using UnityEditor.Experimental.AssetImporters;

namespace UFLT.Editor
{
    [ScriptedImporter(1, new[] { "rgb", "rgba", ".bw", "int", "inta", "sgi" }, 0)]
    public class SgiImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var importer = new TextureSGI(ctx.assetPath);

            #if UNITY_2017_3_OR_NEWER
            ctx.AddObjectToAsset(importer.Texture.name, importer.Texture);
            ctx.SetMainObject(importer.Texture);
            #else
            ctx.SetMainAsset(importer.Texture.name, importer.Texture);
            #endif
        }
    }
}