using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public bool activated;
    private GameMasterScript gameMasterScript;

    private GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        gameMasterScript = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        door = GameObject.Find("Door");
    }

    // Update is called once per frame
    void Update()
    {
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
            gameMasterScript.LevelWin();
        }
    }
}
