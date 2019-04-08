using Unity.Images.SGI;
using UnityEditor.Experimental.AssetImporters;

namespace UnityEditor.Images.SGI
{
    [ScriptedImporter(1, new string[] { "sgi", "rgb", "rgba", "bw", "int", "inta" })]
    public class TextureSgiImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var image = new TextureSgi(ctx.assetPath);
            ctx.AddObjectToAsset("Texture", image.Texture);
            ctx.SetMainObject(image.Texture);
        }
    }
}
