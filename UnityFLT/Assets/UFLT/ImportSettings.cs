using UnityEngine;
using System.Collections.Generic;
using UFLT.MonoBehaviours;

namespace UFLT
{
	/// <summary>
	/// Openflight Import settings.    
	/// </summary>
	[System.Serializable]
	public class ImportSettings 
	{
		#region Properties
		
		[SerializeField]
		protected List<string> additionalSearchDirectories;
		/// <summary>
		/// Additional directories to search for textures and external references.
		/// </summary>
		public List<string> AdditionalSearchDirectories
		{
			get
			{
				return additionalSearchDirectories;
			}
			set
			{
				additionalSearchDirectories = value;
			}
		}	
		
		[SerializeField]
		protected string degreeOfFreedomComponent;		
		/// <summary>
		/// This class will be added to DOF GameObjects, if the class inherits from 
		/// UFLT.MonoBehaviours.DegreeOfFreedom it will also have the DOF node 
		/// values populated. Leave empty to have no component added.
		/// </summary>		
		public string DegreeOfFreedomComponent
		{
			get
			{
				return degreeOfFreedomComponent;				
			}
			set
			{
				degreeOfFreedomComponent = value;	
			}
		}

        [SerializeField]
        protected string levelOfDetailComponent;
        /// <summary>
        /// This class will be added to LOD GameObjects, if the class inherits from 
        /// UFLT.MonoBehaviours.LevelOfDetails it will also have the LOD node 
        /// values populated. Leave empty to have no component added.
        /// </summary>		
        public string LevelOfDetailComponent
        {
            get
            {
                return levelOfDetailComponent;
            }
            set
            {
                levelOfDetailComponent = value;
            }
        }	
		
		[SerializeField]
		protected bool generateColliders = false;
		
		/// <summary>
		/// Adds mesh colliders to objects that contain a mesh.
		/// </summary>		
		public bool GenerateColliders
		{
			get
			{
				return generateColliders;
			}
			set
			{
				generateColliders = value;
			}
		}
				
		
		#endregion Properties
				
        //////////////////////////////////////////////////////////////////
		/// <summary>
		/// Ctr
		/// </summary>
        //////////////////////////////////////////////////////////////////
		public ImportSettings()
		{
			additionalSearchDirectories = new List<string>();		
	
            // Defaults
            degreeOfFreedomComponent = "DegreeOfFreedom";
            levelOfDetailComponent = "LevelOfDetail";
		}				
	}
}