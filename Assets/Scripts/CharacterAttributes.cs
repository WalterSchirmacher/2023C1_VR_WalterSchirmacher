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
        bool currentlyNotMoving = Mathf.Approximately(moveSpeed, 0f);

      //  Debug.Log("Currently moving " + currentlyMoving + " moveSpeed " + moveSpeed);

      
        // Was Player moving last time?
        if (localIsMoving)
        {
         //   Debug.Log("I was moving last time " + GameMaster.Instance.IsMoving);
            // If was moving last time, are they still moving?
            if(currentlyNotMoving == true)
            {
                
                //Not moving now, so stop moving and healing
                GameMaster.Instance.IsMoving = false;
                localIsMoving = false;
          //      Debug.Log("But I am not moving now " + GameMaster.Instance.IsMoving);
            }
        //    Debug.Log("and I am still moving " + GameMaster.Instance.IsMoving);
        }
        else
        {
        //    Debug.Log("I was NOT moving last time " + GameMaster.Instance.IsMoving);
            // If wasn't moving previously, but currently are moving...
            if (currentlyNotMoving == false)
            {
               
                GameMaster.Instance.IsMoving = true;
                localIsMoving = true;
          //      Debug.Log("But now I am moving " + GameMaster.Instance.IsMoving);
            }
        }
    
    }
}
