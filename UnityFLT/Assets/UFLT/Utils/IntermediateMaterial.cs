using UnityEngine;
using System.Collections;
using UFLT.Records;
using UFLT.DataTypes.Enums;

namespace UFLT.Utils
{
    /// <summary>
    /// Represents a material created from various OpenFlight records/fields. 
    /// We want to defer creating the actual unity material as long as possible as this has to be done in the main thread.
    /// </summary>
    public class IntermediateMaterial
    {
        #region Properties
		
		/// <summary>
		/// Gets or sets the unity material.
		/// This could be null if the material has not been imported into the scene yet.
		/// </summary>		
		private Material unityMaterial;
		public Material UnityMaterial
		{
			get
			{
				if( unityMaterial == null )
				{
					unityMaterial = CreateUnityMaterial();
				}                
				return unityMaterial;
			}
		}

        /// <summary>
        /// The material palette if one exists, this contains the standard material 
        /// atributes such as diffuse, specular and alpha.
        /// Can be null.
        /// </summary>
        public MaterialPalette Palette
        {
            get;
            set;
        }

        /// <summary>
        /// Main texture, can be null.
        /// </summary>
        public TexturePalette MainTexture
        {
            get;
            set;
        }

        /// <summary>
        /// Detail texture, can be null.
        /// </summary>
        public TexturePalette DetailTexture
        {
            get;
            set;
        }

        /// <summary>
        /// Transparency
        /// 0 = Opaque
        /// 65535 = Totally clear
        /// </summary>
        public ushort Transparency
        {
            get;
            set;
        }

        /// <summary>
        /// Light mode.
        /// </summary>
        public LightMode LightMode
        {
            get;
            set;
        }

        #endregion Properties     
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Initializes a new instance of the <see cref="UFLT.Utils.IntermediateMaterial"/> class.
		/// </summary>
		/// <param name='mp'>Material palette or null.</param>
		/// <param name='main'>Main texture or null.</param>
		/// <param name='detail'>Detail texture or null.</param>
		/// <param name='transparancy'>Transparancy</param>
		/// <param name='lm'>Light mode.</param>
		//////////////////////////////////////////////////////////////////
		public IntermediateMaterial( MaterialPalette mp, TexturePalette main, TexturePalette detail, ushort transparancy, LightMode lm )
		{
			Palette = mp;
			MainTexture = main;
			DetailTexture = detail;
			Transparency = transparancy;
			LightMode = lm;			
		}
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates the unity material.
		/// </summary>		
		//////////////////////////////////////////////////////////////////
		protected virtual Material CreateUnityMaterial()
		{									
            Material mat = null;//new Material( Shader.Find( "Specular" ) );
			
			if( Palette != null )
			{
				// Final alpha = material alpha * (1.0- (geometry transparency / 65535))
				float finalAlpha = Palette.Alpha * ( 1f - ( Transparency / 65535f ) );
				
				if( Palette.Emissive.grayscale > 0 )
				{
					mat = new Material( Shader.Find( "VertexLit" ) );					
				}
				else if( finalAlpha < 1f )
				{										
					mat = new Material( Shader.Find( "Transparent/Specular" ) );										
					Palette.Diffuse = new Color( Palette.Diffuse.r, Palette.Diffuse.g, Palette.Diffuse.b, finalAlpha );
				}
				else
				{
					mat = new Material( Shader.Find( "Specular" ) );	
				}
				
				mat.color = Palette.Diffuse;					
				
				if( mat.HasProperty( "_Emissive" ) ) // Emissive color of a material (used in vertexlit shaders). 
				{					
					mat.SetColor( "_Emissive", Palette.Emissive );				
				}
				
				if( mat.HasProperty( "_SpecColor" ) )
				{					
					mat.SetColor( "_SpecColor", Palette.Specular );				
				}
				
				if( mat.HasProperty( "_Shininess" ) )
				{
					mat.SetFloat( "_Shininess", Palette.Shininess / 128f );
				}
			}
			
	
			
			// Specular
			
			// Diffuse
			
			// Unlit
			
			// Transparent
			
			
			
			
			
			
			
            
            if( mat == null )
			{
				// Default
				mat = new Material( Shader.Find( "Specular" ) );
			}
			
			if( MainTexture != null )
			{
				if( mat.HasProperty( "_MainTex" ) )
				{
					Texture2D t = MaterialBank.Instance.FindOrCreateTexture( MainTexture );
					if( t != null )
					{
						mat.SetTexture( "_MainTex", t );
					}
				}
			}			
					
			
            if( Palette != null )
            {
                mat.name = Palette.ID;
            }
            
			return mat;
		}

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Compares various material attrbiutes to see if this material matches.
        /// </summary>
        /// <param name="mp"></param>
        /// <param name="main"></param>
        /// <param name="detail"></param>
        /// <param name="trans"></param>
        /// <param name="lm"></param>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        public bool Equals( MaterialPalette mp, TexturePalette main, TexturePalette detail, ushort trans, LightMode lm )
        {
            if( !MaterialPalette.Equals( Palette, mp ) ) return false;
            if( !TexturePalette.Equals( MainTexture, main ) )return false;
            if( !TexturePalette.Equals( DetailTexture, detail ) )return false;
            if( Transparency != trans )return false;
            if( LightMode != lm )return false;
            return true;
        }
    }
}