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

    public GameObject fade_obj;
    private Fade fade_script;
    private int selected_level;

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

        fade_script = fade_obj.GetComponent<Fade>();
        fade_obj.SetActive(false);
        selected_level = -1;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (selected_level != -1 && fade_script.fade_done)
        {
            SceneManager.LoadScene(selected_level);
            selected_level = -1;
        }
    }

    public void SelectLevel(int n)
    {
        Debug.Log("Load level " + n);
        //SceneManager.LoadScene(n);
        if (selected_level == -1)
        {
            fade_obj.SetActive(true);
            fade_script.Reset();
            fade_script.StartFade(3.0f, 1);
            selected_level = n;
        }
    }

    public void Back()
    {
        menu.gameObject.SetActive(true);
        GameObject.Find("Start").GetComponent<Button>().Select();
        levelSelection.gameObject.SetActive(false);
    }
}
