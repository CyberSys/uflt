using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UFLT.DataTypes.Enums;
using UFLT.Utils;

namespace UFLT.Records
{
    /// <summary>
    /// The Degree Of Freedom node specifies a local coordinate system and the range allowed 
    /// for translation, rotation, and scale with respect to that coordinate system.
    /// I.E, a pivot point such as a Tank Turret or gun pivot.
    /// </summary>
	public class DOF : InterRecord
	{
		#region Properties

        /// <summary>
        /// Origin of DOF's local coordinate system.(x, y, z).
        /// </summary>
        public double[] Origin
        {
            get;
            set;
        }

        /// <summary>
        /// Point on x axis of DOF's local coordinate system (x, y, z).
        /// </summary>
        public double[] PointOnXAxis
        {
            get;
            set;
        }

        /// <summary>
        /// Point in xy plane of DOF's local coordinate system (x, y, z)
        /// </summary>
        public double[] PointInXYPlane
        {
            get;
            set;
        }

        /// <summary>
        /// Min, Max, Current & Increment of x with respect to local coordinate system.
        /// </summary>
        public double[] MinMaxCurrentIncrementX
        {
            get;
            set;
        }

        /// <summary>
        /// Min, Max, Current & Increment of y with respect to local coordinate system.
        /// </summary>
        public double[] MinMaxCurrentIncrementY
        {
            get;
            set;
        }

        /// <summary>
        /// Min, Max, Current & Increment of z with respect to local coordinate system.
        /// </summary>
        public double[] MinMaxCurrentIncrementZ
        {
            get;
            set;
        }

        /// <summary>
        /// Min, Max, Current & Increment of pitch with respect to local coordinate system.
        /// </summary>
        public double[] MinMaxCurrentIncrementPitch
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
        public DOF( Record parent ) :
			base( parent, parent.Header )
		{
            //RootHandler.Handler[Opcodes.PushLevel] = HandlePush;
            //RootHandler.Handler[Opcodes.LongID] = HandleLongID;
            //RootHandler.Handler[Opcodes.Comment] = HandleComment;
            //RootHandler.Handler[Opcodes.Matrix] = HandleMatrix;
            
            //RootHandler.ThrowBacks.UnionWith( RecordHandler.ThrowBackOpcodes );
            
            //ChildHandler.Handler[Opcodes.Face] = HandleFace;
            //// TODO: index light point
            //// TODO: inline light point
            //ChildHandler.Handler[Opcodes.PushLevel] = HandlePush;
            //ChildHandler.Handler[Opcodes.PopLevel] = HandlePop;
		}

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses binary stream.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public override void Parse()
        {
            ID                               = NullTerminatedString.GetAsString( Header.Stream.Reader.ReadBytes( 8 ) );
            /* Skip reserved bytes*/         Header.Stream.Reader.BaseStream.Seek( 4, SeekOrigin.Current );
            Origin                           = new double[] { Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble() };
            PointOnXAxis                     = new double[] { Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble() };
            PointInXYPlane                   = new double[] { Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble() };
            MinMaxCurrentIncrementZ          = new double[] { Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble() };
            MinMaxCurrentIncrementY          = new double[] { Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble() };
            MinMaxCurrentIncrementX          = new double[] { Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble(), Header.Stream.Reader.ReadDouble() };


            //Flags = Header.Stream.Reader.ReadInt32();
            //RelativePriority = Header.Stream.Reader.ReadInt16();
            //Transparency = Header.Stream.Reader.ReadUInt16();
            //SpecialEffectID1 = Header.Stream.Reader.ReadInt16();
            //SpecialEffectID2 = Header.Stream.Reader.ReadInt16();
            //// Ignore last 2 reserved bytes.    
        
            //// Parse children
            //base.Parse();
        }
	}
}