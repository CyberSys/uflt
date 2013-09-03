using UnityEngine;
using System.Collections.Generic;
using UFLT.Records;
using UFLT.DataTypes.Enums;
using System.IO;
using UFLT.Textures;
using System.Collections;

namespace UFLT.Utils
{
    /// <summary>
    /// Class for storing materials & textures that are created based on multiple records from the OpenFlight file/s.
    /// We cannot rely on the Material palette to provide our materials, it does not take into account the texture or lighting used.
    /// Advanced materials such as reflective, cube maps etc require additional data from extended materials.
    /// All of these different records/fields can impact the type of material/shader we need, this class brings it all together.    
    /// Helps reduce the number of materials & textures used by re-using materials/textures over multiple databases.    
    /// </summary>
    public class MaterialBank
    {
        #region Properties
		
		/// <summary>
		/// Current materials.		
		/// </summary>		
		public List<IntermediateMaterial> Materials
		{
			get;
			set;
		}		

		/// <summary>
		/// Known textures, key is <b>absolute</b> file path.
		/// </summary>		
		private Dictionary<string, Texture2D> Textures
		{
			get;
			set;
		}
        
        #endregion Properties

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctr
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public MaterialBank()
        {
			Materials = new List<IntermediateMaterial>();
			Textures = new Dictionary<string, Texture2D>();
        }
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Finds the or create material. Thread safe.
		/// </summary>
		/// <returns>
		/// The found or created material, never returns null.
		/// </returns>
		/// <param name='f'>The face to find a material for.</param>
		//////////////////////////////////////////////////////////////////
		public IntermediateMaterial FindOrCreateMaterial( Face f )
		{				
			// TODO: A faster lookup data structure, currently using a linear search.
		
			// Fetch palettes
			MaterialPalette mp = f.MaterialIndex != -1 ? f.Header.MaterialPalettes[f.MaterialIndex] : null;									
			TexturePalette mainTex = f.TexturePattern != -1 ? f.Header.TexturePalettes[f.TexturePattern] : null;
			TexturePalette detailTex = f.DetailTexturePattern != -1 ? f.Header.TexturePalettes[f.DetailTexturePattern] : null;
			lock( this )
			{
				foreach( IntermediateMaterial current in Materials )
				{
	                if( current.Equals( mp, mainTex, detailTex, f.Transparency, f.LightMode ) )
					{
						// We found a matching material
						return current;
					}				
				}            
	
				// Create a new material
				IntermediateMaterial im = new IntermediateMaterial( this, mp, mainTex, detailTex, f.Transparency, f.LightMode );
				Materials.Add( im );
				return im;				
			}			
		}
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Loads all material textures using a coroutine.
		/// </summary>
		//////////////////////////////////////////////////////////////////
		public IEnumerator LoadTextures()
		{
			foreach( IntermediateMaterial im in Materials )
			{
				string path = FileFinder.Instance.Find( im.MainTexture.FileName );			
				if( path != string.Empty )
				{
					// Have we already loaded this texture?					
					if( Textures.ContainsKey( path ) )
					{
						// Dont need to load it.
						break;
					}
					
					string ext = Path.GetExtension( path );
					if( ext == ".rgb" ||
						ext == ".rgba" ||
	                    ext == ".bw" ||
						ext == ".int" || 
						ext == ".inta" || 
						ext == ".sgi" )
					{					
						TextureSGI sgi = new TextureSGI( path );
						Texture2D tex = sgi.Texture;
						if( tex != null )
						{
							Textures[path] = tex;	
							tex.name = Path.GetFileNameWithoutExtension( path );
							return tex;
						}
					}
					else
					{							
						WWW www = new WWW( "file://" + path );				
						yield return www;
		
						if( www.error == null && www.texture != null )
						{
							Textures[path] = www.texture;															
							www.texture.name = Path.GetFileNameWithoutExtension( path );
							return www.texture;
						}							
						else
						{
							Debug.LogError( www.error );	
						}
					}
				}			
			}
		}
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Finds the texture if it has already been loaded else loads
		/// the new texture and records it for future re-use.
		/// </summary> Texture will be loaded in the same thread.
		/// <returns>
		/// Found texture or null if it can not be found/loaded.
		/// </returns>
		/// <param name='tp'>Tp.</param>
		//////////////////////////////////////////////////////////////////
		public Texture2D FindOrCreateTexture( TexturePalette tp )
		{
			// Find the texture 
			// TODO: Make the path absolute?	
			string path = FileFinder.Instance.Find( tp.FileName );			
			if( path != string.Empty )
			{
				// Have we already loaded this texture?
				Texture2D tex = null;
				// TODO: Maybe hash the paths for faster lookup?
				if( Textures.TryGetValue( path, out tex ) )
				{
					// We found it!
					return tex;
				}
				
				string ext = Path.GetExtension( path );
				if( ext == ".rgb" ||
					ext == ".rgba" ||
                    ext == ".bw" ||
					ext == ".int" || 
					ext == ".inta" || 
					ext == ".sgi" )
				{					
					TextureSGI sgi = new TextureSGI( path );
					tex = sgi.Texture;
					if( tex != null )
					{
						Textures[path] = tex;
						tex.name = Path.GetFileNameWithoutExtension( path );
						return tex;
					}
				}
				else
				{							
					WWW www = new WWW( "file://" + path );				
					while( !www.isDone )
					{							
					}
	
					if( www.error == null && www.texture != null )
					{
						Textures[path] = www.texture;	
						www.texture.name = Path.GetFileNameWithoutExtension( path );
						return www.texture;
					}							
					else
					{
						Debug.LogError( www.error );	
					}
				}
			}			
			return null;
		}
    }
}