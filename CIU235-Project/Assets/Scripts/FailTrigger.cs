using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailTrigger : MonoBehaviour
{
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Box")
        {
            Debug.Log("Reached failstate");
            canvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Box")
        {
            Debug.Log("Undone failstate");
            canvas.gameObject.SetActive(false);
        }
    }
}
