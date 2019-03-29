﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : Pusher
{
    public const float DELAY_DEFAULT = 0.1f;
    public const float EPSILON = 0.0001f;

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
        move_delay = 0;
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
            if (Input.GetAxis("Horizontal") > 0) Move(cur_pos, 1, 0, 0);
            if (Input.GetAxis("Horizontal") < 0) Move(cur_pos, -1, 0, 0);
            if (Input.GetAxis("Vertical") > 0) Move(cur_pos, 0, 0, 1);
            if (Input.GetAxis("Vertical") < 0) Move(cur_pos, 0, 0, -1);

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

    private void OnTriggerStay(Collider other)
    {
        if (moving && other.gameObject.name == "Wall")
        {
            Vector3 grid_pos = Utility.GetGridPos(rb.position, Utility.GRID_SIZE);
            grid_pos.y = rb.position.y;
            Stop(grid_pos);
            Debug.Log("CHARACTER - Collision with wall, new pos: " + grid_pos);
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

    //public void Stop(Vector3 position)
    //{
    //    moving = false;
    //    rb.MovePosition(position);
    //}

    //public bool CollisionCheckInFront(Vector3 direction)
    //{
    //    bool collision = false;

    //    RaycastHit hit = new RaycastHit();
    //    rb.SweepTest(direction, out hit);
    //    if (hit.collider != null && hit.distance < Utility.GRID_SIZE)
    //    {
    //        collision |= (hit.collider.gameObject.tag == "Wall");

    //        if (hit.collider.gameObject.tag == "Box")
    //        {
    //            collision |= hit.collider.gameObject.GetComponent<BoxPushedScript>().CollisionCheckInFront(direction);
    //        }
    //    }

    //    return collision;
    //}
}
