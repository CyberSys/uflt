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
			// TODO: create material
			
            Material m = new Material( Shader.Find( "Specular" ) );
            m.SetColor( "_Color", Color.red );            

            if( Palette != null )
            {
                m.name = Palette.ID;
            }
            
			return m;
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