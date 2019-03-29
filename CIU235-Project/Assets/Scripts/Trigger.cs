using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private Material material;

    private GameObject wt;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
        wt = GameObject.Find("WinCon");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.tag == "Box")
        {
            if (other.gameObject.GetComponent<MeshRenderer>().materials[0].color == material.color)
            {
                wt.GetComponent<WinTrigger>().activated = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        wt.GetComponent<WinTrigger>().activated = false;
    }
}
