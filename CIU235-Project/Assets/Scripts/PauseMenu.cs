using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameMasterScript game_master_script;

    public GameObject fade_obj;
    private Fade fade_script;
    private bool quit_to_menu;

    // Start is called before the first frame update
    void Start()
    {
        game_master_script = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();

        fade_obj = game_master_script.fade_obj;
        fade_script = fade_obj.GetComponent<Fade>();
        fade_obj.SetActive(false);
        quit_to_menu = false;
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
        Time.timeScale = 1;
        //gameObject.SetActive(false);
        //game_master_script.LoadLevel(0);
        fade_obj.SetActive(true);
    }
}
