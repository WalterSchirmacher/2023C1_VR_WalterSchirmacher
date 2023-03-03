using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FriendOrFoe : MonoBehaviour
{

    public enum TransStatus { Default, Partial, TempHidden, TempVisible };
    [Tooltip("Friendly won't attack, Neutral is wary, Hostile will attack, ExtremeHatred attacks with no warning.")]
    public GameMaster.Disposition myStatus = GameMaster.Disposition.Neutral;
    public GameObject parentObject, visibleObject, transparentObject, lightObject;
    public NewAIFSM ai;
    public AudioSource audioRegular, audioMad;
    public float volRegular = 0.5f, volMad = 0.75f;
    public string animationIdle, animationStalk, animationChase;
    public List<string> animationAttack;

    [Tooltip("Time in seconds to wait between hits on Collision.")]
    public float damangeWaitTime = 1;
    private float dmgWaitTimer = 0f;
    public float Damage { get; set; } = 0f;
    private bool _canHit = true;

    [HideInInspector]
    public TransStatus currentTransState;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Vector3 myHomeLocation;

    [Tooltip("Default sets the object to the starting visibility, Partial shows with transparency, TempHidden hides it, TempVisible makes it appear.")]
    public TransStatus defaultTransState = TransStatus.Default;
    public Collider detectedObject;

    private int myLayer, tempVisLayer, tempHiddenLayer;
    public string gTag;
    private float randomLower = 3f;
    private float randomUpper = 10f;
    private bool stopRegularAudio = false;
    private bool stopMadAudio = false;
    private int fadeAudioTime = 10;

    [HideInInspector]
    public bool isChasing = false;

    private void Awake()
    {
        myHomeLocation = gameObject.transform.position;
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("My status is " + myStatus);
        Damage = GameMaster.Instance.GetDamageAmount(myStatus);

        if (!audioRegular || !audioMad)
        {
            Debug.Log("Invalid Audio Source!");
            Debug.Log("AudioSource: " + audioRegular);
            Debug.Log("Short Roar: " + audioMad);
        }
        else
        {
            audioRegular.volume = volRegular;
            audioMad.volume = volMad;
        }

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
            GameMaster.Instance.ReduceHealth(Damage);

            // Change myStatus to the next level of Disposition
            myStatus = GameMaster.Instance.ChangeDisposition(myStatus, true);
            dmgWaitTimer = 0;
            _canHit = false;
        }
    }


    [ContextMenu("Make Default")]
    public void MakeDefault()
    {
   //     Debug.Log("Set to Default");

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
        if (lightObject)
        {
            lightObject.SetActive(false);
        }
    }

    [ContextMenu("Partly Transparent")]
    public void MakePartlyTransparent()
    {
     //   Debug.Log("Make Partly Transparent");

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
        if(lightObject)
        {
            lightObject.SetActive(true);
        }
    }

    [ContextMenu("Temporarily Hides Object")]
    public void MakeTempHidden()
    {
  //      Debug.Log("Make Hidden Temporarily");

        currentTransState = TransStatus.TempHidden;
        transparentObject.SetActive(false);
        visibleObject.SetActive(true);
        CheckAndSetLayer(tempHiddenLayer);
        if (lightObject)
        {
            lightObject.SetActive(false);
        }
    }

    [ContextMenu("Temporarily Makes Visible")]
    public void MakeTempVisible()
    {
    //    Debug.Log("Make Hidden Temporarily");

        currentTransState = TransStatus.TempVisible;
        transparentObject.SetActive(false);
        visibleObject.SetActive(true);
        CheckAndSetLayer(tempVisLayer);
        if (lightObject)
        {
            lightObject.SetActive(true);
        }
    }

    [ContextMenu("Reset Object to Defaults")]
    public void ResetObject()
    {
     //   Debug.Log("Resetting Object");

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

    public void ChasePlayer()
    {
    //    Debug.Log("Chasing Player");
        isChasing = true;
        myStatus = GameMaster.Disposition.ExtremeHatred;
        Damage = GameMaster.Instance.GetDamageAmount(myStatus);
        PauseRegularAudio();
        StartCoroutine(MadRoarAudio());
        MakePartlyTransparent();
    }

    public void PlayerLost()
    {
        stopRegularAudio = true;
        stopMadAudio = true;
        MakeDefault();
    }

    public void PlayerFound()
    {
        stopRegularAudio = false;
        stopMadAudio = false;
        StartCoroutine(RegularRoarAudio());
        Debug.Log("Roar!");
        
    }

    public void PlayAttackAnimation(string ani)
    {
        ResetAnimates();
        animator.SetTrigger(ani);
    }

    public void PlayNewAnimation(string ani)
    {
        ResetAnimates();
        animator.SetBool(ani, true);
    }

    public void ResetAnimates()
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            animator.SetBool(parameter.name, false);
        }
    }

    IEnumerator RegularRoarAudio()
    {
    //    Debug.Log("Playing regular");

        while (!stopRegularAudio)
        {
            audioRegular.Play();
            yield return new WaitForSeconds(audioRegular.clip.length + Random.Range(randomLower, randomUpper));
        }

        audioRegular.FadeOut(fadeAudioTime);
    }

    IEnumerator MadRoarAudio()
    {
    //    Debug.Log("Playing mad");

        while (!stopMadAudio)
        {
            audioMad.Play();
            yield return new WaitForSeconds(audioMad.clip.length + (Random.Range(randomLower, randomUpper)) / 2);
        }

        audioMad.FadeOut(fadeAudioTime);
    }

    IEnumerable PauseRegularAudio()
    {
        stopRegularAudio = true;
        yield return new WaitForSeconds(fadeAudioTime + 1);
        stopRegularAudio = false;
        StartCoroutine(RegularRoarAudio());
    }
}
