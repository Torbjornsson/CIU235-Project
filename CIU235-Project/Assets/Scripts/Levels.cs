using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Levels : MonoBehaviour
{
    public int nmr_scns;
    // Start is called before the first frame update
    void Start()
    {
        nmr_scns = SceneManager.sceneCountInBuildSettings;

        for (int i = 1; i < nmr_scns; i++){
            GameObject btn = new GameObject();
            btn.transform.SetParent(gameObject.transform);
            btn.AddComponent<RectTransform>();
            //btn.AddComponent<Button>();
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
