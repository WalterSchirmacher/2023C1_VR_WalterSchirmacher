using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuckTest : MonoBehaviour
{
    ChuckSubInstance chuckSnd;

    // Start is called before the first frame update
    void Start()
    {
        chuckSnd = GetComponent<ChuckSubInstance>();
        chuckSnd.RunCode(@"
		SinOsc foo => dac;
		while( true )
		{
			Math.random2f( 300, 1000 ) => foo.freq;
			100::ms => now;
		}
	");
    }

}
