
using System;
using UnityEngine;
using UnityEditor;
using UFLT.Textures;
using System.IO;

namespace UFLT.Editor.Importer
{
    /// <summary>
    /// Importer for loading SGI textures(.rgb, .rgba, .int, .inta, .sgi, .bw)
    /// </summary>
	public class CustomImporterSGI : CustomImporter
    {
        #region Properties

        public enum OutputFormat
        {
            PNG,
            JPG
        }
        public OutputFormat textureFormat = OutputFormat.PNG;

        /// <summary>
        /// File extension/s supported by this importer.
        /// </summary>
        public new static string[] Extensions
        {
            get
            {
                return new string[] { ".rgb", ".rgba", ".int", ".inta", ".bw", ".sgi" };
            }
        }

        /// <summary>
        /// SGI files should be imported first, they have no dependency but may be a dependency for other files.
        /// </summary>
        public override int Priority
        {
            get
            {
                return 100;
            }
        }

        #endregion Properties

        public override void OnSourceFileImported()
        {            
            string sourceFilePath = AssetDatabase.GUIDToAssetPath( guid );
            string textureOutFilePath = sourceFilePath.Replace( Path.GetExtension( sourceFilePath ), textureFormat == OutputFormat.PNG ? "_Converted.png" : "_Converted.jpg" );            
            TextureSGI textureLoader = new TextureSGI( sourceFilePath );

            if( textureLoader.Valid )
            {
                byte[] texBytes = textureFormat == OutputFormat.PNG ? textureLoader.Texture.EncodeToPNG() : textureLoader.Texture.EncodeToJPG();
                File.WriteAllBytes( textureOutFilePath, texBytes );
                AssetDatabase.ImportAsset( textureOutFilePath );
            }
        }

        public override void OnSourceFileMoved()
        {
            throw new NotImplementedException();
        }

        public override void OnSourceFileDeleted()
        {
            throw new NotImplementedException();
        }
    }
}
