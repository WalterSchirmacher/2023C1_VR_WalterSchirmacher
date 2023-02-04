using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitCameraController : MonoBehaviour
{
    public Camera mainCamera;
    public Camera leftCamera;
    public Camera rightCamera;
    public bool showSplit;

    // Start is called before the first frame update
    void Start()
    {
        if (showSplit)
        {
            SplitView(true);
        }
        else
        {
            SplitView(false);
        }
    }

    void SplitView(bool turnon)
    {
        if(turnon)
        {
            mainCamera.enabled = false;
            leftCamera.enabled = true;
            rightCamera.enabled = true;
        } else
        {
            mainCamera.enabled = true;
            leftCamera.enabled = false;
            rightCamera.enabled = false;
        }

    }
}
