using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Pusher
{
    public GameObject elevator_trigger;
    public ElevatorTrigger trigger_script;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        moving = false;
        next_pos = rb.position;

        trigger_script = elevator_trigger.GetComponent<ElevatorTrigger>();
    }

    // Update is called once per frame
    //void Update() { }

    private void LateUpdate()
    {
        Vector3 cur_pos = rb.position;

        if (moving)
        {
            Vector3 new_pos = cur_pos + direction * Utility.ELEVATOR_SPEED * Time.deltaTime;

            if ((direction.y > 0 && new_pos.y >= next_pos.y) || (direction.y < 0 && new_pos.y <= next_pos.y))
            {
                Stop(next_pos);
            }
            else
            {
                rb.MovePosition(new_pos);
            }
        }
    }

    public void MoveToLevel(int level)
    {
        if (!moving)
        {
            Vector3 cur_pos = rb.position;

            direction = Vector3.zero;
            if (cur_pos.y <= 0 && level == 1)
            {
                direction = Vector3.up;
            }
            else if (cur_pos.y >= 1 && level == 0)
            {
                direction = Vector3.down;
            }

            if (direction != Vector3.zero)
            {
                //Debug.Log("Elevator moving! direction: "+direction);
                next_pos = cur_pos + direction * Utility.GRID_SIZE;
                moving = true;

                CollisionCheckInFront(direction);
            }
        }
    }

    public void ResetToLevel(int level)
    {
        moving = false;

        Vector3 cur_pos = rb.position;
        if (level == 0 && cur_pos.y > level || level == 1 && cur_pos.y < level)
        {
            Vector3 new_pos = Utility.GetGridPos(rb.position);
            new_pos.y = level;
            rb.MovePosition(new_pos);
            next_pos = new_pos;
        }
    }
}
