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
	
	void Start()
    {		
		UFLT.Controllers.OpenFlightLoader.LoadOpenFlight( file, OnFileLoaded, settings );		
	}
			
	void OnFileLoaded( Database db )
	{
		Debug.Log( "File loaded" );		
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
