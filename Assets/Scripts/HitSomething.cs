using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HitSomething : MonoBehaviour
{

    public enum ManOrWoman { Man, Woman };
    public ManOrWoman manOrWoman = ManOrWoman.Man;
    public AudioSource audioSourceBody;
    public AudioClip _impactSound, _impactMan, _impactWoman;
    public AudioClip[] adClips = new AudioClip[2];

    // Start is called before the first frame update
    void Start()
    { 
        audioSourceBody.GetComponent<AudioSource>();
          
        if (audioSourceBody)
        {
            adClips[0] = _impactSound;

            Debug.Log("Audio sources good");

            if (manOrWoman == ManOrWoman.Man)
            {
               adClips[1] = _impactMan;
            }
            else 
            {
                adClips[1] = _impactWoman;
            }
        } else
        {
            Debug.Log("Invalid Audio Source or Clip!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit a " + collision.gameObject.name + " " + collision.gameObject.tag);
        if(collision.gameObject.name != "Terrain")
        {
            StartCoroutine(PlayAudioSequentially());
        }
        
    }

  IEnumerator PlayAudioSequentially()
    {
        yield return null;
        for (int i = 0; i < adClips.Length; i++)
        {
            audioSourceBody.clip = adClips[i];
            audioSourceBody.Play();

            while (audioSourceBody.isPlaying)
            {
                yield return null;
            }
        }
    }
}
