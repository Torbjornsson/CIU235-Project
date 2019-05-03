using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI : MonoBehaviour
{
    public UnityEngine.Canvas levelSelection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void LevelSelect()
    {
        levelSelection.gameObject.SetActive(true);
        GameObject.Find("BackButton").GetComponent<UnityEngine.UI.Button>().Select();
        gameObject.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
