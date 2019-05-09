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

    public Stack undo_stack = new Stack();
    //public Stack undoStackC = new Stack();
    bool level_win;

    private static GameMasterScript instance = null;
    public static GameMasterScript Instance {
        get { return instance; }
    }

    public float EPSILON = 0.00001f;

    public Canvas pause_menu;

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
        if ((Input.GetButtonDown(button_reset)))
        {
                ResetLevel();
        }

        if ((Input.GetButtonDown(button_menu)) && SceneManager.GetActiveScene().buildIndex > 0)
        {
            if (!pause_menu.isActiveAndEnabled)
            {
                Pause();
            }
            else if (pause_menu.isActiveAndEnabled)
            {
                Resume();
            }
        }

        if (Input.GetButtonDown(button_accept))
        {
            Debug.Log("ACCEPT");
        }

        if ((Input.GetButtonDown(button_cancel)))
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
            undo_stack.Clear();
            SceneManager.LoadScene(bIndex);
        }
        
    }

    public void ResetLevel()
    {
        Debug.Log("Reset level");
        undo_stack.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Debug.Log("Quit application");
        Application.Quit();
    }

    public bool UndoAvailable()
    {
        return undo_stack.Count > 0;
    }

    public void Undo()
    {
        if (UndoAvailable())
        {
            StatePackage state = (StatePackage) undo_stack.Pop();
            Debug.Log("Undo !");
            state.ResetState();
            state.Destroy();
        }
    }
    //public Vector3 Undo()
    //{
    //    GameObject go = (GameObject)undoStackC.Pop();
    //    Vector3 pos = (Vector3)undoStack.Pop();
    //    if (go.tag == "Box")
    //    {
    //        Debug.Log(go.GetComponent<Rigidbody>().position);
    //        go.GetComponent<Rigidbody>().MovePosition(pos);
    //        go = (GameObject)undoStackC.Pop();
    //        pos = (Vector3)undoStack.Pop();
    //    }
    //    Debug.Log("Undo " + go + " to pos " + pos);

    //    return pos;
    //}

    public void RecordUndo()
    {
        GameObject character = GameObject.FindWithTag("Player");
        RecordUndo(character, character.GetComponent<Rigidbody>().position);
    }

    public void RecordUndo(GameObject character, Vector3 position)
    {
        StatePackage state = new StatePackage(character, position);
        //Debug.Log("Recorded " + character.name + " at pos " + position);

        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        foreach(GameObject box in boxes)
        {
            state.AddObject(box, box.GetComponent<Rigidbody>().position);
        }

        undo_stack.Push(state);
    }

    //public void RecordUndo(GameObject go,Vector3 pos)
    //{
    //    //Debug.Log("Recorded " + go.name + " at pos " + pos);
    //    undoStack.Push(pos);
    //    undoStackC.Push(go);
    //}

    public System GetSystem()
    {
        return system;
    }

    public void Pause(){
        Debug.Log("Menu");
        pause_menu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume(){
        Debug.Log("Resume");
        pause_menu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void LoadLevel(int n){
        Debug.Log(SceneManager.GetSceneByBuildIndex(n).name);
        SceneManager.LoadScene(n);
    }
}
