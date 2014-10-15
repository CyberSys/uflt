
using System;
using UnityEngine;
using UnityEditor;
using UFLT.Textures;
using System.IO;

namespace UFLT.Editor.Importer
{
    /// <summary>
    /// Editor importer for loading SGI textures(.rgb, .rgba, .int, .inta, .sgi, .bw).    
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
        /// Text appened onto the original file name to create the converted file name. Includes file extension.
        /// </summary>
        public string ConvertedFileAppendName
        {
            get
            {
                return textureFormat == OutputFormat.PNG ? "_Converted.png" : "_Converted.jpg";
            }
        }

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
        /// SGI files should be imported first, they have no dependency themselves however may be a dependency for other files.
        /// </summary>
        public override int Priority
        {
            get
            {
                return 100;
            }
        }

        #endregion Properties

        /// <summary>
        /// Import the sgi texture.
        /// </summary>
        public override void OnSourceFileImported()
        {            
            string sourceFilePath = AssetDatabase.GUIDToAssetPath( guid );
            string textureOutFilePath = sourceFilePath.Replace( Path.GetExtension( sourceFilePath ), ConvertedFileAppendName );

            try
            {
                TextureSGI textureLoader = new TextureSGI( sourceFilePath );
                byte[] texBytes = textureFormat == OutputFormat.PNG ? textureLoader.Texture.EncodeToPNG() : textureLoader.Texture.EncodeToJPG();
                File.WriteAllBytes( textureOutFilePath, texBytes );
                AssetDatabase.ImportAsset( textureOutFilePath );
            }
            catch( Exception e )
            {
                Debug.LogException( e );
            }            
        }

        /// <summary>
        /// Move the converted texture.
        /// </summary>
        public override void OnSourceFileMoved()
        {
            string currentPath = AssetDatabase.GetAssetPath( this );
            string oldFile = currentPath.Replace( "(Importer Settings).asset", ConvertedFileAppendName );
            
            string sourceFilePath = AssetDatabase.GUIDToAssetPath( guid );
            string newPath = sourceFilePath.Replace( Path.GetExtension( sourceFilePath ), ConvertedFileAppendName );
            AssetDatabase.MoveAsset( oldFile, newPath );

            base.OnSourceFileMoved();
        }

        /// <summary>
        /// Removes the converted texture and the import settings file.
        /// </summary>
        public override void OnSourceFileDeleted()
        {            
            string sourceFilePath = AssetDatabase.GUIDToAssetPath( guid );
            string textureOutFilePath = sourceFilePath.Replace( Path.GetExtension( sourceFilePath ), ConvertedFileAppendName );
            AssetDatabase.DeleteAsset( textureOutFilePath );

            base.OnSourceFileDeleted();
        }
    }
}
