using UnityEngine;
using System.Collections;
using System.Text;

namespace UFLT
{
	/// <summary>
	/// Log class. This allows for multiple Debug messages without cluttering the console.
	/// </summary>
	public class Log 
	{
		#region Properties

		/// <summary>
		/// Gets or sets the log builder.		
		/// </summary>		
		public static StringBuilder LogBuilder
		{
			get;
			set;
		}
		
		#endregion Properties	
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates a new log.
		/// </summary>
		//////////////////////////////////////////////////////////////////
		public static void Init()
		{
			// Create a new log
			LogBuilder = new StringBuilder();
		}
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Writes a line to the log.
		/// </summary>
		//////////////////////////////////////////////////////////////////
		public static void Write( string line )
		{
			LogBuilder.AppendLine( line );	
		}
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Writes a warning line to the log.
		/// </summary>
		//////////////////////////////////////////////////////////////////
		public static void WriteWarning( string line )
		{
			LogBuilder.AppendFormat( "WARNING: {0}\n", line );			
		}
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Writes a error line to the log.
		/// </summary>
		//////////////////////////////////////////////////////////////////
		public static void WriteError( string line )
		{
			LogBuilder.AppendFormat( "ERROR: {0}\n", line );			
		}		
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns log data as string.
		/// </summary>
		//////////////////////////////////////////////////////////////////
		public new static string ToString ()
		{
			return LogBuilder.ToString();
		}
	}
}