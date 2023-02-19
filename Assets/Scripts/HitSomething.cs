using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HitSomething : MonoBehaviour
{

    public enum ManOrWoman { Man, Woman };
    public ManOrWoman manOrWoman = ManOrWoman.Man;
    public AudioSource audioSourceRock, audioSourceTree, audioSourceBush;
    public AudioClip _rockSoundMan, _rockSoundWoman, _treeSoundMan, _treeSoundWoman, _bushSoundMan, _bushSoundWoman;

    public int rockHits = 0;
    public int treeHits = 0;
    public int bushHits = 0;


    // Start is called before the first frame update
    void Start()
    {
        audioSourceRock.GetComponent<AudioSource>();
        audioSourceTree.GetComponent<AudioSource>();

        if (audioSourceRock && audioSourceTree && audioSourceBush && _rockSoundMan && _treeSoundMan && _bushSoundMan && _rockSoundWoman && _treeSoundWoman && _bushSoundWoman)
        {
            Debug.Log("Audio sources good");

            if(manOrWoman == ManOrWoman.Man)
            {
                audioSourceRock.clip = _rockSoundMan;
                audioSourceTree.clip = _treeSoundMan;
                audioSourceBush.clip = _bushSoundMan;
            } else
            {
                audioSourceRock.clip = _rockSoundWoman;
                audioSourceTree.clip = _treeSoundWoman;
                audioSourceBush.clip = _bushSoundWoman;
            }
        } else
        {
            Debug.Log("Invalid Audio Source or Clip!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string theTag = collision.gameObject.tag;

        switch (theTag)
        {
            case "Rock":
                rockHits++;
                audioSourceRock.Play();
                break;
            case "Tree":
                treeHits++;
                audioSourceTree.Play();
                break;
            case "Bush":
                bushHits++;
                audioSourceBush.Play();
                break;
            default:
                Debug.Log("Collided with something other than a Rock, Tree, or a Bush");
                break;
        }

        if(theTag != string.Empty)
        {
            Debug.Log("Hit a " + theTag);
        }
       
        // collision.gameObject.SetActive(false);
    }
}
