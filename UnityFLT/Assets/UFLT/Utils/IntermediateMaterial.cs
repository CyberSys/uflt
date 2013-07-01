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
    }
}