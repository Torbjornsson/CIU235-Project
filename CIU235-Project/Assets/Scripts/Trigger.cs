﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public bool activated;
    public Color color;

    //private Material material;

    private GameObject wt;

    // Start is called before the first frame update
    void Start()
    {
        //material = GetComponent<MeshRenderer>().materials[0];
        wt = GameObject.Find("WinCon");
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.tag == "Box")
        {
            if (other.gameObject.GetComponent<MeshRenderer>().materials[0].color == color)
            {
                activated = true;
                wt.SendMessage("TriggerActivated");
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        activated = false;
        wt.SendMessage("TriggerActivated");
    }
}
