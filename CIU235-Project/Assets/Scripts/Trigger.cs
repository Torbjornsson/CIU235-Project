using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public bool activated;
    public Color color;

    private GameObject wt;

    // Start is called before the first frame update
    void Start()
    {
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
            //Vector3 other_p = other.gameObject.GetComponent<Transform>().position;
            //Vector3 this_p = gameObject.GetComponent<Transform>().position;
            //if (other_p.Equals(Utility.GetGridPos(this_p)) && 
            if (!other.gameObject.GetComponent<BoxPushedScript>().IsMoving() && 
                other.gameObject.GetComponent<MeshRenderer>().materials[0].color == color)
            {
                if (!activated)
                {
                    activated = true;
                    wt.SendMessage("TriggerActivated");
                }
            }
            else
            {
                if (activated)
                {
                    activated = false;
                    wt.SendMessage("TriggerActivated");
                }
            }
        }
    }

    //private void OnTriggerExit(Collider other) 
    //{
    //    activated = false;
    //    wt.SendMessage("TriggerActivated");
    //}
}
