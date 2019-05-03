using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameMasterScript game_master_script;
    // Start is called before the first frame update
    void Start()
    {
        game_master_script = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        game_master_script.Resume();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        game_master_script.ResetLevel();
        
    }

    public void QuitToMenu(){
        game_master_script.LoadLevel(0);
    }
}
