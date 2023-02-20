using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTransparent : MonoBehaviour
{

    [SerializeField] private List<InTheWay> currentlyInTheWay;
    [SerializeField] private List<InTheWay> alreadyTransparent;
    [SerializeField] private Transform player;
    private new readonly Transform camera;

    private void Awake()
    {
        currentlyInTheWay = new List<InTheWay>();
        alreadyTransparent = new List<InTheWay>();

    }

    // Update is called once per frame
    void Update()
    {
        GetAllObjectsInTheWay();

        MakeObjectsSolid();
        MakeObjectsTransparent();
    }

    private void GetAllObjectsInTheWay()
    {
        currentlyInTheWay.Clear();

        float cameraPlayerDistance = Vector3.Magnitude(camera.position - player.position);
        Ray ray1_Forward = new Ray(camera.position, player.position - camera.position);
        Ray ray1_Backward = new Ray(player.position, camera.position - player.position);

        var hits_Forward = Physics.RaycastAll(ray1_Forward, cameraPlayerDistance);
        var hits_Backward = Physics.RaycastAll(ray1_Backward, cameraPlayerDistance);

        foreach (var hit in hits_Forward)
        {
            if(hit.collider.gameObject.TryGetComponent(out InTheWay inTheWay))
            {
                if(!currentlyInTheWay.Contains(inTheWay))
                {
                    currentlyInTheWay.Add(inTheWay);
                }
            }
        }

        foreach (var hit in hits_Backward)
        {
            if (hit.collider.gameObject.TryGetComponent(out InTheWay inTheWay))
            {
                if (!currentlyInTheWay.Contains(inTheWay))
                {
                    currentlyInTheWay.Add(inTheWay);
                }
            }
        }


    }

    private void MakeObjectsTransparent()
    {
        for (int i = 0; i < currentlyInTheWay.Count; i++)
        {
            InTheWay inTheWay = currentlyInTheWay[i];
            if(!alreadyTransparent.Contains(inTheWay))
            {
                inTheWay.ShowTransparent();
                alreadyTransparent.Add(inTheWay);
            }
        }
    }

    private void MakeObjectsSolid()
    {
        for (int i = alreadyTransparent.Count-1; i>=0; i--)
        {
            InTheWay wasInTheWay = alreadyTransparent[i];
            if (!currentlyInTheWay.Contains(wasInTheWay))
            {
                wasInTheWay.ShowSolid();
                alreadyTransparent.Remove(wasInTheWay);
            }
        }
    }
}
