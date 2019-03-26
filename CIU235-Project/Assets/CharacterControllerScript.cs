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
    public Vector3 direction;
    private Vector3 next_pos;

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
            //while (rb.position != next_pos)
            //{
            //    rb.MovePosition(next_pos);
            //}
            if (Input.GetAxis("Horizontal") > 0) move(cur_pos, 1, 0, 0);
            if (Input.GetAxis("Horizontal") < 0) move(cur_pos, -1, 0, 0);
            if (Input.GetAxis("Vertical") > 0) move(cur_pos, 0, 0, 1);
            if (Input.GetAxis("Vertical") < 0) move(cur_pos, 0, 0, -1);
        }

        if (moving)
        {
            //rb.MovePosition(cur_pos + direction * speed * Time.deltaTime);
            Vector3 new_pos = cur_pos + direction * speed * Time.deltaTime;
            if ((direction.x > 0 && new_pos.x > next_pos.x) || (direction.x < 0 && new_pos.x < next_pos.x)
                || (direction.z > 0 && new_pos.z > next_pos.z) || (direction.z < 0 && new_pos.z < next_pos.z))
            {
                //while(rb.position != next_pos)
                //{
                    rb.MovePosition(next_pos);
                //}
                moving = false;
            }
            else
            {
                rb.MovePosition(new_pos);
            }
        }
    }

    Vector3 getGridPos(float x, float y, float z)
    {
        Vector3 grid_pos = new Vector3((Mathf.Round( x / grid_size )) * grid_size, y, (Mathf.Round(z / grid_size)) * grid_size);
        return grid_pos;
    }

    void move(Vector3 cur_pos, float x, float y, float z)
    {
        next_pos = new Vector3(cur_pos.x + grid_size * x, cur_pos.y + grid_size * y, cur_pos.z + grid_size * z);
        direction.x = x;
        direction.y = y;
        direction.z = z;
        moving = true;
    }
}
