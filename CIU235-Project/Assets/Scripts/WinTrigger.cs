using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public bool activated;

    private GameMasterScript game_master_script;

    private ArrayList triggers;

    //public GameObject win_light;
    public GameObject win_pad;
    public GameObject win_point_light;

    // Start is called before the first frame update
    void Start()
    {
        game_master_script = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        triggers = new ArrayList(GameObject.FindGameObjectsWithTag("Trigger"));
        TriggerActivated();
    }

    // Update is called once per frame
    void Update()
    {
        //Change to som animation trigger and so on
        if (activated)
        {
            //win_light.SetActive(true);
            //win_point_light.SetActive(true);
            win_point_light.GetComponent<Light>().intensity = 1.5f;
            win_pad.GetComponent<MeshRenderer>().materials[0].EnableKeyword("_EMISSION");
        }
        else
        {
            //win_light.SetActive(false);
            //win_point_light.SetActive(false);
            win_point_light.GetComponent<Light>().intensity = 0.5f;
            win_pad.GetComponent<MeshRenderer>().materials[0].DisableKeyword("_EMISSION");
        }
    }

    // Checks for wins (that is, WinTrigger is active and player is present)
    void OnTriggerStay(Collider other) {
        if (other.gameObject.name == "Character" && activated &&
            !other.gameObject.GetComponent<CharacterControllerScript>().IsMoving())
        {
            game_master_script.SendMessage("LevelWin");
            //activated = false;
        }
    }

    // Called whenever a Trigger object is activated
    // Checks if ALL Triggers are active, and in that case activates itself
    public void TriggerActivated()
    {
        activated = true;
        foreach (GameObject item in triggers)
        {
            if (!item.gameObject.GetComponent<Trigger>().activated)
                activated = false;
        }
    }
}
