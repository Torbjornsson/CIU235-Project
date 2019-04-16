using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMasterScript : MonoBehaviour
{
    bool isAxisUsed;
    bool levelWin;
    public Stack undoStack = new Stack();
    public Stack undoStackC = new Stack();

    private static GameMasterScript instance = null;
    public static GameMasterScript Instance {
        get { return instance; }
    }
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Reset") != 0){
            if (!isAxisUsed){
                isAxisUsed = true;
                ResetLevel();
            }
        }
        else if (Input.GetAxis("Reset") == 0)
        {
            isAxisUsed = false;
        }
        if (Input.GetAxis("Menu") != 0)
        {
            if (!isAxisUsed){
                isAxisUsed = true;
                Debug.Log("QUIT");
                Quit();
            }
        }
        else if(Input.GetAxis("Menu") == 0)
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
}
