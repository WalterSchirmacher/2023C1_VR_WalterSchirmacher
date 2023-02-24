using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendOrFoe : MonoBehaviour
{
    public enum MyStatus { Friendly, Neutral, Hostile, ExtremeHatred };
    public enum TransStatus { Default, Partial, TempHidden, TempVisible };

    [Tooltip("Friendly won't attack, Neutral is wary, Hostile will attack, ExtremeHatred attacks with no warning.")]
    public MyStatus myStatus = MyStatus.Neutral;
    public GameObject parentObject;
    public GameObject visibleObject;
    public GameObject transparentObject;
    public GameObject innerSphere;
    public GameObject outerSphere;
    public bool OuterPlayerFound { get; set; } = false;
    public bool InnerPlayerFound { get; set; } = false;

[HideInInspector]
    public TransStatus currentTransState;
    [Tooltip("Default sets the object to the starting visibility, Partial shows with transparency, TempHidden hides it, TempVisible makes it appear.")]
    public TransStatus defaultTransState = TransStatus.Default;

    private int myLayer;
    private int tempVisLayer;
    private int tempHiddenLayer;
    private string gTag;

    // Start is called before the first frame update
    void Start()
    {
        gTag = gameObject.tag;
        string gLayerName;

        switch (gTag)
        {
            case "Rock":
                gLayerName = "Rocks";
                break;
            case "Tree":
                gLayerName = "Plants";
                break;
            case "Bush":
                gLayerName = "Plants";
                break;
            case "Animal":
                gLayerName = "Animals";
                break;
            case "Mushroom":
                gLayerName = "Plants";
                break;
            case "MonsterPlant":
                gLayerName = "MonsterPlants";
                break;
            default:
                gLayerName = "Default";
                Debug.Log("Item Tag Not Found");
                break;
        }

        tempVisLayer = LayerMask.NameToLayer("TempVisible");
        tempHiddenLayer = LayerMask.NameToLayer("TempHidden");
        myLayer = LayerMask.NameToLayer(gLayerName);

        if (defaultTransState == TransStatus.Default)
        {
            MakeDefault();
        }
        else if (defaultTransState == TransStatus.Partial)
        {
            MakePartlyTransparent();
        }
        else if (defaultTransState == TransStatus.TempHidden)
        {
            MakeTempHidden();
        } 
        else if (defaultTransState == TransStatus.TempVisible)
        {
            MakeTempVisible();
        }
    }

  /*  private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I detect a " + other.gameObject.name);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("I still detect a " + other.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("I no longer detect a " + other.gameObject.name);
    }
  */


    [ContextMenu("Make Default")]
    public void MakeDefault()
    {
        Debug.Log("Set to Default");

        if(defaultTransState == TransStatus.Partial)
        {
            MakePartlyTransparent();
        }
        else
        {
            currentTransState = TransStatus.Default;
            transparentObject.SetActive(false);
            visibleObject.SetActive(true);
            CheckAndSetLayer(myLayer);
        }
    }

    [ContextMenu("Partly Transparent")]
    public void MakePartlyTransparent()
    {
        Debug.Log("Make Partly Transparent");

        currentTransState = TransStatus.Partial;
        visibleObject.SetActive(false);
        transparentObject.SetActive(true);

        if (gTag == "Plant" || gTag == "Rock")
        {
            CheckAndSetLayer(tempVisLayer);
        } else
        {
            CheckAndSetLayer(myLayer);
        }
    }

    [ContextMenu("Temporarily Hides Object")]
    public void MakeTempHidden()
    {
        Debug.Log("Make Hidden Temporarily");

        currentTransState = TransStatus.TempHidden;
        transparentObject.SetActive(false);
        visibleObject.SetActive(true);
        CheckAndSetLayer(tempHiddenLayer);
    }

    [ContextMenu("Temporarily Makes Visible")]
    public void MakeTempVisible()
    {
        Debug.Log("Make Hidden Temporarily");

        currentTransState = TransStatus.TempVisible;
        transparentObject.SetActive(false);
        visibleObject.SetActive(true);
        CheckAndSetLayer(tempVisLayer);
    }

    [ContextMenu("Reset Object to Defaults")]
    public void ResetObject()
    {
        Debug.Log("Resetting Object");

        currentTransState = TransStatus.Default;
        transparentObject.SetActive(false);
        visibleObject.SetActive(true);
        CheckAndSetLayer(myLayer);
    }

    // Check if layer is correct layer for item
    private void CheckAndSetLayer(int theLayer)
    {
     //   Debug.Log("Setting layer to: " + theLayer);
        parentObject.layer = theLayer;
        SetLayerAllChildren(parentObject, theLayer);
    }

    private void SetLayerAllChildren(GameObject _go, int _layer)
    {
        _go.layer = _layer;
        foreach (Transform child in _go.transform)
        {
            child.gameObject.layer = _layer;

            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetLayerAllChildren(child.gameObject, _layer);

        }
    }

}
