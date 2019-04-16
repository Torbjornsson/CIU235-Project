using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : Pusher
{
    public const float DELAY_DEFAULT = 0.1f;
    public const float EPSILON = 0.0001f;
    public const float DEAD_ZONE = 0.3f;

    //private Rigidbody rb;
    private float move_delay;

    //private bool moving;
    private Vector3 next_pos;

    public Vector3 direction;
    public float rotation;
    public float speed;
    public GameMasterScript gameMasterScript;

    // Start is called before the first frame update
    void Start()
    {
        gameMasterScript = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        rb = GetComponent<Rigidbody>();
        moving = false;
        direction = new Vector3();
        next_pos = rb.position;
        rotation = 0;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 cur_pos = rb.position;
        

        if (!moving)
        {
            if (Input.GetButtonDown("G")) 
            {
                Vector3 prev_pos = gameMasterScript.Undo();
                Move(cur_pos, prev_pos.x, prev_pos.y, prev_pos.z);
            }
            if (Input.GetAxis("Horizontal") > DEAD_ZONE) Move(cur_pos, 1, 0, 0);
            if (Input.GetAxis("Horizontal") < -DEAD_ZONE) Move(cur_pos, -1, 0, 0);
            if (Input.GetAxis("Vertical") > DEAD_ZONE) Move(cur_pos, 0, 0, 1);
            if (Input.GetAxis("Vertical") < -DEAD_ZONE) Move(cur_pos, 0, 0, -1);

            // After getting a direction and starts to move, checks for collision in that direction
            if (moving && CollisionCheckInFront(direction))
            {
                Stop(cur_pos);
            }
        }

        if (moving)
        {
            Vector3 new_pos = cur_pos + direction * speed * Time.deltaTime;
            if ((direction.x > 0 && new_pos.x >= next_pos.x) || (direction.x < 0 && new_pos.x <= next_pos.x)
                || (direction.z > 0 && new_pos.z >= next_pos.z) || (direction.z < 0 && new_pos.z <= next_pos.z))
            {
                Stop(next_pos);
                gameMasterScript.RecordUndo(next_pos);
                //Debug.Log("CHARACTER - Stopped at next_pos: " + next_pos);
            }
            else
            {
                rb.MovePosition(new_pos);
                //Debug.Log("CHARACTER - Moved to new_pos: " + new_pos);
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

    // Starts to move in a certain direction
    public void Move(Vector3 cur_pos, float dir_x, float dir_y, float dir_z)
    {
        next_pos = new Vector3(cur_pos.x + Utility.GRID_SIZE * dir_x, cur_pos.y + Utility.GRID_SIZE * dir_y, cur_pos.z + Utility.GRID_SIZE * dir_z);
        direction.x = dir_x;
        direction.y = dir_y;
        direction.z = dir_z;
        moving = true;
    }

    public bool IsMoving(){
        return moving;
    }
}
