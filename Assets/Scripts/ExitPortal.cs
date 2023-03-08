using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortal : MonoBehaviour
{

    public AudioSource audioSrc;
    public GameObject goFX;
    public GameObject arch;

    private void Awake()
    {
        arch.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        StartPortal();
    }

    [ContextMenu("StartPortal")]
    public void StartPortal()
    {
        arch.SetActive(true);
        audioSrc.Play();
        goFX.SetActive(true);
    }
}

