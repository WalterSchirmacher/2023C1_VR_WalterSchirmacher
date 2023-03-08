using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bushes : MonoBehaviour
{
    public string gTag;
    // Start is called before the first frame update
    void Start()
    {
        gTag = gameObject.tag;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "DetectorCone")
        {
            GameMaster.Instance.AddtoVisible(gameObject, gTag);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "DetectorCone")
        {
            GameMaster.Instance.NoLongerVisible(gameObject, gTag);
        }
    }
}
