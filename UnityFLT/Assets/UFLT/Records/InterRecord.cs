using UnityEngine;
using System.Collections.Generic;
using UFLT.DataTypes.Enums;
using System.IO;
using System.Text;
using UFLT.Utils;


namespace UFLT.Records
{
    /// <summary>
    /// The base class for all OpenFlight records that have a id/name field.
    /// </summary>
    public class InterRecord : Record
    {
        #region Properties

        /// <summary>
        /// The Unity object for this record.
        /// </summary>
        public GameObject Object
        {
            get;
            set;
        }

        #region Mesh Params

        /// <summary>
        /// Position of vertices if this record contains a mesh
        /// </summary>
        public List<Vector3> VertexPositions
        {
            get;
            set;
        }
		
		// TODO: Uvs
		
		// TODO: Normals
		
		/// <summary>
		/// Materials paired with their triangles.
		/// </summary>		
		public KeyValuePair<IntermediateMaterial, List<int>> Triangles
		{
			get;
			set;
		}

        #endregion Mesh Params

        /// <summary>
        /// This objects matrix(tansform, rotation and scale)
        /// </summary>
        public Matrix4x4 Matrix
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
        public InterRecord()
        {            
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctr
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="header"></param>
        //////////////////////////////////////////////////////////////////
        public InterRecord( Record parent, Database header ) :
            base( parent, header )
        {    
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Converts the record/s into a Unity GameObject structure with meshes, 
        /// materials etc and imports into the scene.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public override void ImportIntoScene()
        {
			/*
            // Create an empty gameobject
            Object = new GameObject( ID );

            // Assign parent
            if( Parent != null && Parent is InterRecord )
            {
                Object.transform.parent = ( Parent as InterRecord ).Object.transform;
            }

            // Processes children
            base.ImportIntoScene();

            // Finalise mesh
            if( VertexPositions != null )
            {    
                Mesh m = new Mesh();                
                m.vertices = VertexPositions.ToArray();
                m.triangles = Triangles.ToArray();

                MeshRenderer mr = Object.AddComponent<MeshRenderer>();
                MeshFilter mf = Object.AddComponent<MeshFilter>();
                mf.mesh = m;

                // TODO: use Mesh.SetTriangles if more than 1 material.

            }
            */
        }

        #region Record Handlers

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handle matrix records.
        /// Reads a 4x4 matrix of floats, row major order.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        protected bool HandleMatrix()
        {
            Matrix4x4 m = new Matrix4x4();
            for( int i = 0; i < 4; i++ )
            {
                for( int j = 0; j < 4; ++j )
                {
                    m[i, j] = Header.Stream.Reader.ReadSingle();
                }
            }
            Matrix = m;
            return true;
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handles records that are not fully handled.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        protected bool HandleUnhandled()
        {
            Unhandled uh = new Unhandled( this );
            uh.Parse();
            return true;
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handles object records.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        protected bool HandleObject()
        {
            Object o = new Object( this );
            o.Parse();
            return true;
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handles Group records.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        protected bool HandleGroup()
        {
            Group g = new Group( this );
            g.Parse();
            return true;
        }

        #endregion Record Handlers
    }
}