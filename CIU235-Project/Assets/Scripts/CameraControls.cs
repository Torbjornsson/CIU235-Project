﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public const float DEAD_ZONE = 0.5f;
    public const float EPSILON = 0.0001f;
    public const float ROTATION_SPEED = 150f;

    public GameObject character;
    protected GameMasterScript game_master_script;

    private Quaternion from, to;
    private Vector3 offset;
    public float speed;

    //private bool rotating;
    private int rotate_dir;
    private string stick_rotate;

    private float target_angle;
    //public float r_distance;
    //public float r_speed;

    // Start is called before the first frame update
    void Start()
    {
        game_master_script = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();

        character = GameObject.Find("Character");

        transform.position += character.transform.position;
        offset = transform.position - character.transform.position;
        rotate_dir = 0;
        speed = 0.1f;

        stick_rotate = (game_master_script.GetSystem() == GameMasterScript.System.OSX) ? "RotateOSX" : "RotateOther";

        target_angle = 0;
        //r_distance = 1.0f;
        //r_speed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Mathf.Abs(target_angle) < EPSILON)
        //if (rotate_dir == 0)
        {
            if (Input.GetAxis(stick_rotate) > DEAD_ZONE || Input.GetAxis("Rotate") > DEAD_ZONE)
            {
                //Debug.Log(to.eulerAngles.y);
                //transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                //to.x = 0;
                //to.z = 0;
                //transform.RotateAround(character.transform.position, Vector3.up, 90.0f);

                //rotate_dir = 1;
                //Rotate(rotate_dir);
                target_angle = 90f;
                Debug.Log("Rotating! " + target_angle);
            }
            else if (Input.GetAxis(stick_rotate) < -DEAD_ZONE || Input.GetAxis("Rotate") < -DEAD_ZONE)
            {
                //rotate_dir = -1;
                //Rotate(rotate_dir);
                target_angle = -90f;
                Debug.Log("Rotating! " + target_angle);
            }
        }

        if (Mathf.Abs(target_angle) > EPSILON)
        //if (rotate_dir != 0)
        {
            //Debug.Log("Rotating! "+rotate_dir);

            //rotate_dir = 0;

            //transform.rotation = Quaternion.RotateTowards(from, to, Time.deltaTime * speed);
            Rotate();
            //if (transform.rotation == to)
            //{
            //    //rotating = false;
            //    rotate_dir = 0;
            //}
        }

    }

    private void LateUpdate() 
    {
        transform.position = character.transform.position + offset;
    }

    private void Rotate()
    {
        //from = transform.rotation;
        //to = from;

        //Debug.Log(to.eulerAngles.y);

        //float step = r_speed * Time.deltaTime;
        //float orbit_circumfrance = 2F * r_distance * Mathf.PI;
        //float distance_degrees = (r_speed / orbit_circumfrance) * 360;
        //float distance_radians = (r_speed / orbit_circumfrance) * 2 * Mathf.PI;

        float rotation_delta = ROTATION_SPEED * Time.deltaTime;
        if (rotation_delta > Mathf.Abs(target_angle)) rotation_delta = Mathf.Abs(target_angle);
        Debug.Log("Rotating! ROTATION_SPEED ("+ ROTATION_SPEED + ") * Time.deltaTime ("+ Time.deltaTime + ") = " + rotation_delta+", target angle: "+target_angle);

        if (target_angle > 0)
        {
            transform.RotateAround(character.transform.position, Vector3.up, -rotation_delta);
            target_angle -= rotation_delta;
        }
        else if (target_angle < 0)
        {
            transform.RotateAround(character.transform.position, Vector3.up, rotation_delta);
            target_angle += rotation_delta;
        }

        offset = transform.position - character.transform.position;
        //Quaternion.AngleAxis(30, Vector3.up);
    }

    //private void RotateC(Transform t1, Transform t2, float degrees, float rotatetime){
    //    if (rotating)
    //    {
    //        return;
    //    }
    //    rotating = true;

    //    float rate = degrees / rotatetime;

    //    for (float i = 0.0f; i < degrees; i += (Time.deltaTime * rate)){
    //        transform.RotateAround(character.transform.position, Vector3.up, rate);
    //    }
    //    rotating = false;
    //}
}
