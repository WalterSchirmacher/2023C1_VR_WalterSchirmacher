using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HitSomething : MonoBehaviour
{


    public AudioSource audioSourceRock, audioSourceTree;

    public int rockHits = 0;
    public int treeHits = 0;


    // Start is called before the first frame update
    void Start()
    {
       // audioSourceRock.Play();
    }

    private void OnCollisionEnter(Collision collision)
        {
        string theTag = collision.gameObject.tag;
            if (theTag == "Rock")
            {
                rockHits++;
                audioSourceRock.Play();
            Debug.Log("Hit a rock");
            } else if (theTag == "Tree")
            {
                treeHits++;
               audioSourceTree.Play();
            Debug.Log("Hit a tree!");
             } else
            {
                Debug.Log("Collided with something other than a Rock or a Tree");
            }
       // collision.gameObject.SetActive(false);
        }
}
