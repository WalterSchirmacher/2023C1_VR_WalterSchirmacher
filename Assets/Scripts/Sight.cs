using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public float distance = 3f;
    public float angle = 60f;
    public LayerMask objectsLayers;
    public LayerMask obstancesLayers;
    public AudioSource audioSource;
  //  public List<Collider> detectedObjects = new List<Collider>();

    [SerializeField] private List<FriendOrFoe> currentlyVisible;
    [SerializeField] private List<FriendOrFoe> alreadyPartialTransparent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        GetAllVisibleObjects();

        MakeObjectsVisible();
       // MakeObjectsDefault();
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

        Debug.Log(string.Join(",", currentlyVisible));
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

    private void MakeObjectsVisible()
    {
        for (int i = 0; i < currentlyVisible.Count; i++)
        {
            FriendOrFoe friendOrFoe = currentlyVisible[i];
            if (!alreadyPartialTransparent.Contains(friendOrFoe))
            {
                friendOrFoe.MakePartlyTransparent();
                alreadyPartialTransparent.Add(friendOrFoe);
                Debug.Log("Making Visible " + friendOrFoe.name);
                audioSource.Play();
                Debug.Log(string.Join(",", alreadyPartialTransparent));
            }
        }
    }

    private void MakeObjectsDefault()
    {
        for (int i = alreadyPartialTransparent.Count - 1; i >= 0; i--)
        {
            FriendOrFoe friendOrFoe = alreadyPartialTransparent[i];
            if (!currentlyVisible.Contains(friendOrFoe))
            {
                friendOrFoe.MakeDefault();
                alreadyPartialTransparent.Remove(friendOrFoe);
                Debug.Log("Making default " + friendOrFoe.name);
                audioSource.Stop();
                Debug.Log(string.Join(",", alreadyPartialTransparent));
            }
        }
    }

     private void OnDrawGizmos()
     {
         Gizmos.color = Color.red;
      //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
      Gizmos.DrawWireSphere(transform.position, distance);
     } 
}
