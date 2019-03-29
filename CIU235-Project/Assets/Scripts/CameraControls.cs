using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public GameObject character;

    private Transform t;

    private Vector3 offset;

    private bool rotating;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.Find("Character");

        offset = transform.position - character.transform.position;

        rotating = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate() 
    {
        transform.position = character.transform.position + offset;
        
    }

    private void RotateC(Transform t1, Transform t2, float degrees, float rotatetime){
        if (rotating)
        {
            return;
        }
        rotating = true;

        float rate = degrees / rotatetime;

        for (float i = 0.0f; i < degrees; i += (Time.deltaTime * rate)){
            transform.RotateAround(character.transform.position, Vector3.up, rate);
        }
        rotating = false;
    }
}
