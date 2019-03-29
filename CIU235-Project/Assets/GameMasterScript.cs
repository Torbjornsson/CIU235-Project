using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterScript : MonoBehaviour
{
    bool levelWin;
    // Start is called before the first frame update
    void Start()
    {
        levelWin = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelWin()
    {
        levelWin = true;
        Debug.Log("Start new level");
    }
}
