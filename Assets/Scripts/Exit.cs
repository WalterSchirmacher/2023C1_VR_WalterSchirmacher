using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Exit : MonoBehaviour
{
    public GameObject tmp;
    public AudioSource audioSource;
    // Start is called before the first frame update

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        tmp.SetActive(true);
        Time.timeScale = 0;
        audioSource.Play();
    }
}
