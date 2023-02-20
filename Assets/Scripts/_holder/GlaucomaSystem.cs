using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlaucomaSystem : MonoBehaviour
{
    private GameObject ving;
    public bool showGlaucoma;

    // Start is called before the first frame update
    void Start()
    {
        ving = GameObject.Find("TunnelingVignette");

        if (showGlaucoma)
        {
            GlaucomaView(true);
        } else
        {
            GlaucomaView(false);
        }
    }

    void GlaucomaView(bool showIt)
    {

        ving.SetActive(showIt);

    }
}
