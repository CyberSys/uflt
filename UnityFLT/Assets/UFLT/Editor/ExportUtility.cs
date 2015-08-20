using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class ExportUtility
{
	public static string MakePathRelative(string abs)
	{
		return "Assets" + abs.Replace(Application.dataPath, "");
	}

	public static Texture SaveTextureToDisc(Texture t, string dir)
	{
		Texture2D tex2D = t as Texture2D;
		if (tex2D)
		{			
			string file = Path.Combine(dir, (string.IsNullOrEmpty(t.name) ? t.GetHashCode().ToString() : t.name)) + ".png";
			string outFileRelative = MakePathRelative(file);
			if (!File.Exists(file)) // Does the file already exist?
			{
				byte[] bytes = tex2D.EncodeToPNG();
				File.WriteAllBytes(file, bytes);
				AssetDatabase.ImportAsset(outFileRelative);
			}

			Object o = AssetDatabase.LoadAssetAtPath(outFileRelative, typeof(Texture));
			if (o != null)
			{
				//Object.DestroyImmediate(t); // Dont destroy it, if its a shared texture we lose connection in the other materials.
				return o as Texture;
			}
		}

		return t;
	}
}
