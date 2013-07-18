using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UFLT.Records;
using System;

namespace UFLT.Controllers
{
	[AddComponentMenu( "UFLT/OpenFlight Loader" )]
	public class OpenFlightLoader : MonoBehaviour 
	{
		#region Properties
		
		// How many files can be simultaneously. 
		public int filesToLoadSimultaneously = 1;	
		
		// Import settings for all files.
		public ImportSettings settings = new ImportSettings();
		
		private static OpenFlightLoader instance = null;
		
		/// <summary>
		/// Singleton instance.
		/// </summary>		
		public static OpenFlightLoader Instance
		{
			get
			{
				if( instance == null )
				{			
					GameObject go = new GameObject( "OpenFlight Loader" );
					instance = go.AddComponent<OpenFlightLoader>();
				}
				
				return instance;
			}
		}
						
		private class LoadRequest
		{
			public string path;
			public Database root;
			public Action<Database> callback;
			public ImportSettings settings;
		}				
		
		// Load Queue.
		private Queue<LoadRequest> Queue = new Queue<LoadRequest>();

		// Files currently being loaded.
		private List<LoadRequest> beingProcessed = new List<LoadRequest>();
		
		#endregion
			
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Init
		/// </summary>
		//////////////////////////////////////////////////////////////////
		private void Start() 
		{
			if( instance == null || instance == this )
			{
				instance = this;				
			}
			else
			{
				Debug.LogWarning( "Only one instance should exist!" );
				Destroy( this );
			}
		}		
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Loads the open flight file when a loading slot is available.
		/// </summary>
		/// <param name='file'>The openflight file to load.</param>
		/// <param name='callback'>Callback when the file has finished loading into the scene. Can be null.</param>
		/// <param name='settings'>Custom import settings for this file.</param>
		//////////////////////////////////////////////////////////////////
		public static void LoadOpenFlight( string file, Action<Database> callback, ImportSettings settings )
		{			
			OpenFlightLoader l = Instance;
			
			LoadRequest lr = new LoadRequest();
			lr.path = file;
			lr.callback = callback;
			lr.settings = settings;
			
			l.Queue.Enqueue( lr );			
			l.UpdateLoaders();			
		}
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Loads the open flight file when a loading slot is available.
		/// </summary>
		/// <param name='file'>The openflight file to load.</param>
		/// <param name='callback'>Callback when the file has finished loading into the scene. Can be null.</param>		
		//////////////////////////////////////////////////////////////////
		public static void LoadOpenFlight( string file, Action<Database> callback )
		{									
			LoadOpenFlight( file, callback, Instance.settings );				
		}		
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Starts up new loaders if a slot is free.
		/// </summary>
		//////////////////////////////////////////////////////////////////
		private void UpdateLoaders()
		{			
			while( beingProcessed.Count < filesToLoadSimultaneously && Queue.Count > 0 )
			{
				StartCoroutine( ProcessFile( Queue.Dequeue() ) );								
			}
		}		
		
		//////////////////////////////////////////////////////////////////
		/// <summary>
		/// Coroutine to loads the database.
		/// </summary>		
		/// <param name="file">File to load</param>
		//////////////////////////////////////////////////////////////////
		private IEnumerator ProcessFile( LoadRequest file )
		{
			// Register that the file is being loaded.
			beingProcessed.Add( file );
			
			// Now start loading the file			   
        	file.root = new Database( file.path, null, settings );					
	        yield return StartCoroutine( file.root.ParseAsynchronously( this ) ); 
			
			// Import into our scene.
        	file.root.ImportIntoScene(); 
			
			// Callback?
			if( file.callback != null )
			{
				file.callback( file.root );	
			}
			
			beingProcessed.Remove( file );
			
			UpdateLoaders();					
		}
	}
}