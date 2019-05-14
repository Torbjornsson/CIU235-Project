using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    //public GameObject elevator;
    private CharacterControllerScript c_script;
    private GameMasterScript game_master_script;

    // Start is called before the first frame update
    void Start()
    {
        c_script = GameObject.Find("Character").GetComponent<CharacterControllerScript>();
        game_master_script = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if (!c_script.IsMoving()) c_script.elevator_trigger = false;
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Triggered... by " + other.gameObject.name);
        if (other.gameObject.name == "Character")
        {
            //CharacterControllerScript script = other.gameObject.GetComponent<CharacterControllerScript>();
            //Debug.Log("Prev elevator trigger: " + c_script.elevator_trigger);
            if (!c_script.IsMoving())
            {
                if (!c_script.elevator_trigger && c_script.elevator_trigger_pos != Utility.GetGridPos(c_script.rb.position))
                {
                    Debug.Log("Start elevators!!");
                    game_master_script.ChangeElevatorLevel();
                }
                c_script.elevator_trigger = true;
                c_script.elevator_trigger_pos = Utility.GetGridPos(c_script.rb.position);
            }
        }
    }
}
