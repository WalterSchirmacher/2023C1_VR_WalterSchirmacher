using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FriendOrFoe : MonoBehaviour
{

    public enum TransStatus { Default, Partial, TempHidden, TempVisible };
    public GameMaster gameMaster;
    [Tooltip("Friendly won't attack, Neutral is wary, Hostile will attack, ExtremeHatred attacks with no warning.")]
    public GameMaster.Disposition myStatus = GameMaster.Disposition.Neutral;
    public GameObject parentObject;
    public GameObject visibleObject;
    public GameObject transparentObject;
    public GameObject innerSphere;
    public GameObject outerSphere;
    public Vector3 myHomeLocation;
    public AIFSM aiFSM;

    public string animationIdle, animationStalk, animationChase;
    [Tooltip("To list multiple attack animations, seperate them by a pipe (|).")]
    public List<string> animationAttack;


    [Tooltip("Time in seconds to wait between hits on Collision.")]
    public float damangeWaitTime = 1;
    private float dmgWaitTimer = 0f;
    public float Damage { get; set; } = 0f;
    private bool _canHit = true;
    public bool OuterPlayerFound { get; set; } = false;
    public bool InnerPlayerFound { get; set; } = false;


    [HideInInspector]
    public TransStatus currentTransState;
    [HideInInspector]
    public Animator animator;

    [Tooltip("Default sets the object to the starting visibility, Partial shows with transparency, TempHidden hides it, TempVisible makes it appear.")]
    public TransStatus defaultTransState = TransStatus.Default;

    private int myLayer;
    private int tempVisLayer;
    private int tempHiddenLayer;
    private string gTag;

    private void Awake()
    {
        myHomeLocation = gameObject.transform.position;
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
    /*    Debug.Log("attack " + animationAttack);
        if (animationAttack.Contains("|"))
        {
            aniAttacks = animationAttack.Split('|').ToList();
            useAniList = true;
            Debug.Log(aniAttacks.ToString());
        }
    */

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

    private void Update()
    {
        dmgWaitTimer += Time.deltaTime;

        if(dmgWaitTimer > damangeWaitTime && !_canHit)
        {
            _canHit = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(gameObject.name + " Hit a " + collision.gameObject.name + " " + collision.gameObject.tag);
        if (collision.gameObject.name == "PlayerBody" && _canHit)
        {
            gameMaster.ReduceHealth(Damage);

            // Change myStatus to the next level of Disposition
            myStatus = gameMaster.ChangeDisposition(myStatus, true);
            dmgWaitTimer = 0;
            _canHit = false;
        }
    }


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
