using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{

    public Vector3 lastPos;
    public Vector3 currentPos;
    public float moveLength, moveSpeed;
    private bool localIsMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        currentPos = transform.position;
        lastPos = currentPos;
        localIsMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        lastPos = currentPos;
        currentPos = transform.position;

        moveLength = Vector3.Distance(lastPos, currentPos);
        moveSpeed = moveLength / Time.deltaTime;
        bool currentlyMoving = Mathf.Approximately(moveSpeed, 0f);

        // Was Player moving last time?
        if (localIsMoving)
        {
            // If was moving last time, are they still moving?
            if(currentlyMoving == false)
            {
                //Not moving now, so stop moving and healing
                GameMaster.Instance.IsMoving = false;
                localIsMoving = false;
            }
        }
        else
        {
            // If wasn't moving previously, but currently are moving...
            if (currentlyMoving)
            {
                GameMaster.Instance.IsMoving = true;
                localIsMoving = true;
            }
        }
     
    }
}
