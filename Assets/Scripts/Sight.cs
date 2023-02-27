using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    public float distance = 3f;
    public float angle = 60f;
    public LayerMask objectsLayers;
    public LayerMask obstancesLayers;
    public Collider detectedObject;

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, distance, (int)objectsLayers);

        detectedObject = null;

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider collider = colliders[i];

            Vector3 directionToController = Vector3.Normalize(collider.bounds.center - transform.position);

            float angleToCollider = Vector3.Angle(transform.forward, directionToController);

            if (angleToCollider < angle)
            {
                if (!Physics.Linecast(transform.position, collider.bounds.center, (int)obstancesLayers))
                {
                    detectedObject = collider;
                    Debug.Log("Found " + collider.gameObject.name);
                    break;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, angle);
    }
}
