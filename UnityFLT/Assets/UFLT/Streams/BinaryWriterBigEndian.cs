using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System;

namespace UFLT.Streams
{
    /// <summary>
    /// BinaryWriter that supports writing little-endian to big-endian data.
    /// </summary>
    public class BinaryWriterBigEndian : BinaryWriter
    {
        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="input"></param>
        //////////////////////////////////////////////////////////////////////
        public BinaryWriterBigEndian( Stream input ) :
            base( input )
        {
        }
        
        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Swap endian and write data.
        /// </summary>
        /// <param name="value"></param>
        //////////////////////////////////////////////////////////////////////
        public override void Write( float value )
        {            
            // Get raw data
            byte[] bytes = BitConverter.GetBytes( value );

            // Endian swap
            Array.Reverse( bytes );

            Write( bytes );            
        }

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Swap endian and write data.
        /// </summary>
        /// <param name="value"></param>
        //////////////////////////////////////////////////////////////////////
        public override void Write( double value )
        {
            // Get raw data
            byte[] bytes = BitConverter.GetBytes( value );

            // Endian swap
            Array.Reverse( bytes );

            Write( bytes ); 
        }

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Swap endian and write data.
        /// </summary>
        /// <param name="value"></param>
        //////////////////////////////////////////////////////////////////////
        public override void Write( ushort value )
        {
            // Get raw data
            byte[] bytes = BitConverter.GetBytes( value );

            // Endian swap
            Array.Reverse( bytes );

            Write( bytes ); 
        }

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Swap endian and write data.
        /// </summary>
        /// <param name="value"></param>
        //////////////////////////////////////////////////////////////////////
        public override void Write( short value )
        {
            base.Write( IPAddress.NetworkToHostOrder( value ) );
        }

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Swap endian and write data.
        /// </summary>
        /// <param name="value"></param>
        //////////////////////////////////////////////////////////////////////
        public override void Write( uint value )
        {
            // Get raw data
            byte[] bytes = BitConverter.GetBytes( value );

            // Endian swap
            Array.Reverse( bytes );

            Write( bytes ); 
        }

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Swap endian and write data.
        /// </summary>
        /// <param name="value"></param>
        //////////////////////////////////////////////////////////////////////
        public override void Write( int value )
        {
            base.Write( IPAddress.NetworkToHostOrder( value ) );
        }

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Swap endian and write data.
        /// </summary>
        /// <param name="value"></param>
        //////////////////////////////////////////////////////////////////////
        public override void Write( ulong value )
        {
            // Get raw data
            byte[] bytes = BitConverter.GetBytes( value );

            // Endian swap
            Array.Reverse( bytes );

            Write( bytes ); 
        }

        //////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Swap endian and write data.
        /// </summary>
        /// <param name="value"></param>
        //////////////////////////////////////////////////////////////////////
        public override void Write( long value )
        {
            base.Write( IPAddress.NetworkToHostOrder( value ) );
        }
    }
}