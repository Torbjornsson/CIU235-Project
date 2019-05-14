using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : Pusher
{
    public const float DELAY_DEFAULT = 0.1f;
    public const float EPSILON = 0.0001f;
    public const float DEAD_ZONE = 0.3f;
    public const float SQUEEZE_SIZE = 0.25f;

    public float rotation;
    public float speed;

    public bool pushing;
    public float speed_push;

    public bool elevator_trigger;
    public Vector3 elevator_trigger_pos;

    public GameObject eye;

    private CameraControls camera_script;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        game_master_script = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        camera_script = GameObject.Find("Main Camera").GetComponent<CameraControls>();
        moving = false;
        direction = new Vector3();
        next_pos = rb.position;
        
        rotation = 0;
        pushing = false;
        speed = Utility.CHARACTER_SPEED;

        elevator_trigger = false;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 cur_pos = rb.position;
        bool move_input = false;

        if (!moving)
        {
            CheckForFall();
            if (IsFalling()) game_master_script.ChangeElevatorLevel();
        }

        if (!falling && !moving)
        {
            if (game_master_script.UndoAvailable() && (Input.GetButtonDown("Undo")
                || (game_master_script.GetSystem() == GameMasterScript.System.OSX && Input.GetButtonDown("UndoOSX"))))
            {
                game_master_script.Undo();
                cur_pos = rb.position;
            }

            if (camera_script.GetFacing() != CameraControls.Facing.ROTATING)
            {
                if (Input.GetAxis("Horizontal") > DEAD_ZONE
                    || Input.GetAxis("HorizontalDpad" + game_master_script.GetSystem()) > DEAD_ZONE)
                {
                    SetDir(1, 0, 0);
                    moving = true;
                }
                if (Input.GetAxis("Horizontal") < -DEAD_ZONE
                    || Input.GetAxis("HorizontalDpad" + game_master_script.GetSystem()) < -DEAD_ZONE)
                {
                    SetDir(-1, 0, 0);
                    moving = true;
                }
                if (Input.GetAxis("Vertical") > DEAD_ZONE
                    || Input.GetAxis("VerticalDpad" + game_master_script.GetSystem()) > DEAD_ZONE)
                {
                    SetDir(0, 0, 1);
                    moving = true;
                }
                if (Input.GetAxis("Vertical") < -DEAD_ZONE
                    || Input.GetAxis("VerticalDpad" + game_master_script.GetSystem()) < -DEAD_ZONE)
                {
                    SetDir(0, 0, -1);
                    moving = true;
                }
                if (moving) move_input = true;
            }

            // After getting a direction and starts to move, checks for collision in that direction
            if (moving && CollisionCheckInFront(direction))
            {
                Stop(cur_pos);
                move_input = false;
            }
            else if (moving)
            {
                SetNextPos(cur_pos, direction);
            }

            if (move_input) game_master_script.RecordUndo();

            //Debug.Log("Character trying to move - moving: "+moving+", move_input: "+move_input+", falling: "+falling);
        }

    }

    private void LateUpdate()
    {
        if (moving)
        {
            Vector3 cur_pos = rb.position;

            float temp_speed = speed;
            if (falling) temp_speed = Utility.FALLING_SPEED;
            else if (pushing) temp_speed = Utility.PUSHING_SPEED;
            else if (direction.y != 0) temp_speed = Utility.ELEVATOR_SPEED;

            Vector3 new_pos = cur_pos + direction * temp_speed * Time.deltaTime;

            if ((direction.x > 0 && new_pos.x >= next_pos.x) || (direction.x < 0 && new_pos.x <= next_pos.x)
                || (direction.y > 0 && new_pos.y >= next_pos.y) || (direction.y < 0 && new_pos.y <= next_pos.y)
                || (direction.z > 0 && new_pos.z >= next_pos.z) || (direction.z < 0 && new_pos.z <= next_pos.z))
            {
                Stop(next_pos);
            }
            else
            {
                rb.MovePosition(new_pos);
            }

            if (direction.y == 0)
            {
                // For walking animation:
                float distance = Vector3.Distance(rb.position, next_pos); // Should be between [0,1]
                float height = (1 - SQUEEZE_SIZE) + Mathf.Abs(0.5f - distance) * SQUEEZE_SIZE * 2; // Only change constant, no terms here
                Vector3 scale = new Vector3(1, height, 1); // Only height is affected
                eye.GetComponent<Transform>().localScale = scale;
            }
        }

        UpdateFacing();
    }

    // Updates facing of player
    private void UpdateFacing()
    {
        if (direction.x > 0)
        {
            rotation = 0;
        }
        else if (direction.z < 0)
        {
            rotation = 90;
        }
        else if (direction.x < 0)
        {
            rotation = 180;
        }
        else if (direction.z > 0)
        {
            rotation = 270;
        }

        Quaternion target = Quaternion.Euler(0, rotation, 0);
        gameObject.GetComponent<Transform>().rotation = target;
    }

    public override void SetDir(float dir_x, float dir_y, float dir_z)
    {
        direction = Utility.RotateInputVector(dir_x, dir_y, dir_z, camera_script.GetFacing());
    }

    public override void Stop(Vector3 position)
    {
        base.Stop(position);
        pushing = false;
    }

    public void SetMoving(bool ismoving)
    {
        moving = ismoving;
    }

    public override void Pushed(GameObject pusher)
    {
        // Getting things to use
        GameObject c = pusher;
        if (c.tag == "Elevator")
        {
            Elevator c_script = c.GetComponent<Elevator>();
            direction = c_script.GetDir();
        }

        Vector3 cur_pos = rb.position;

        // Checking character diff from original position
        Vector3 c_pos = c.GetComponent<Rigidbody>().position;
        Vector3 c_grid_pos = Utility.GetGridPos(c_pos);
        Vector3 diff = c_pos - c_grid_pos;

        if (!moving)
        {
            SetNextPos(cur_pos, direction);
            moving = true;
        }

        if (moving)
        {
            // Updating position to be off exactly as much as character, from grid
            cur_pos += diff;
            rb.MovePosition(cur_pos);
        }
    }
}
