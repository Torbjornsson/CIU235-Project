using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public enum Facing
    {
        NORTH, EAST, SOUTH, WEST, ROTATING
    }

    public const float DEAD_ZONE = 0.5f;
    public const float EPSILON = 0.0001f;
    public const float ROTATION_SPEED = 150f;

    public GameObject character;
    protected GameMasterScript game_master_script;

    private Vector3 offset;
    private float target_angle;

    private Facing facing;
    private Facing next_facing;

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
        facing = Facing.NORTH;
        next_facing = Facing.NORTH;
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

    private void InitiateRotation(float angle)
    {
        target_angle = angle;

        switch (facing)
        {
            case Facing.NORTH:
                if (angle < 0) next_facing = Facing.EAST;
                else next_facing = Facing.WEST;
                break;
            case Facing.EAST:
                if (angle < 0) next_facing = Facing.SOUTH;
                else next_facing = Facing.NORTH;
                break;
            case Facing.SOUTH:
                if (angle < 0) next_facing = Facing.WEST;
                else next_facing = Facing.EAST;
                break;
            case Facing.WEST:
                if (angle < 0) next_facing = Facing.NORTH;
                else next_facing = Facing.SOUTH;
                break;
            case Facing.ROTATING:
                break;
        }
        facing = Facing.ROTATING;
        Debug.Log("Rotating! " + target_angle + ", next facing: " + next_facing + ", actual rotation: " + transform.rotation);
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

        if (Mathf.Abs(target_angle) < EPSILON) facing = next_facing;
    }

    private void RotateStep(float rotation_delta)
    {
        transform.RotateAround(character.transform.position, Vector3.up, rotation_delta);
        target_angle += rotation_delta;
        offset = Quaternion.AngleAxis(rotation_delta, Vector3.up) * offset;
    }

    public Facing GetFacing()
    {
        return facing;
    }
}
