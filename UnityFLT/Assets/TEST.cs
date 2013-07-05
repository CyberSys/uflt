using UnityEngine;
using System.Collections;
using System.IO;
using UFLT.Records;
using System.Threading;
using UFLT;
using UFLT.Textures;

public class TEST : MonoBehaviour 
{
    public string file;

    public Thread t;

    public Database db;
	
	public Texture2D tex;
	
	void Start ()
    {	
		TextureSGI s = new TextureSGI( file );
		tex = s.Texture;
		
		// WHY ARE MATERIALS NOT SHARED. E.G load the same file twice, works for textures but not materials.
		
		
		//WWW www = new WWW( "file://" + @"D:\Desktop\TruckTop.jpg" );
		//yield return www;
		//tex = www.texture;
		
		
        // How to load a database multithreaded
        //db = new Database( file );

        // Single-threaded
        //db.ParsePrepareAndImport();

        // Multi-threaded
        //yield return StartCoroutine( db.ParseAsynchronously() );        
        //db.ImportIntoScene();
		
		// Print out log 
		//Debug.Log( Log.ToString() );		        
	}
  

    /// <summary>
    /// Testing what is safe to create/use in an other thread.
    /// </summary>
    void ThreadStart()
    {
		// Materials can only be created from main thread.
		//Material m = new Material("Diffuse");
		//Debug.Log( m );
        //Vector2 v = new Vector2( 0, 0 );
        //Debug.Log( v );

        //Color c = new Color();
        //Debug.Log( c );

        //Texture2D t = new Texture2D( 1024, 1024 ); // can not be created
        ////Debug.Log( t );
    }
}
