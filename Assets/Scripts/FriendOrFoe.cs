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
    public float minAlpha = 0;
    public float maxAlpha = 0;
    public float currentAlpha;

    // Start is called before the first frame update
    void Start()
    {
        if(makeTransparent)
        {
            Renderer ren = GetComponent<Renderer>();
            float matMode = ren.material.GetFloat("_mode");

            if (matMode == 0 || matMode == 1)
            {
                StandardShaderUtils.ChangeRenderMode(ren.material, StandardShaderUtils.BlendMode.Transparent);
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

}
