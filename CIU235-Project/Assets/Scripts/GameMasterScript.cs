using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMasterScript : MonoBehaviour
{
    bool isAxisUsed;
    bool levelWin;
    public Stack undoStack = new Stack();

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

    public Vector3 Undo(){
        Vector3 pos = (Vector3)undoStack.Pop();
        Debug.Log("Undo" + pos);

        return pos;
    }

    public void RecordUndo(Vector3 pos){
        Debug.Log("Recorded pos" + pos);
        undoStack.Push(pos);
    }
}
