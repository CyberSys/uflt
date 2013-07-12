using UnityEngine;
using System.Collections;

namespace UFLT.MonoBehaviours
{
    [ExecuteInEditMode]
    public class LevelOfDetail : MonoBehaviour
    {
        #region Properties
        
        /// <summary>
        /// The distance to switch the model into view.
        /// </summary>
        public float switchInDistance;

        /// <summary>
        /// The distance to switch the model out of view.
        /// </summary>
        public float switchOutDistance;

        /// <summary>
        /// Use previous slant range.
        /// </summary>
        public bool usePreviousSlantRange;

        /// <summary>
        /// false for replacement LOD, true for additive LOD.
        /// </summary>
        public bool additive;
 
        /// <summary>
        /// Flags value
        /// </summary>
        public bool freezeCenter;

        /// <summary>
        /// Center of LOD.
        /// </summary>
        public Vector3 center;

        /// <summary>
        /// The range over which real-time smoothing effects should be employed 
        /// while switching from one LOD to another. Smoothing effects include 
        /// geometric morphing and image blending. The smoothing effect is active
        /// between: switch-in distance minus transition range (near), and 
        /// switch-in distance(far).
        /// </summary>
        public float transitionRange;
  
        /// <summary>
        /// Used to calculate switch in and out distances based on viewing 
        /// parameters of your simulation display system
        /// </summary>
        public float significantSize;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {

        }
    }
}