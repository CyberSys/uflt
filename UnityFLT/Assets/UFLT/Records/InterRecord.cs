using UnityEngine;
using System.Collections;
using UFLT.DataTypes.Enums;
using System.IO;
using System.Text;

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

        /// <summary>
        /// Mesh data for this object.
        /// </summary>
        public Mesh Mesh
        {
            get;
            set;
        }

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
            // Create an empty gameobject
            Object = new GameObject( ID );

            // Assign parent
            if( Parent != null && Parent is InterRecord )
            {
                Object.transform.parent = ( Parent as InterRecord ).Object.transform;
            }

            // Processes children
            base.ImportIntoScene();
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
            //Debug.Log( "Handle " + Header.Stream.Opcode );
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
            //Debug.Log( "Handle " + Header.Stream.Opcode );
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
            //Debug.Log( "Handle " + Header.Stream.Opcode );
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
            //Debug.Log( "Handle " + Header.Stream.Opcode );
            Group g = new Group( this );
            g.Parse();
            return true;
        }

        #endregion Record Handlers
    }
}