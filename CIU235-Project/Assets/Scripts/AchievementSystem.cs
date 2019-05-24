using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementSystem : MonoBehaviour
{
    ArrayList steps = new ArrayList();
    private static AchievementSystem instance = null;
    public static AchievementSystem Instance {
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSteps(int lvl, int n)
    {
        if(steps.Count == 0)
            steps.Add(0);
        
        if(steps.Count > lvl)
        {
        if((int)steps[lvl] > n)
            steps[lvl] = n;
        }
        else 
        {
            steps.Add(n);
        }
    }

    public int GetSteps(int lvl)
    {
        return (int)steps[lvl];
    }
}
