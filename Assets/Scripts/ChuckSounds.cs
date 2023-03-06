using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuckSounds : MonoBehaviour
{

    ChuckSubInstance chuckSnd;

    private void Awake()
    {
        chuckSnd = GetComponent<ChuckSubInstance>();
    }

    public void AnimalRadarSnd()
    {
        if (chuckSnd)
        {
            chuckSnd.RunCode(@"
		        TriOsc Osc => dac;
                1 => Osc.gain;
                [1,3,7,9,11,13,15,17,19,21,23,25] @=> int minor[];
                12  => int num;
                22 => int offset;
                int position;
                0.03::second => dur beat;
                0 => position;
                for (0 => int i; i < num; i++)
                {
                    Std.mtof(minor[i] + offset + position) => Osc.freq;
                    beat => now;
                }
	        ");
        }
    }

    public void TreeRadarSnd()
    {
        if (chuckSnd)
        {
            chuckSnd.RunCode(@"
		       TriOsc Osc => dac;
                1 => Osc.gain;
                [0,2,4,6,8,10,12,14,16,18,20,22] @=> int major[];
                12  => int num;
                34 => int offset;
                int position;
                0.03::second => dur beat;
                0 => position;
                for(0 => int i; i < num; i++)
                {
                    Std.mtof(major[i] + offset + position) => Osc.freq;
                    beat => now;
                }
	        ");
        }
    }

    public void BushRadarSnd()
    {
        if (chuckSnd)
        {
            chuckSnd.RunCode(@"
                Phasor Osc => dac;
                0.8 => Osc.gain;
                [0,2,4,6,8,10,12,14,16,18,20,22] @=> int major[];
                12  => int num;
                46 => int offset;
                int position;
                0.03::second => dur beat;
                0 => position;
                for(0 => int i; i < num; i++)
                {
                    Std.mtof(major[i] + offset + position) => Osc.freq;
                    beat => now;
                }
	        ");
        }
    }

    public void RockRadarSnd()
    {
        if (chuckSnd)
        {
            chuckSnd.RunCode(@"
		            SawOsc Osc => dac;
                    1 => Osc.gain;
                    [1,3,7,9,11,13,15,17,19,21,23,25] @=> int minor[];
                    12  => int num;
                    22 => int offset;
                    int position;
                    0.03::second => dur beat;
                    0 => position;
                    for(0 => int i; i < num; i++)
                    {
                        Std.mtof(minor[i] + offset + position) => Osc.freq;
                        beat => now;
                    }
	       ");
        }
    }

    public void MushroomRadarSnd()
    {
        if (chuckSnd)
        {
            chuckSnd.RunCode(@"
                SinOsc Osc => ADSR env1 => dac;
                0.8 => Osc.gain;
                8  => int num;
                56 => int offset;
                int position;
                0.03::second => dur beat;
                (beat / 2, beat /2, 0, 1::ms) => env1.set;
                3 => int numLoops;
                for (0 => int j; j < numLoops; j++) { 
                    0 => position;
                    for(0 => int i; i < num; i++)
                    {
                        Std.mtof(i + offset + position) => Osc.freq;
                        1 => env1.keyOn;
                        beat => now;
                    }
                }
	        ");
        }
    }
    public void OtherRadarSnd()
    {
        if (chuckSnd)
        {
            chuckSnd.RunCode(@"
		       TriOsc Osc => ADSR env1 => dac;
                1 => Osc.gain;
                24  => int num;
                56 => int offset;
                int position;
                0.02::second => dur beat;
                (beat / 2, beat /2, 0, 1::ms) => env1.set;
                4 => int numLoops;
                for (0 => int j; j < numLoops; j++) { 
                    0 => position;
                    for(0 => int i; i < num; i++)
                    {
                        Std.mtof(i + offset + position) => Osc.freq;
                        1 => env1.keyOn;
                        beat => now;
                    }
                }
	        ");
        }
    }
}
