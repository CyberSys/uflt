using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

namespace UFLT.Records
{
    /// <summary>
    /// A vertex with color & UV data.
    /// </summary>
	public class VertexWithColorUV : VertexWithColor
	{
		#region Properties

        /// <summary>
        /// x,y uv texture coordinates.
        /// </summary>
        public double[] UV
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
        public VertexWithColorUV( Record parent ) :
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
            Coordinate = new double[] { Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble() };
            UV = new double[] { Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble() };
            
            Color32 c = new Color32();
            c.a = Header.Stream.Reader.ReadByte();
            c.b = Header.Stream.Reader.ReadByte();
            c.g = Header.Stream.Reader.ReadByte();
            c.r = Header.Stream.Reader.ReadByte();
            PackedColor = c;

            VertexColorIndex = Header.Stream.Reader.ReadUInt32();
        }
	}
}