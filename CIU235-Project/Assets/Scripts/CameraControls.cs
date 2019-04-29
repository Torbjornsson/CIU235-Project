using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public const float DEAD_ZONE = 0.5f;
    public const float EPSILON = 0.0001f;
    public const float ROTATION_SPEED = 150f;

    public GameObject character;
    protected GameMasterScript game_master_script;

    private Vector3 offset;
    private float target_angle;

    private string stick_rotate;

    // Start is called before the first frame update
    void Start()
    {
        game_master_script = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();

        character = GameObject.Find("Character");

        transform.position += character.transform.position;
        offset = transform.position - character.transform.position;

        stick_rotate = (game_master_script.GetSystem() == GameMasterScript.System.OSX) ? "RotateOSX" : "RotateOther";

        target_angle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Mathf.Abs(target_angle) < EPSILON)
        {
            if (Input.GetAxis(stick_rotate) > DEAD_ZONE || Input.GetAxis("Rotate") > DEAD_ZONE)
            {
                InitiateRotation(90f);
            }
            else if (Input.GetAxis(stick_rotate) < -DEAD_ZONE || Input.GetAxis("Rotate") < -DEAD_ZONE)
            {
                InitiateRotation(-90f);
            }
        }

        if (Mathf.Abs(target_angle) > EPSILON)
        {
            Rotate();
        }

    }

    private void LateUpdate() 
    {
        transform.position = character.transform.position + offset;
    }

    private void Rotate()
    {

        float rotation_delta = ROTATION_SPEED * Time.deltaTime;
        if (rotation_delta > Mathf.Abs(target_angle)) rotation_delta = Mathf.Abs(target_angle);
        //Debug.Log("Rotating! ROTATION_SPEED ("+ ROTATION_SPEED + ") * Time.deltaTime ("+ Time.deltaTime + ") = " + rotation_delta+", target angle: "+target_angle);

        if (target_angle > 0)
        {
            RotateStep(-rotation_delta);
        }
        else if (target_angle < 0)
        {
            RotateStep(rotation_delta);
        }
    }

    private void InitiateRotation(float angle)
    {
        target_angle = angle;
        //Debug.Log("Rotating! " + target_angle);
    }

    private void RotateStep(float rotation_delta)
    {
        transform.RotateAround(character.transform.position, Vector3.up, rotation_delta);
        target_angle += rotation_delta;
        offset = Quaternion.AngleAxis(rotation_delta, Vector3.up) * offset;
    }
}
