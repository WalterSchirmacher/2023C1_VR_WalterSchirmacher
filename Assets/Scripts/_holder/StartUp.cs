using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    public GameObject TreeClone;

    private GameObject[] trees;

    // Start is called before the first frame update
    void Start()
    {
        TreeClone.SetActive(true);
        trees = GameObject.FindGameObjectsWithTag("Tree");
        foreach (GameObject tree in trees)
        {
            if(tree.name.Contains("tree02"))
            {
                GameObject newTree = GameObject.Instantiate(TreeClone, new Vector3(tree.transform.position.x, TreeClone.transform.position.y, tree.transform.position.z), Quaternion.identity);
            }
        }
        TreeClone.SetActive(false);
    }

}
