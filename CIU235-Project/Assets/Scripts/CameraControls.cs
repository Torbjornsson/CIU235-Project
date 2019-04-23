using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public const float DEAD_ZONE = 0.5f;

    public GameObject character;
    protected GameMasterScript game_master_script;

    private Quaternion from, to;
    private Vector3 offset;
    public float speed;

    //private bool rotating;
    private int rotate_dir;
    private string stick_rotate;

    // Start is called before the first frame update
    void Start()
    {
        game_master_script = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();

        character = GameObject.Find("Character");

        offset = transform.position - character.transform.position;
        rotate_dir = 0;
        speed = 0.1f;

        stick_rotate = (game_master_script.GetSystem() == GameMasterScript.System.OSX) ? "RotateOSX" : "RotateOther";
    }

    // Update is called once per frame
    void Update()
    {
        
        if (rotate_dir == 0)
        {
            from = transform.rotation;
            to = from;
            if (Input.GetAxis(stick_rotate) > DEAD_ZONE || Input.GetAxis("Rotate") > DEAD_ZONE)
            {
                //Debug.Log(to.eulerAngles.y);
                //transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                //to.x = 0;
                //to.z = 0;
                //transform.RotateAround(character.transform.position, Vector3.up, 90.0f);
                rotate_dir = 1;
                
            }
            else if (Input.GetAxis(stick_rotate) < -DEAD_ZONE || Input.GetAxis("Rotate") < -DEAD_ZONE)
            {
                rotate_dir = -1;
            }
        }

        if (rotate_dir != 0)
        {
            Debug.Log("Rotating! "+rotate_dir);

            //transform.rotation = Quaternion.RotateTowards(from, to, Time.deltaTime * speed);
            //if (transform.rotation == to)
            //{
            //    rotating = false;
            //}
            rotate_dir = 0;
        }

    }

    private void LateUpdate() 
    {
        transform.position = character.transform.position + offset;
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
