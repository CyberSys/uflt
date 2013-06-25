using UnityEngine;
using System.Collections;
using System.IO;
using UFLT.Records;
using System.Threading;

public class TEST : MonoBehaviour 
{
    public string file;

    public Thread t;

    public Database db;

	// Use this for initialization
	void Start ()
    {
        db = new Database( file );
        db.Parse();
        db.ImportIntoScene();
        //t = new Thread( ThreadStart );
        //t.Start();

	}

    /// <summary>
    /// Testing what is safe to create/use in an other thread.
    /// </summary>
    void ThreadStart()
    {
        //Vector2 v = new Vector2( 0, 0 );
        //Debug.Log( v );

        //Color c = new Color();
        //Debug.Log( c );

        //Texture2D t = new Texture2D( 1024, 1024 ); // can not be created
        ////Debug.Log( t );
    }
}
