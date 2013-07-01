using UnityEngine;
using System.Collections.Generic;
using UFLT.Records;
using UFLT.DataTypes.Enums;

namespace UFLT.Utils
{
    /// <summary>
    /// Class for storing materials that are created based on multiple records from the OpenFlight file/s.
    /// We cannot rely on the Material palette to provide our materials, it does not take into account the texture or lighting used.
    /// Advanced materials such as relfective ones require additional data from extended materials.
    /// All of these different records/fields can impact the type of material we need, this class brings it all together.
    /// Helps reduce the number of materials used by re-using materials.
    /// </summary>
    public class MaterialBank
    {
        #region Properties
		
        private static MaterialBank instance;
		
		/// <summary>
		/// Current materials.
		/// TODO: A faster lookup data structure, currently using a linear search.
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
			// MaterialPalette mp, TexturePalette texture, TexturePalette detail,  ushort Transparency, LightMode lm )
			IntermediateMaterial im = null;
	
			// Check materials first						
			MaterialPalette mp = f.MaterialIndex == -1 ? f.Header.MaterialPalettes[f.MaterialIndex] : null;						
			
			//List<IntermediateMaterial> searchList = Materials.FindAll( o => o.Palette != null ? o.Palette.Equals( mp ) : o.Palette  );
			
			//if( searchList.Count > 0 )
			//{
				// Next main texture
			//	TexturePalette
				
			//}
			
			
			
			
			//foreach( IntermediateMaterial current in Materials )
			//{
				// Check material
			//	MaterialPalette mp = 
			//	if( f.MaterialIndex = -1 && f.Header.MaterialPalettes[f.MaterialIndex
				
				
				
				/*if( current.Palette.Equals( mp ) && 
					current.MainTexture.Equals( texture ) &&
					current.DetailTexture.Equals( detail ) &&
					current.Transparency == Transparency &&
					current.LightMode == lm*/
			//}
					
			return null;
		}
		
    }
}