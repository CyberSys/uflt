using UnityEngine;
using UnityEditor;
using System.Collections;
using UFLT.Textures;
using System.IO;

namespace UFLT.Editor.Importer
{
    [CustomEditor( typeof( CustomImporterSGI ) )]
    public class CustomImporterSGIEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Cached Importer for this texture.
        /// Provides data on the texture such as header values.
        /// </summary>
        TextureSGI TextureLoader
        {
            get
            {
                if( _textureLoader == null )
                {
                    string sourceFilePath = AssetDatabase.GUIDToAssetPath( Importer.guid );
                    _textureLoader = new TextureSGI( sourceFilePath );
                }
                return _textureLoader;
            }
        }
        TextureSGI _textureLoader;

        /// <summary>
        /// Cached importer.
        /// </summary>
        CustomImporterSGI Importer
        {
            get
            {
                if( _importer == null )
                    _importer = target as CustomImporterSGI;
                return _importer;
            }
        }
        CustomImporterSGI _importer;

        /// <summary>
        /// Custome inspector view. Shows the contents of the header as well as forcing a re-import if any values are changed.
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Header
            EditorGUILayout.BeginVertical( GUI.skin.box );
            EditorGUILayout.LabelField( "SGI File Header", EditorStyles.boldLabel );
            EditorGUILayout.LabelField( "Run Length Encoding:", TextureLoader.RLE.ToString() );
            EditorGUILayout.LabelField( "Bytes Per Field:", TextureLoader.BPC.ToString() );
            EditorGUILayout.LabelField( "Dimensions:", TextureLoader.Dimension.ToString() );
            EditorGUILayout.LabelField( "Size:", TextureLoader.Size[0].ToString() + "," + TextureLoader.Size[1].ToString() );

            switch( TextureLoader.Size[2] )
            {
                case 1: EditorGUILayout.LabelField( "Channels:", "Greyscale" ); break;
                case 3: EditorGUILayout.LabelField( "Channels:", "RGB" ); break;
                case 4: EditorGUILayout.LabelField( "Channels:", "RGBA" ); break;
                default: EditorGUILayout.LabelField( "Channels:", "Unknown(" + TextureLoader.Size[2].ToString() + ")" ); break;
            }

            EditorGUILayout.LabelField( "Pixel Min/Max:", TextureLoader.PixMinMax[0].ToString() + "/" + TextureLoader.PixMinMax[1].ToString() );
            EditorGUILayout.LabelField( "Name:", TextureLoader.Name );

            switch( TextureLoader.ColorMapID )
            {
                case 0: EditorGUILayout.LabelField( "Colormap ID:", "Normal" ); break;
                case 1: EditorGUILayout.LabelField( "Colormap ID:", "Dithered" ); break;
                case 2: EditorGUILayout.LabelField( "Colormap ID:", "Index colour" ); break;
                case 3: EditorGUILayout.LabelField( "Colormap ID:", "Colormap" ); break;
                default: EditorGUILayout.LabelField( "Colormap ID:", "Unknown(" + TextureLoader.ColorMapID.ToString() + ")" ); break;
            }

            EditorGUILayout.EndVertical();

            EditorGUI.BeginChangeCheck();
            Importer.textureFormat = ( CustomImporterSGI.OutputFormat )EditorGUILayout.EnumPopup( "Converted Texture Format", Importer.textureFormat );
            if( EditorGUI.EndChangeCheck() )
            {
                // Delete the old file
                string sourceFilePath = AssetDatabase.GUIDToAssetPath( Importer.guid );
                string textureOutFilePath = sourceFilePath.Replace( Path.GetExtension( sourceFilePath ), Importer.textureFormat != CustomImporterSGI.OutputFormat.PNG ? "_Converted.png" : "_Converted.jpg" );
                AssetDatabase.DeleteAsset( textureOutFilePath );          
     
                Importer.OnSourceFileImported();
            }
        }
    }
}