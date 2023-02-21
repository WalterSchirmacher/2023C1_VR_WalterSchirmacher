using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendOrFoeLOD : FriendOrFoe
{

    public GameObject lOD0, lOD1, lOD2;

    // Start is called before the first frame update
    void Start()
    {
        if (transparancyState == TransStatus.UseMin)
        {
            MakeTransparent();
        }
        else if (transparancyState == TransStatus.UseMax)
        {
            MakeSolid();
        }
    }

    new public void MakeTransparent()
    {
        Debug.Log("Make Transparent");

        if (changeTransparency)
        {
            ChangeTransparancy(lOD0, minAlpha);
            ChangeTransparancy(lOD1, minAlpha);
            ChangeTransparancy(lOD2, minAlpha);
        }
    }


    new public void MakeSolid()
    {
        Debug.Log("Make Solid");

        if (changeTransparency)
        {
            ChangeTransparancy(lOD0, maxAlpha);
            ChangeTransparancy(lOD1, maxAlpha);
            ChangeTransparancy(lOD2, maxAlpha);
        }

    }
}
