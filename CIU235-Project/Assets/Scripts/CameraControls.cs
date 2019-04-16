using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public GameObject character;

    private Quaternion from, to;

    private Vector3 offset;

    public float speed;

    private bool rotating;
    // Start is called before the first frame update
    void Start()
    {
        
        character = GameObject.Find("Character");

        transform.position += character.transform.position;

        offset = transform.position - character.transform.position;

        rotating = false;

        speed = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!rotating)
        {
            from = transform.rotation;
            to = from;
            if (Input.GetAxis("Rotate") > 0.5)
            {
                //Debug.Log(to.eulerAngles.y);
                //transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                //to.x = 0;
                //to.z = 0;
                //transform.RotateAround(character.transform.position, Vector3.up, 90.0f);
                rotating = true;
                
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(from, to, Time.deltaTime * speed);
            if (transform.rotation == to)
            {
                rotating = false;
            }
        }

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
