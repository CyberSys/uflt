using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UFLT.DataTypes.Enums;

namespace UFLT.Records
{
    /// <summary>
    /// Double precision vertex records are stored in a vertex palette for the entire database. Vertices 
    /// shared by one or more geometric entities are written only one time in the vertex palette. This 
    /// reduces the overall size of the OpenFlight file by writing only �unique� vertices. Vertex palette 
    /// records are referenced by faces and light points via vertex list and morph vertex list records
    /// </summary>
	public class VertexPalette : Record
	{
		#region Properties

        /// <summary>
        /// Next offset value top be parsed.
        /// </suvmmary>
        private int Offset
        {
            get;
            set;
        }

        /// <summary>
        /// Length of this record plus all vertices.
        /// </summary>
        public int LengthPlusVertexPalette
        {
            get;
            set;
        }

        /// <summary>
        /// Vertex records with offset as dictionary key.
        /// </summary>
        public Dictionary<int, VertexWithColor> Vertices
        {
            get;
            set;
        }

		#endregion Properties

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctr
        /// </summary>
        /// <param name="parent"></param>
        //////////////////////////////////////////////////////////////////
        public VertexPalette( Record parent ) :
			base( parent, parent.Header )
		{
            RootHandler.Handler[Opcodes.VertexWithColor] = HandleVertexColor;
            RootHandler.Handler[Opcodes.VertexwithColorAndNormal] = HandleVertexColorNormal;
            RootHandler.Handler[Opcodes.VertexWithColorAndUV] = HandleVertexColorUV;
            RootHandler.Handler[Opcodes.VertexWithColorNormalAndUV] = HandleVertexColorNormalUV;

            RootHandler.ThrowBackUnhandled = true;

            Vertices = new Dictionary<int, VertexWithColor>();
		}

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses binary stream.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public override void Parse()
        {            
            LengthPlusVertexPalette = Header.Stream.Reader.ReadInt32();
            Offset = 8;

            // Parse vertices
            base.Parse();
        }

        #region Record Handlers

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handle vertex with color.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        private bool HandleVertexColor()
        {
            VertexWithColor v = new VertexWithColor( this );
            v.Parse();
            Vertices[Offset] = v;
            Offset += v.Length;
            return true;
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handle vertex with color.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        private bool HandleVertexColorNormal()
        {
            VertexWithColorNormal v = new VertexWithColorNormal( this );
            v.Parse();
            Vertices[Offset] = v;
            Offset += v.Length;
            return true;
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handle vertex with color.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        private bool HandleVertexColorUV()
        {
            VertexWithColorUV v = new VertexWithColorUV( this );
            v.Parse();
            Vertices[Offset] = v;
            Offset += v.Length;
            return true;
        }

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handle vertex with color.
        /// </summary>
        /// <returns></returns>
        //////////////////////////////////////////////////////////////////
        private bool HandleVertexColorNormalUV()
        {
            VertexWithColorNormalUV v = new VertexWithColorNormalUV( this );
            v.Parse();
            Vertices[Offset] = v;
            Offset += v.Length;
            return true;
        }

        #endregion Record Handlers
	}
}