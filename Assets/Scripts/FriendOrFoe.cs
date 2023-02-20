using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendOrFoe : MonoBehaviour
{

    public enum MyStatus { Friendly, Neutral, Hostile, ExtremeHatred };
    public MyStatus myStatus = MyStatus.Neutral;
    public GameObject mainObject;
    public bool moveParent = false;
    public GameObject parentObject;
    public bool makeTransparent = false;
    public bool changeTransparency = true;
    public float minAlpha = 0;
    public float maxAlpha = 30;

    private float currentAlpha;
    private bool changedToTrans = false;

    // Start is called before the first frame update
    void Start()
    {
        if(makeTransparent)
        {
            MakeTransparent();
        }

    }

    public void MakeTransparent()
    {
        Debug.Log("Make Transparent");

        if(changeTransparency)
        {
            Renderer ren = GetComponent<Renderer>();

            if(!changedToTrans)
            {
                ChangeMaterialToTrans(ren);
            }

            currentAlpha = ren.material.color.a;

            if (currentAlpha != minAlpha)
            {
                Color color = ren.material.color;
                color.a = minAlpha;
                ren.material.color = color;
                currentAlpha = minAlpha;
            }
        }

    }

    public void MakeSolid()
    {
        Debug.Log("Make Solid");

        if (changeTransparency)
        {
            Renderer ren = GetComponent<Renderer>();

            if (!changedToTrans)
            {
                ChangeMaterialToTrans(ren);
            }

            currentAlpha = ren.material.color.a;

            if (currentAlpha != maxAlpha)
            {
                Color color = ren.material.color;
                color.a = maxAlpha;
                ren.material.color = color;
                currentAlpha = maxAlpha;
            }
        }
         
    }

    void ChangeMaterialToTrans(Renderer theRen)
    {
        float matMode = theRen.material.GetFloat("_mode");

        if (matMode == 0 || matMode == 1)
        {
            StandardShaderUtils.ChangeRenderMode(theRen.material, StandardShaderUtils.BlendMode.Transparent);
        }
        changedToTrans = true;
    }
    

}
