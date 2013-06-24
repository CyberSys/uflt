using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UFLT.DataTypes.Enums;

namespace UFLT.Records
{
    /// <summary>
    /// A record that is not fully handled but required in order to access child records.
    /// </summary>
	public class Unhandled : InterRecord
	{
        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctr
        /// </summary>
        /// <param name="parent"></param>
        //////////////////////////////////////////////////////////////////
        public Unhandled( Record parent ) :
			base( parent, parent.Header )
		{
            RootHandler.Handler[Opcodes.PushLevel] = HandlePush;
            RootHandler.Handler[Opcodes.LongID] = HandleLongID;
            RootHandler.Handler[Opcodes.Comment] = HandleComment;
            RootHandler.Handler[Opcodes.Matrix] = HandleMatrix; 
            
            RootHandler.ThrowBacks.UnionWith( RecordHandler.ThrowBackOpcodes );

            ChildHandler.Handler[Opcodes.Group] = HandleGroup;            
            ChildHandler.Handler[Opcodes.Object] = HandleObject;
            ChildHandler.Handler[Opcodes.PushLevel] = HandlePush;
            ChildHandler.Handler[Opcodes.PopLevel] = HandlePop;
            ChildHandler.Handler[Opcodes.Switch] = HandleUnhandled;
            ChildHandler.Handler[Opcodes.DegreeOfFreedom] = HandleUnhandled;
            ChildHandler.Handler[Opcodes.Sound] = HandleUnhandled;
            ChildHandler.Handler[Opcodes.ClipRegion] = HandleUnhandled;

            ChildHandler.Handler[Opcodes.LevelOfDetail] = HandleUnhandled;
            ChildHandler.Handler[Opcodes.ExternalReference] = HandleUnhandled;
		}

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses binary stream.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public override void Parse()
        {
            ID = Encoding.ASCII.GetString( Header.Stream.Reader.ReadBytes( 8 ) );
            base.Parse();
        }
	}
}