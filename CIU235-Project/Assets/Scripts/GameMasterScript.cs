using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMasterScript : MonoBehaviour
{
    public enum System
    {
        OSX, LIN, WIN
    }
    private System system;

    private string button_reset;
    private string button_menu;

    bool is_axis_used;
    bool level_win;

    private static GameMasterScript instance = null;
    public static GameMasterScript Instance {
        get { return instance; }
    }

    public float EPSILON = 0.00001f;

    private void Awake() {
        if (instance != null && instance != this) 
        {
         Destroy(this.gameObject);
         return;
        } 
        else 
        {
         instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        level_win = false;
        is_axis_used = false;

        if (Application.platform.Equals(RuntimePlatform.OSXEditor) || Application.platform.Equals(RuntimePlatform.OSXPlayer))
        {
            system = System.OSX;
        }
        else if (Application.platform.Equals(RuntimePlatform.LinuxEditor) || Application.platform.Equals(RuntimePlatform.LinuxPlayer))
        {
            system = System.LIN;
        }
        else if (Application.platform.Equals(RuntimePlatform.WindowsEditor) || Application.platform.Equals(RuntimePlatform.WindowsPlayer))
        {
            system = System.WIN;
        }
        Debug.Log("Running platform: "+system);

        button_reset = (system == System.OSX) ? "ResetOSX" : "Reset";
        button_menu = (system == System.OSX) ? "MenuOSX" : "Menu";
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Input.GetAxis(button_reset)) > EPSILON)
        {
            if (!is_axis_used)
            {
                is_axis_used = true;
                ResetLevel();
            }
        }
        else if (Mathf.Abs(Input.GetAxis(button_reset)) <= EPSILON)
        {
            is_axis_used = false;
        }

        if (Mathf.Abs(Input.GetAxis(button_menu)) > EPSILON)
        {
            if (!is_axis_used)
            {
                is_axis_used = true;
                Debug.Log("QUIT");
                Quit();
            }
        }
        else if(Mathf.Abs(Input.GetAxis(button_menu)) <= EPSILON)
        {
            is_axis_used = false;
        }
    }

    public void LevelWin()
    {
        level_win = true;
        Debug.Log("Start new level");
        int bIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (bIndex >= SceneManager.sceneCountInBuildSettings){
            Debug.Log("Congraturations U win tHe Games!?");
        }
        else
        {
            SceneManager.LoadScene(bIndex);
        }
        
    }

    public void ResetLevel()
    {
        Debug.Log("Reset level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit(){
        Debug.Log("Quit application");
        Application.Quit();
    }

    public System GetSystem()
    {
        return system;
    }
}
