using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{
    public int nmr_scns;

    public Canvas menu, levelSelection;

    public GameObject button_prefab;
    // Start is called before the first frame update
    void Start()
    {
        nmr_scns = SceneManager.sceneCountInBuildSettings;

        for (int i = 1; i < nmr_scns; i++){
            GameObject go = (GameObject)Instantiate(button_prefab);
            go.transform.SetParent(this.transform, false);

            Button btn = go.GetComponent<Button>();
            int tmp = i;
            
            btn.onClick.AddListener(() => SelectLevel(tmp));
            btn.GetComponentInChildren<Text>().text = tmp.ToString();
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectLevel(int n)
    {
        Debug.Log("Load level " + n);
        SceneManager.LoadScene(n);
    }

    public void Back()
    {
        menu.gameObject.SetActive(true);
        levelSelection.gameObject.SetActive(false);
    }
}
