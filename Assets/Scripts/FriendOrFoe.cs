using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendOrFoe : MonoBehaviour
{

    public enum MyStatus { Friendly, Neutral, Hostile, ExtremeHatred };
    public enum TransStatus { UseMin, UseMax, None };
    public MyStatus myStatus = MyStatus.Neutral;
    public GameObject mainObject;
    public GameObject parentObject;
    public TransStatus transparancyState = TransStatus.None;
    public bool changeTransparency = true;
    public float minAlpha = 0;
    public float maxAlpha = 30;

    private float currentAlpha;


    // Start is called before the first frame update
    void Start()
    {
        if(transparancyState == TransStatus.UseMin)
        {
            MakeTransparent();     
        } else if (transparancyState == TransStatus.UseMax)
        {
            MakeSolid();
        }
    }

    [ContextMenu("Make Transparent")]
    public void MakeTransparent()
    {
        Debug.Log("Make Transparent");

        if (changeTransparency)
        {
            ChangeTransparancy(mainObject, minAlpha);
        }
    }

    [ContextMenu("Make Solid")]
    public void MakeSolid()
    {
        Debug.Log("Make Solid");

        if (changeTransparency)
        {
                ChangeTransparancy(mainObject, maxAlpha);
        }
         
    }


    public void ChangeTransparancy(GameObject theObj, float theAlpha)
    {
        Renderer ren = theObj.GetComponent<Renderer>();

        currentAlpha = ren.material.color.a;

        if (currentAlpha != theAlpha)
        {
            Color color = ren.material.color;
            color.a = minAlpha;
            ren.material.color = color;
            currentAlpha = theAlpha;
        }

    }

    

}
