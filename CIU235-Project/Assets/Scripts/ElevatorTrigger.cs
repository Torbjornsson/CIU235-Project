using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    private bool character_stay;

    // Start is called before the first frame update
    void Start()
    {
        character_stay = false;
    }

    void OnTriggerStay(Collider other)
    {
        character_stay = (other.gameObject.name == "Character");
    }

    void OnTriggerExit(Collider other)
    {
        character_stay = false;
    }

    public bool CharacterOnElevator()
    {
        return character_stay;
    }
}
