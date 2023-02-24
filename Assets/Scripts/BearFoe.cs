using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearFoe : FriendOrFoe
{
    public AudioSource audioSource;
    public AudioClip _shortRoar, _longRoaring;
    private float theVol = 0f;
    private float randomLower = 3f;
    private float randomUpper = 10f;
    private bool stopAudio = false;
    private int fadeAudioTime = 10;

    // Start is called before the first frame update
   void Start()
    {
        Debug.Log("Bear Ready.");

        if(!audioSource || !_shortRoar || !_longRoaring)
        {
            Debug.Log("Invalid Audio Source or Clip!");
        }
    }


    public void ChasePlayer()
    {
        audioSource.clip = _shortRoar;
        StartCoroutine(ShortRoar());
    }

    IEnumerator ShortRoar()
    {
        float waitTime = audioSource.clip.length + 3;
        audioSource.Play();
        yield return new WaitForSeconds(waitTime);
        MoreChasing();
    }

    private void MoreChasing()
    {
        Debug.Log("Continue the chase");
        audioSource.clip = _longRoaring;
        StopCoroutine(RoarAudio());
    }
    
    public void PlayerLost()
    {
        stopAudio = true;
       // StopCoroutine(RoarAudio());
    }

    public void PlayerFound()
    {
        stopAudio = false;
        theVol = 0f;

        audioSource.clip = _longRoaring;

        if (OuterPlayerFound && !InnerPlayerFound)
        {
            theVol = 0.25f;
        }
        else if (OuterPlayerFound && InnerPlayerFound)
        {
            theVol = 0.5f;
        }
        audioSource.volume = theVol;
        StartCoroutine(RoarAudio());
        Debug.Log("Roar!");
    }

    IEnumerator RoarAudio()
    {
        float waitTime = audioSource.clip.length + Random.Range(randomLower, randomUpper);
        Debug.Log("Wait Time: " + waitTime);

        audioSource.Play();

        if(stopAudio)
        {
            yield return new WaitForSeconds(waitTime);
            
        } else
        {
            audioSource.FadeOut(fadeAudioTime);
        }
     }

}

