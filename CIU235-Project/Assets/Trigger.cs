using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
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
                GameObject wc = GameObject.Find("WinCon");
                wc.GetComponent<wintrigger>().activated = true;
            }
        }
    }
}
