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
            ctx.AddObjectToAsset(importer.Texture.name, importer.Texture);
            ctx.SetMainObject(importer.Texture);
        }
    }
}