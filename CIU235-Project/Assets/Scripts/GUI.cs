using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI : MonoBehaviour
{
    public UnityEngine.Canvas levelSelection;

    public GameObject fade_obj;
    private Fade fade_script;
    private bool start_game;
    private bool menu_intro;

    // Start is called before the first frame update
    void Start()
    {
        //fade_obj = GameObject.Find("Fade");
        fade_script = fade_obj.GetComponent<Fade>();

        if (menu_intro)
        {
            fade_obj.SetActive(true);
            fade_script.Reset();
            fade_script.StartFade(3.0f, -1);
            return;
        }

        fade_obj.SetActive(false);
        start_game = false;
        menu_intro = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (start_game && fade_script.fade_done)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            start_game = false;
        }
    }

    public void StartGame()
    {
        if (!start_game)
        {
            fade_obj.SetActive(true);
            fade_script.Reset();
            fade_script.StartFade(3.0f, 1);
            start_game = true;
        }
    }

    public void LevelSelect()
    {
        if (!start_game)
        {
            levelSelection.gameObject.SetActive(true);
            GameObject.Find("BackButton").GetComponent<UnityEngine.UI.Button>().Select();
            gameObject.SetActive(false);
        }
    }

    public void MenuIntro()
    {
        menu_intro = true;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
