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

    void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.name == "Character" && activated){
            gameMasterScript.SendMessage("LevelWin");
        }
    }

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
