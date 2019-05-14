using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    //public GameObject elevator;
    //private CharacterControllerScript c_script;
    //private GameMasterScript game_master_script;

    private bool character_stay;

    // Start is called before the first frame update
    void Start()
    {
        //c_script = GameObject.Find("Character").GetComponent<CharacterControllerScript>();
        //game_master_script = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();

        character_stay = false;
    }

    // Update is called once per frame
    //void LateUpdate()
    //{
        //if (!c_script.IsMoving()) c_script.elevator_trigger = false;
    //}

    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Triggered... by " + other.gameObject.name);
        //if (other.gameObject.name == "Character")
        //{
        //CharacterControllerScript script = other.gameObject.GetComponent<CharacterControllerScript>();
        //Debug.Log("Prev elevator trigger: " + c_script.elevator_trigger);
        //if (!c_script.IsMoving())
        //{
        //    Debug.Log("Character on elevator - el trigger? "+ c_script.elevator_trigger+", el trigger pos: "+ c_script.elevator_trigger_pos+", char pos: "+ Utility.GetGridPos(c_script.rb.position));
        //    if (!c_script.elevator_trigger && c_script.elevator_trigger_pos != Utility.GetGridPos(c_script.rb.position))
        //    {
        //        Debug.Log("Start elevators!!");
        //        game_master_script.ChangeElevatorLevel();
        //    }
        //    c_script.elevator_trigger = true;
        //    c_script.elevator_trigger_pos = Utility.GetGridPos(c_script.rb.position);
        //}
        //}

        character_stay = (other.gameObject.name == "Character");

        //if (character_stay) game_master_script.CheckElevatorTriggering();
    }

    void OnTriggerExit(Collider other)
    {
        character_stay = false;
        //game_master_script.CheckElevatorTriggering();
    }

    public bool CharacterOnElevator()
    {
        return character_stay;
    }
}
