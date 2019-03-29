using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private Material material;

    private GameObject wc;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
        wc = GameObject.Find("WinCon");
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
                wc.GetComponent<WinTrigger>().activated = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        wc.GetComponent<WinTrigger>().activated = false;
    }
}
