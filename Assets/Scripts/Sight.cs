using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public float distance;
    public float angle;
    public LayerMask objectsLayers;
    public LayerMask obstancesLayers;
  //  public List<Collider> detectedObjects = new List<Collider>();

    [SerializeField] private List<FriendOrFoe> currentlyVisible;
    [SerializeField] private List<FriendOrFoe> alreadyTransparent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        GetAllVisibleObjects();

        MakeObjectsSolid();
        MakeObjectsTransparent();

        /*  Collider[] colliders = Physics.OverlapSphere(transform.position, distance, (int)objectsLayers);

          detectedObjects.Clear();

          for (int i = 0; i < colliders.Length; i++)
          {
              Collider collider = colliders[i];

              Vector3 directionToController = Vector3.Normalize(collider.bounds.center - transform.position);

              float angleToCollider = Vector3.Angle(transform.forward, directionToController);

              if (angleToCollider < angle)
              {
                  if(!Physics.Linecast(transform.position, collider.bounds.center, (int)obstancesLayers))
                  {
                      detectedObjects.Add(collider);
                      break;
                  }
              }
          }
          if(detectedObjects.Any())
          {
              Debug.Log("I see the following:");
              detectedObjects.ForEach(delegate (string name))
              {
                  Debug.Log(name);
              }

          } else
          {
              Debug.Log("Nothing in sight!");
          } */

    }

    private void GetAllVisibleObjects()
    {
        currentlyVisible.Clear();

        Collider[] colliders = Physics.OverlapSphere(transform.position, distance, (int)objectsLayers);

        currentlyVisible.Clear();

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider collider = colliders[i];

            Vector3 directionToController = Vector3.Normalize(collider.bounds.center - transform.position);

            float angleToCollider = Vector3.Angle(transform.forward, directionToController);

            if (angleToCollider < angle)
            {
                if (!Physics.Linecast(transform.position, collider.bounds.center, (int)obstancesLayers))
                {
                  //  detectedObjects.Add(collider);

                    if (collider.gameObject.TryGetComponent(out FriendOrFoe friendOrFoe))
                    {
                        if (!currentlyVisible.Contains(friendOrFoe))
                        {
                            currentlyVisible.Add(friendOrFoe);
                            Debug.Log("I see the following:" + friendOrFoe.name);
                        }
                    }

                    break;
                }
            }
        }
       /* if (detectedObjects.Any())
        {
            Debug.Log("I see the following:");
            detectedObjects.ForEach(delegate (string name))
            {
                Debug.Log(name);
            }

        }
        else
        {
            Debug.Log("Nothing in sight!");
        } */
    }

    private void MakeObjectsTransparent()
    {
        for (int i = 0; i < currentlyVisible.Count; i++)
        {
            FriendOrFoe friendOrFoe = currentlyVisible[i];
            if (!alreadyTransparent.Contains(friendOrFoe))
            {
                friendOrFoe.MakeTransparent();
                alreadyTransparent.Add(friendOrFoe);
            }
        }
    }

    private void MakeObjectsSolid()
    {
        for (int i = alreadyTransparent.Count - 1; i >= 0; i--)
        {
            FriendOrFoe friendOrFoe = alreadyTransparent[i];
            if (!currentlyVisible.Contains(friendOrFoe))
            {
                friendOrFoe.MakeSolid();
                alreadyTransparent.Remove(friendOrFoe);
            }
        }
    }

    /* private void //OnDrawGizmos()
     {
         Gizmos.color = Color.red;
      //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
      Gizmos.DrawWireSphere(transform.position, distance);
     } */
}
