using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortal : MonoBehaviour
{

    public AudioSource audioSrc;
    public GameObject goFX;


    private void OnTriggerEnter(Collider other)
    {
        StartPortal();
    }

    [ContextMenu("StartPortal")]
    public void StartPortal()
    {
        audioSrc.Play();
        goFX.SetActive(true);
    }
}

