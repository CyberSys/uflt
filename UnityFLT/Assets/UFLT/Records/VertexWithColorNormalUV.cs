using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

namespace UFLT.Records
{
    /// <summary>
    /// A vertex with color, normal & UV data.
    /// </summary>
	public class VertexWithColorNormalUV : VertexWithColorNormal
	{
		#region Properties

        /// <summary>
        /// x,y uv texture coordinates.
        /// </summary>
        public Vector2 UV
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
        public VertexWithColorNormalUV( Record parent ) :
			base( parent )
		{			
		}

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses binary stream.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public override void Parse()
        {
            ColorNameIndex = Header.Stream.Reader.ReadUInt16();
            Flags = Header.Stream.Reader.ReadInt16();
            Coordinate = new double[3]{ Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble() };			
            Normal = new Vector3( Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle() );												            			
			UV = new Vector2( Header.Stream.Reader.ReadSingle(), Header.Stream.Reader.ReadSingle() );
            
            Color32 c = new Color32();
            c.a = Header.Stream.Reader.ReadByte();
            c.b = Header.Stream.Reader.ReadByte();
            c.g = Header.Stream.Reader.ReadByte();
            c.r = Header.Stream.Reader.ReadByte();
            PackedColor = c;

            VertexColorIndex = Header.Stream.Reader.ReadUInt32();
            // Last 4 bytes are reserved
        }
	}
}