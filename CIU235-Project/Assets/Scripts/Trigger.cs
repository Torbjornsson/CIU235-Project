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

    // Called as long as there is collission going on (with focus on boxes)
    void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.tag == "Box")
        {
            BoxPushedScript other_script = other.gameObject.GetComponent<BoxPushedScript>();
            if (!other_script.IsMoving())
            {
                if (other.gameObject.GetComponent<MeshRenderer>().materials[0].color == color)
                {
                    SetActivated(true);
                    other_script.SetState(BoxPushedScript.State.CORRECT);
                }
                else
                {
                    other_script.SetState(BoxPushedScript.State.WRONG);
                }
            }
            else
            {
                SetActivated(false);
                other_script.SetState(BoxPushedScript.State.IDLE);
            }
        }
    }

    // Helper method that makes sure everything is activated or deactivated only when it needs to
    private void SetActivated(bool activated)
    {
        if (activated != this.activated)
        {
            this.activated = activated;
            wt.SendMessage("TriggerActivated");
        }
    }
}
