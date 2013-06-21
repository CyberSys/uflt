using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UFLT.DataTypes.Enums;

namespace UFLT.Records
{
    /// <summary>
    /// A single object.
    /// </summary>
	public class Object : InterRecord
	{
		#region Properties

        // TODO: props
    
		#endregion Properties

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctr
        /// </summary>
        /// <param name="parent"></param>
        //////////////////////////////////////////////////////////////////
        public Object( Record parent ) :
			base( parent, parent.Header )
		{
            RootHandler.Handler[Opcodes.PushLevel] = HandlePush;
            RootHandler.Handler[Opcodes.LongID] = HandleLongID;
            RootHandler.Handler[Opcodes.Comment] = HandleComment;
            RootHandler.Handler[Opcodes.Matrix] = HandleMatrix; 
            
            RootHandler.ThrowBacks.UnionWith( RecordHandler.ThrowBackOpcodes );

            // TODO: Handle children
		}

        //////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parses binary stream.
        /// </summary>
        //////////////////////////////////////////////////////////////////
        public override void Parse()
        {
            // TODO: You are here!!!!!!!!!!!!!!!!
        }
	}
}