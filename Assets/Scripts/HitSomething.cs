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
    [HideInInspector]
    public AudioClip[] adClips = new AudioClip[2];
    public Terrain theTerrain;
    private int terrainLayer;

    // Start is called before the first frame update
    void Start()
    {
        terrainLayer = LayerMask.NameToLayer("Terrain");
        // Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), theTerrain.GetComponent<Collider>());

        audioSourceBody.GetComponent<AudioSource>();
          
        if (audioSourceBody && _impactMan && _impactWoman)
        {
            adClips[0] = _impactSound;

    //        Debug.Log("Audio sources good");

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
            Debug.Log("AudioSource: " + audioSourceBody);
            Debug.Log("Impact Man: " + _impactMan);
            Debug.Log("Impact Woman: " + _impactWoman);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Terrain" && collision.gameObject.tag != "DetectorSphere")
        {
          //  Debug.Log("Hit a " + collision.gameObject.name + " " + collision.gameObject.tag);
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
