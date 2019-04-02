using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public bool activated;
    private GameMasterScript gameMasterScript;

    private ArrayList triggers;

    public GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        gameMasterScript = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        //door = GameObject.Find("Door");
        
        triggers = new ArrayList(GameObject.FindGameObjectsWithTag("Trigger"));
        door.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        //Change to som animation trigger and so on
        if (activated)
        {
            door.SetActive(false);
        }
        else
        {
            door.SetActive(true);
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
