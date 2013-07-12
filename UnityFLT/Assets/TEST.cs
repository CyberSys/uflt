using UnityEngine;
using System.Collections;
using System.IO;
using UFLT.Records;
using System.Threading;
using UFLT;
using UFLT.Textures;
using UFLT.Utils;

public class TEST : MonoBehaviour 
{
    public string file;

    public ImportSettings settings = new ImportSettings();
        		
	public Texture2D tex;
	
	IEnumerator Start ()
    {
		// TODO: WHY ARE MATERIALS NOT SHARED. E.G load the same file twice, works for textures but not materials.
		
		//gameObject.AddComponent( typ );
		
		//WWW www = new WWW( "file://" + @"D:\Desktop\TruckTop.jpg" );
		//yield return www;
		//tex = www.texture;
		
		
        // How to load a database multithreaded
        Database db = new Database( file, null, settings );

        // Single-threaded
        //db.ParsePrepareAndImport();

        // Multi-threaded
        yield return StartCoroutine( db.ParseAsynchronously() );        
        db.ImportIntoScene(); 
        db.Cleanup();
        //MaterialBank.instance = null;

        // TODO: Material bank should not keep references or should clear them, maybe create a gameobject to keep reference of this stuff instead of static?
        MaterialBank.instance.Materials.Clear();

        db = null;
        //settings = null;
        
        yield return new WaitForSeconds( 4 );
        System.GC.Collect();
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
