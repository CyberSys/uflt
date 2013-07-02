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
        /// The vertices used in our mesh.
        /// </summary>
        public List<VertexWithColor> Vertices
        {
            get;
            set;
        }

        /// <summary>
        /// Position of vertices if this record contains a mesh
        /// </summary>
        public List<Vector3> VertexPositions
        {
            get;
            set;
        }

        /// <summary>
        /// Mesh vertex normals
        /// </summary>
        public List<Vector3> Normals
        {
            get;
            set;
        }

        /// <summary>
        /// Mesh vertex Uvs
        /// </summary>
        public List<Vector2> UVS
        {
            get;
            set;
        }
		
		/// <summary>
		/// Materials paired with their triangles.
		/// </summary>		
		public List<KeyValuePair<IntermediateMaterial, List<int>>> SubMeshes
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
        /// Sets up record for importing into scene, if its a mesh creates the mesh structures.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public override void PrepareForImport()
        {
            // Do we have any faces?
            if( Children.Find( o => o is Face ) != null )
            {
                Vertices = new List<VertexWithColor>();
                SubMeshes = new List<KeyValuePair<IntermediateMaterial, List<int>>>();                

                base.PrepareForImport();

                // Do we have any verts, we may have just processed hidden faces.
                if( Vertices.Count > 0 )
                {                                
                    // TODO: Remove doubles. Check for duplicate verts and merge if possible. dont forget to change triangle indexes.

                    // Now setup for mesh                
                    VertexPositions = new List<Vector3>( Vertices.Count );
                    Normals = new List<Vector3>( Vertices.Count );
                    UVS = new List<Vector2>( Vertices.Count );

                    foreach( VertexWithColor vwc in Vertices )
                    {
                        VertexPositions.Add( new Vector3( ( float )vwc.Coordinate[0], ( float )vwc.Coordinate[1], ( float )vwc.Coordinate[2] ) );

                        // Normals                        
                        if( vwc is VertexWithColorNormal )
                        {
                            Normals.Add( ( vwc as VertexWithColorNormal ).Normal );
                        }
                        else
                        {                            
                            Normals.Add( Vector3.zero );
                        }

                        // Uvs
                        if( vwc is VertexWithColorNormalUV )
                        {
                            UVS.Add( ( vwc as VertexWithColorNormalUV ).UV );
                        }
                        else if( vwc is VertexWithColorUV )
                        {
                            UVS.Add( ( vwc as VertexWithColorUV ).UV );
                        }
                        else
                        {
                            UVS.Add( Vector2.zero );
                        }
                    }
                }
            }
            else
            {                
                base.PrepareForImport();
            }
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Converts the record/s into a Unity GameObject structure with meshes, 
        /// materials etc and imports into the scene.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public override void ImportIntoScene()
        {                        			
            // Create an empty gameobject
            Object = new GameObject( ID );

            // Assign parent
            if( Parent != null && Parent is InterRecord )
            {
                Object.transform.parent = ( Parent as InterRecord ).Object.transform;
            }

            // Processes children
            base.ImportIntoScene();

            // Create mesh
            if( Vertices != null && Vertices.Count > 0 )
            {    
                Mesh m = new Mesh();                
                m.name = ID;
                m.vertices = VertexPositions.ToArray();
                m.normals = Normals.ToArray();
                m.uv = UVS.ToArray();
				
                MeshRenderer mr = Object.AddComponent<MeshRenderer>();
                mr.materials = new Material[SubMeshes.Count];
                MeshFilter mf = Object.AddComponent<MeshFilter>();                    
     
                // Set submeshes
                m.subMeshCount = SubMeshes.Count;
                for( int i = 0; i < SubMeshes.Count; i++ )
                {
                    mr.materials[i] = SubMeshes[i].Key.UnityMaterial;                    
                    m.SetTriangles( SubMeshes[i].Value.ToArray(), i );
                }                

                //m.RecalculateNormals(); // TODO: if no normals then recalculate?
                mf.mesh = m;                 
            }
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the submesh for this face based on material info.
        /// </summary>        
        /// <param name='f'>The face to find a submesh for.</param>
        //////////////////////////////////////////////////////////////////
        public KeyValuePair<IntermediateMaterial, List<int>> FindOrCreateSubMesh( Face f )
        {            
            // Fetch palettes
            MaterialPalette mp = f.MaterialIndex != -1 ? f.Header.MaterialPalettes[f.MaterialIndex] : null;
            TexturePalette mainTex = f.TexturePattern != -1 ? f.Header.TexturePalettes[f.TexturePattern] : null;
            TexturePalette detailTex = f.DetailTexturePattern != -1 ? f.Header.TexturePalettes[f.DetailTexturePattern] : null;

            // Check locally
            foreach( KeyValuePair<IntermediateMaterial, List<int>> mesh in SubMeshes )
            {				
                if( mesh.Key.Equals( mp, mainTex, detailTex, f.Transparency, f.LightMode ) )
                {
                    return mesh;
                }
            }

            // Create a new submesh
            IntermediateMaterial im = MaterialBank.Instance.FindOrCreateMaterial( f );
            KeyValuePair<IntermediateMaterial, List<int>> newMesh = new KeyValuePair<IntermediateMaterial, List<int>>( im, new List<int>() );
            SubMeshes.Add( newMesh );
            return newMesh;
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