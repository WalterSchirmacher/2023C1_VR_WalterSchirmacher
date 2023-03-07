using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightDetector : MonoBehaviour
{

    public int animals, rocks, plants, trees, bushes, other;
    // Start is called before the first frame update
    void Start()
    {
        animals = rocks = plants = trees = bushes = other = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
    //   AddRadar(other.gameObject.tag);
    }
    private void OnTriggerStay(Collider other)
    {
       // string gTag = other.gameObject.tag;
        //   Debug.Log("I still see a: " + gTag + ", and it is a: " + other.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
      //  string gTag = other.gameObject.tag;
        //  SubtractRadar(other.gameObject.tag);
    }

    void AddRadar(string gTag)
    {
        switch (gTag)
        {
            case "Rock":
                rocks++;
                break;
            case "Tree":
                trees++;
                break;
            case "Bush":
                bushes++;
                break;
            case "Animal":
                animals++;
                break;
            case "Mushroom":
                other++;
                break;
            case "MonsterPlant":
                other++;
                break;
            default:
                Debug.Log("Item Tag Not Found");
                break;
        }
    }

    void SubtractRadar(string gTag)
    {
        switch (gTag)
        {
            case "Rock":
                rocks--;
                break;
            case "Tree":
                trees--;
                break;
            case "Bush":
                bushes--;
                break;
            case "Animal":
                animals--;
                break;
            case "Mushroom":
                other--;
                break;
            case "MonsterPlant":
                other--;
                break;
            default:
                Debug.Log("Item Tag Not Found");
                break;
        }
    }

}
