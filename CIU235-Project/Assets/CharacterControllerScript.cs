using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    public const float DELAY_DEFAULT = 0.2f;
    public const float EPSILON = 0.0001f;

    private Rigidbody rb;
    private float move_delay;

    private bool moving;
    private Vector3 next_pos;

    public Vector3 direction;
    public float speed;
    public float grid_size;

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
    void Update() {}

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 cur_pos = rb.position;

        if (!moving)
        {
            if (Input.GetAxis("Horizontal") > 0) Move(cur_pos, 1, 0, 0);
            if (Input.GetAxis("Horizontal") < 0) Move(cur_pos, -1, 0, 0);
            if (Input.GetAxis("Vertical") > 0) Move(cur_pos, 0, 0, 1);
            if (Input.GetAxis("Vertical") < 0) Move(cur_pos, 0, 0, -1);
        }

        if (moving)
        {
            Vector3 new_pos = cur_pos + direction * speed * Time.deltaTime;
            if ((direction.x > 0 && new_pos.x >= next_pos.x) || (direction.x < 0 && new_pos.x <= next_pos.x)
                || (direction.z > 0 && new_pos.z >= next_pos.z) || (direction.z < 0 && new_pos.z <= next_pos.z))
            {
                Stop(next_pos);
                Debug.Log("Stopped at next_pos: " + next_pos);
            }
            else
            {
                rb.MovePosition(new_pos);
                Debug.Log("Moved to new_pos: " + new_pos);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    //private void OnCollisionEnter(Collision collision)
    {
        if (moving && other.gameObject.name == "Wall")
        {
            Vector3 grid_pos = Utility.GetGridPos(rb.position, grid_size);
            grid_pos.y = rb.position.y;
            Stop(grid_pos);
            Debug.Log("Collision with wall, new pos: " + grid_pos);
        }
    }

    void Move(Vector3 cur_pos, float dir_x, float dir_y, float dir_z)
    {
        next_pos = new Vector3(cur_pos.x + grid_size * dir_x, cur_pos.y + grid_size * dir_y, cur_pos.z + grid_size * dir_z);
        direction.x = dir_x;
        direction.y = dir_y;
        direction.z = dir_z;
        moving = true;
    }

    void Stop(Vector3 position)
    {
        moving = false;
        rb.MovePosition(position);
        //next_pos = position;
    }
}
