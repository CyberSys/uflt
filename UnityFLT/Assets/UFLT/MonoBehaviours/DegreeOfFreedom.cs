using UnityEngine;
using System.Collections;

namespace UFLT.MonoBehaviours
{
	/// <summary>
	/// Info extracted from a Degree of freedom(DOF).
	/// </summary>
	public class DegreeOfFreedom : MonoBehaviour 
	{
		#region Properties
			
        /// <summary>
        /// Origin of DOF's local coordinate system.(x, y, z).
        /// </summary>
        public Vector3 origin;        

        /// <summary>
        /// Point on x axis of DOF's local coordinate system (x, y, z).
        /// </summary>
        public Vector3 pointOnXAxis;        

        /// <summary>
        /// Point in xy plane of DOF's local coordinate system (x, y, z)
        /// </summary>
        public Vector3 pointInXYPlane;
        
        /// <summary>
        /// Min, Max, Current & Increment of x with respect to local coordinate system.
        /// </summary>
        public Vector4 minMaxCurrentIncrementX;        

        /// <summary>
        /// Min, Max, Current & Increment of y with respect to local coordinate system.
        /// </summary>
        public Vector4 minMaxCurrentIncrementY;
        
        /// <summary>
        /// Min, Max, Current & Increment of z with respect to local coordinate system.
        /// </summary>
        public Vector4 minMaxCurrentIncrementZ;

        /// <summary>
        /// Min, Max, Current & Increment of pitch 
        /// </summary>
        public Vector4 minMaxCurrentIncrementPitch;        
		
        /// <summary>
        /// Min, Max, Current & Increment of roll 
        /// </summary>
        public Vector4 minMaxCurrentIncrementRoll;        
		
        /// <summary>
        /// Min, Max, Current & Increment of yaw 
        /// </summary>
        public Vector4 minMaxCurrentIncrementYaw;

		/// <summary>
        /// Min, Max, Current & Increment of scale z
        /// </summary>
        public Vector4 minMaxCurrentIncrementScaleZ;
		
		/// <summary>
        /// Min, Max, Current & Increment of scale y.
        /// </summary>
        public Vector4 minMaxCurrentIncrementScaleY;
		
		/// <summary>
        /// Min, Max, Current & Increment of scale x.
        /// </summary>
        public Vector4 minMaxCurrentIncrementScaleX;
			
		/// <summary>
		/// If true then the X translation should be limited using the relevant Min,Max values.
		/// </summary>		
        public bool xTranslationLimited;  
		
		/// <summary>
		/// If true then the Y translation should be limited using the relevant Min,Max values.
		/// </summary>				
        public bool yTranslationLimited;
		
		/// <summary>
		/// If true then the Z translation should be limited using the relevant Min,Max values.
		/// </summary>				
        public bool zTranslationLimited;
		
		/// <summary>
		/// If true then the pitch should be limited using the relevant Min,Max values.
		/// </summary>				
        public bool pitchLimited;
		
		/// <summary>
		/// If true then the roll should be limited using the relevant Min,Max values.
		/// </summary>					
        public bool rollLimited;
        
		/// <summary>
		/// If true then the yaw should be limited using the relevant Min,Max values.
		/// </summary>			
        public bool yawLimited;
		
		/// <summary>
		/// If true then the X scale should be limited using the relevant Min,Max values.
		/// </summary>					
        public bool scaleXLimited;
        
		/// <summary>
		/// If true then the Y scale should be limited using the relevant Min,Max values.
		/// </summary>				
		public bool scaleYLimited;
        
		/// <summary>
		/// If true then the Z scale should be limited using the relevant Min,Max values.
		/// </summary>						
        public bool scaleZLimited;
        
		#endregion Properties	
	}
}