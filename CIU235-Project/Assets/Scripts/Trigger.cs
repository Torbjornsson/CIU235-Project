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
            if (!other.gameObject.GetComponent<BoxPushedScript>().IsMoving() && 
                other.gameObject.GetComponent<MeshRenderer>().materials[0].color == color)
            {
                SetActivated(true);
            }
            else
            {
                SetActivated(false);
            }
        }
    }

    private void SetActivated(bool activated)
    {
        if (activated != this.activated)
        {
            this.activated = activated;
            wt.SendMessage("TriggerActivated");
        }
    }
}
