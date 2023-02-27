using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFoe : FriendOrFoe
{
    public AudioSource audioRegular, audioMad;
    public Collider detectedObject;


    private float theVol = 0f;
    private float randomLower = 3f;
    private float randomUpper = 10f;
    private bool stopRegularAudio = false;
    private bool stopMadAudio = false;
    private int fadeAudioTime = 10;

    [HideInInspector]
    public bool isChasing = false;
    

    // Start is called before the first frame update
    void Start()
    {
        myStatus = GameMaster.Disposition.Hostile;
        Damage = gameMaster.GetDamageAmount(myStatus);

        if(!audioRegular || !audioMad)
        {
            Debug.Log("Invalid Audio Source!");
            Debug.Log("AudioSource: " + audioRegular);
            Debug.Log("Short Roar: " + audioMad);
        }
    }

    public void ChasePlayer()
    {
        Debug.Log("Chasing Player");
        isChasing = true;
        myStatus = GameMaster.Disposition.ExtremeHatred;
        Damage = gameMaster.GetDamageAmount(myStatus);
        PauseRegularAudio();
        StartCoroutine(MadRoarAudio());
        aiFSM.currentState = AIFSM.AIState.ChasePlayer;
    }
    
    public void StopChasePlayer()
    {
        Debug.Log("Stopping the Chase");
    }

    public void PlayerLost()
    {
        stopRegularAudio = true;
        stopMadAudio = true;
    }

    public void PlayerFound()
    {
        stopRegularAudio = false;
        stopMadAudio = false;
        theVol = 0f;

        if (OuterPlayerFound && !InnerPlayerFound)
        {
            theVol = 0.25f;
        }
        else if (OuterPlayerFound && InnerPlayerFound)
        {
            theVol = 0.5f;
        }
        audioRegular.volume = theVol;
        audioMad.volume = theVol;
        StartCoroutine(RegularRoarAudio());
        Debug.Log("Roar!");
    }

    IEnumerator RegularRoarAudio()
    {
        Debug.Log("Playing regular");
     
        while (!stopRegularAudio)
        {
            audioRegular.Play();
            yield return new WaitForSeconds(audioRegular.clip.length + Random.Range(randomLower, randomUpper));
        }

        audioRegular.FadeOut(fadeAudioTime);
     }

    IEnumerator MadRoarAudio()
    {
        Debug.Log("Playing mad");

        while (!stopMadAudio)
        {
            audioMad.Play();
            yield return new WaitForSeconds(audioMad.clip.length + (Random.Range(randomLower, randomUpper))/2);
        }

        audioMad.FadeOut(fadeAudioTime);
    }

    IEnumerable PauseRegularAudio()
    {
        stopRegularAudio = true;
        yield return new WaitForSeconds(fadeAudioTime + 1);
        stopRegularAudio = false;
        StartCoroutine(RegularRoarAudio());
    }

}

