using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bushes : MonoBehaviour
{
    public string gTag;
    public float damangeWaitTime = 1;
    private float dmgWaitTimer = 0f;
    public float Damage { get; set; } = 0f;
    private bool _canHit = true;
    private GameObject parentObject;
    private int myLayer, tempVisLayer, tempHiddenLayer;

    // Start is called before the first frame update
    void Start()
    {
        gTag = gameObject.tag;
        parentObject = this.gameObject;
        Damage = GameMaster.Instance.GetDamageAmount(GameMaster.Disposition.Neutral);
        tempVisLayer = LayerMask.NameToLayer("TempVisible");
        tempHiddenLayer = LayerMask.NameToLayer("TempHidden");
        myLayer = LayerMask.NameToLayer("Plants");
    }

    private void Update()
    {
        dmgWaitTimer += Time.deltaTime;

        if (dmgWaitTimer > damangeWaitTime && !_canHit)
        {
            _canHit = true;
            parentObject.layer = myLayer;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "DetectorCone")
        {
            Debug.Log("Bush is seeing player");
            GameMaster.Instance.AddtoVisible(gameObject, gTag);
        }
        if (collision.gameObject.name == "PlayerBody" && _canHit)
        {
            GameMaster.Instance.ReduceHealth(Damage);
            dmgWaitTimer = 0;
            _canHit = false;
            parentObject.layer = tempVisLayer;
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
