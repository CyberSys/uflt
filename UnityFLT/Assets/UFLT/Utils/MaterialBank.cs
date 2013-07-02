using UnityEngine;
using System.Collections.Generic;
using UFLT.Records;
using UFLT.DataTypes.Enums;

namespace UFLT.Utils
{
    /// <summary>
    /// Class for storing materials that are created based on multiple records from the OpenFlight file/s.
    /// We cannot rely on the Material palette to provide our materials, it does not take into account the texture or lighting used.
    /// Advanced materials such as reflective, cube maps etc require additional data from extended materials.
    /// All of these different records/fields can impact the type of material/shader we need, this class brings it all together.
    /// Helps reduce the number of materials used by re-using materials.
    /// </summary>
    public class MaterialBank
    {
        #region Properties
		
        private static MaterialBank instance;
		
		/// <summary>
		/// Current materials.		
		/// </summary>		
		public List<IntermediateMaterial> Materials
		{
			get;
			set;
		}

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static MaterialBank Instance
        {
            get
            {
                if( instance == null )
                {
                    instance = new MaterialBank();
                }
				return instance;
            }
        }
        
        #endregion Properties

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Private constructor to enforce singleton.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        private MaterialBank()
        {
			Materials = new List<IntermediateMaterial>();
        }
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Finds the or create material.
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
								
			foreach( IntermediateMaterial current in Materials )
			{
                if( current.Equals( mp, mainTex, detailTex, f.Transparency, f.LightMode ) )
				{
					// We found a matching material
					return current;
				}				
			}
			
			// Create a new material
			IntermediateMaterial im = new IntermediateMaterial( mp, mainTex, detailTex, f.Transparency, f.LightMode );
			Materials.Add( im );
			return im;				
		}
    }
}