using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public bool activated;

    private GameMasterScript gameMasterScript;

    private ArrayList triggers;

    public GameObject winLight;
    public GameObject winPad;

    // Start is called before the first frame update
    void Start()
    {
        gameMasterScript = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        triggers = new ArrayList(GameObject.FindGameObjectsWithTag("Trigger"));
        TriggerActivated();
    }

    // Update is called once per frame
    void Update()
    {
        //Change to som animation trigger and so on
        if (activated)
        {
            winLight.SetActive(true);
            winPad.GetComponent<MeshRenderer>().materials[0].EnableKeyword("_EMISSION");
        }
        else
        {
            winLight.SetActive(false);
            winPad.GetComponent<MeshRenderer>().materials[0].DisableKeyword("_EMISSION");
        }
    }

    // Checks for wins (that is, WinTrigger is active and player is present)
    void OnTriggerStay(Collider other) {
        if (other.gameObject.name == "Character" && activated &&
            !other.gameObject.GetComponent<CharacterControllerScript>().IsMoving())
        {
            gameMasterScript.SendMessage("LevelWin");
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
