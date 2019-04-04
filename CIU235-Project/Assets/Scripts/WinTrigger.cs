using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public bool activated;
    private GameMasterScript gameMasterScript;

    private GameObject door;
    private GameObject winLight;
    private GameObject winPad;
    // Start is called before the first frame update
    void Start()
    {
        gameMasterScript = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        door = GameObject.Find("Door");
        winLight = GameObject.Find("WinLight");
        winPad = GameObject.Find("WinPad");
    }

    // Update is called once per frame
    void Update()
    {
        //Change to som animation trigger and so on
        if (activated)
        {
            door.SetActive(false);
            winLight.SetActive(true);
            winPad.GetComponent<MeshRenderer>().materials[0].EnableKeyword("_EMISSION");
        }
        else
        {
            door.SetActive(true);
            winLight.SetActive(false);
            winPad.GetComponent<MeshRenderer>().materials[0].DisableKeyword("_EMISSION");
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Character" && activated){
            gameMasterScript.LevelWin();
        }
    }
}
