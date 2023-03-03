using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{

    public Vector3 lastPos;
    public Vector3 currentPos;
    public float moveLength, moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        currentPos = transform.position;
        lastPos = currentPos;
    }

    // Update is called once per frame
    void Update()
    {
       /* lastPos = currentPos;
        currentPos = transform.position;

        moveLength = Vector3.Distance(lastPos, currentPos);
        moveSpeed = moveLength / Time.deltaTime;
        bool currentlyMoving = Mathf.Approximately(moveSpeed, 0f);

        // Was Player moving last time?
        if (GameMaster.Instance.IsMoving)
        {
            // If was moving last time, are they still moving?
            if(currentlyMoving == false)
            {
                //Not moving now, so stop moving and healing
                GameMaster.Instance.IsMoving = false;
            }
        }
        else
        {
            // If wasn't moving previously, but currently are moving...
            if (currentlyMoving)
            {
                GameMaster.Instance.IsMoving = true;
            }
        }
       */
    }
}
