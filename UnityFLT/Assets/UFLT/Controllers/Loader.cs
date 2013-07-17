using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UFLT.Records;

namespace UFLT.Controllers
{
	public class Loader : MonoBehaviour 
	{
		#region Properties
		
		// How many files can be simultaneously. 
		public int filesToLoadSimultaneously = 1;
		
		private static Loader instance;
				
		/// <summary>
		/// Singleton instance.
		/// </summary>		
		public static Loader Instance
		{
			get
			{
				if( instance == null )
				{			
					GameObject go = new GameObject( "OpenFlight Loader" );
					instance = go.AddComponent<Loader>();
				}
				
				return instance;
			}
		}
				
		public class LoadItem
		{
			public string path;
			public Database root;
			public OnFileLoaded callback;
		}
		
		/// <summary>
		/// Load Queue.
		/// </summary>		
		private Queue<LoadItem> Queue
		{
			get;
			set;
		}
		
		#endregion
		
		public delegate void OnFileLoaded( Database loadedDb );
	
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Init
		/// </summary>
		//////////////////////////////////////////////////////////////////
		private void Start() 
		{
			if( instance == null )
			{
				instance = this;				
			}
			else
			{
				Debug.LogWarning( "Only one instance should exist!" );
				Destroy( this );
			}
		}
		
		public static void LoadOpenFlight( string file, OnFileLoaded callback )
		{
			//Loader l = Instance;
			// TODO: youa re here.
			
		}
		
	}
}