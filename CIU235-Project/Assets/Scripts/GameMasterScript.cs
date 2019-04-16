using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMasterScript : MonoBehaviour
{
    enum System
    {
        OSX, LIN, WIN
    }
    private System system;

    private string buttonReset;
    private string buttonMenu;

    bool isAxisUsed;
    bool levelWin;

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
        levelWin = false;
        isAxisUsed = false;

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

        buttonReset = (system == System.OSX) ? "ResetOSX" : "Reset";
        buttonMenu = (system == System.OSX) ? "MenuOSX" : "Menu";
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Input.GetAxis(buttonReset)) > EPSILON)
        {
            if (!isAxisUsed){
                isAxisUsed = true;
                ResetLevel();
            }
        }
        else if (Mathf.Abs(Input.GetAxis(buttonReset)) <= EPSILON)
        {
            isAxisUsed = false;
        }

        if (Mathf.Abs(Input.GetAxis(buttonMenu)) > EPSILON)
        {
            if (!isAxisUsed){
                isAxisUsed = true;
                Debug.Log("QUIT");
                Quit();
            }
        }
        else if(Mathf.Abs(Input.GetAxis(buttonMenu)) <= EPSILON)
        {
            isAxisUsed = false;
        }
    }

    public void LevelWin()
    {
        levelWin = true;
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
}
