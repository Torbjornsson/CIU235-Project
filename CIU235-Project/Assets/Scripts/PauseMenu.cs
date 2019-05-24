using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameMasterScript game_master_script;

    public GameObject achievement;
    private AchievementSystem achievement_script;
    public GameObject fade_obj;
    private Fade fade_script;
    private bool quit_to_menu;
    public int steps, best_steps;

    // Start is called before the first frame update
    void Start()
    {
        game_master_script = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();

        achievement_script = achievement.GetComponent<AchievementSystem>();
        CheckSteps();

        fade_obj = game_master_script.fade_obj;
        fade_script = fade_obj.GetComponent<Fade>();
        fade_obj.SetActive(false);
        quit_to_menu = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() {
        CheckSteps();
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
        gameObject.SetActive(false);
        game_master_script.ExitToMenu();
    }

    public void CheckSteps(){
        
        string s_best, s_curr;

        best_steps = 0;
        steps = 0;
        if(game_master_script != null)
        {
            best_steps = achievement_script.GetSteps(game_master_script.b_index);
            steps = game_master_script.undo_stack.Count;

        }
        
        s_best = "Previous Best: ";
        s_curr = "Steps: ";

        if (best_steps == 0)
        {
            s_best += "NA";
        }
        else
        {
            s_best += best_steps.ToString();
        }
        s_curr += steps;

        GameObject step_counter = GameObject.Find("nmbr_steps").gameObject;
        step_counter.GetComponentInChildren<Text>().text = s_best + ", " + s_curr;
    }
}
