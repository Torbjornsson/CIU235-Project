using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wintrigger : MonoBehaviour
{
    public bool activated;
    private GameMasterScript gameMasterScript;
    // Start is called before the first frame update
    void Start()
    {
        gameMasterScript = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Character" && activated){
            gameMasterScript.LevelWin();
        }
    }
}
