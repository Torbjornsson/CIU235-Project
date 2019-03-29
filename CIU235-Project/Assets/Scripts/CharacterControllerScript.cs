using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : Pusher
{
    public const float DELAY_DEFAULT = 0.1f;
    public const float EPSILON = 0.0001f;

    public const float DEAD_ZONE = 0.5f;

    //private Rigidbody rb;
    private float move_delay;

    //private bool moving;
    private Vector3 next_pos;

    public Vector3 direction;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moving = false;
        direction = new Vector3();
        next_pos = rb.position;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 cur_pos = rb.position;

        if (!moving)
        {
            if (Input.GetAxis("Horizontal") > DEAD_ZONE) Move(cur_pos, 1, 0, 0);
            if (Input.GetAxis("Horizontal") < -DEAD_ZONE) Move(cur_pos, -1, 0, 0);
            if (Input.GetAxis("Vertical") > DEAD_ZONE) Move(cur_pos, 0, 0, 1);
            if (Input.GetAxis("Vertical") < -DEAD_ZONE) Move(cur_pos, 0, 0, -1);

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
                Debug.Log("CHARACTER - Stopped at next_pos: " + next_pos);
            }
            else
            {
                rb.MovePosition(new_pos);
                Debug.Log("CHARACTER - Moved to new_pos: " + new_pos);
            }
        }
    }

    public void Move(Vector3 cur_pos, float dir_x, float dir_y, float dir_z)
    {
        next_pos = new Vector3(cur_pos.x + Utility.GRID_SIZE * dir_x, cur_pos.y + Utility.GRID_SIZE * dir_y, cur_pos.z + Utility.GRID_SIZE * dir_z);
        direction.x = dir_x;
        direction.y = dir_y;
        direction.z = dir_z;
        moving = true;
    }
}
