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
    private string button_accept;
    private string button_cancel;

    bool isAxisUsed;
    bool levelWin;
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
        button_accept = (system == System.OSX) ? "AcceptOSX" : "Accept";
        button_cancel = (system == System.OSX) ? "CancelOSX" : "Cancel";
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

        if (Mathf.Abs(Input.GetAxis(button_accept)) > EPSILON)
        {
            Debug.Log("ACCEPT");
        }

        if (Mathf.Abs(Input.GetAxis(button_cancel)) > EPSILON)
        {
            Debug.Log("CANCEL");
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
            undoStack.Clear();
            SceneManager.LoadScene(bIndex);
        }
        
    }

    public void ResetLevel()
    {
        Debug.Log("Reset level");
        undoStack.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit(){
        Debug.Log("Quit application");
        Application.Quit();
    }

    public Vector3 Undo(){
        GameObject go = (GameObject)undoStackC.Pop();
        Vector3 pos = (Vector3)undoStack.Pop();
        if (go.tag == "Box")
        {
            Debug.Log(go.GetComponent<Rigidbody>().position);
            go.GetComponent<Rigidbody>().MovePosition(pos);
            go = (GameObject)undoStackC.Pop();
            pos = (Vector3)undoStack.Pop();
        }
        Debug.Log("Undo " + go + " to pos " + pos);

        return pos;
    }

    public void RecordUndo(GameObject go,Vector3 pos){
        Debug.Log("Recorded " + go.name + " at pos " + pos);
        undoStack.Push(pos);
        undoStackC.Push(go);
    }

    public System GetSystem()
    {
        return system;
    }
}
