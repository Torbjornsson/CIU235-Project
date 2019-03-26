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
    private Vector3 direction;
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
    }

    // Update is called once per frame
    void Update() {}

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(Input.GetAxis("Vertical") + " - " + Input.GetAxis("Horizontal"));
        //Vector3 pos = rb.position;
        //pos.x += Input.GetAxis("Horizontal") * speed;
        //pos.z += Input.GetAxis("Vertical") * speed;
        //rb.MovePosition(pos);


        //if (move_delay > 0) move_delay -= Time.deltaTime;
        //if (move_delay <= 0)
        //{
        //    Vector3 grid_pos = getGridPos(pos.x, pos.y, pos.z);

        //    Debug.Log("Grid pos 1: " + grid_pos +", axis horizontal: " + Input.GetAxis("Horizontal"));

        //    if (Mathf.Abs(Input.GetAxis("Horizontal")) > EPSILON)
        //    {
        //        move_delay = DELAY_DEFAULT;
        //        grid_pos.x += ((Input.GetAxis("Horizontal") > EPSILON) ? 1 : (Input.GetAxis("Horizontal") < EPSILON) ? -1 : 0) * grid_size;
        //    }
        //    if (Mathf.Abs(Input.GetAxis("Vertical")) > EPSILON)
        //    {
        //        move_delay = DELAY_DEFAULT;
        //        grid_pos.z += ((Input.GetAxis("Vertical") > EPSILON) ? 1 : (Input.GetAxis("Vertical") < EPSILON) ? -1 : 0) * grid_size;
        //    }

        //    Debug.Log("Grid pos 2: " + grid_pos);

        //    Vector3 dir = Vector3.Normalize(pos - grid_pos);

        //    rb.MovePosition(pos + dir * speed);
        //}

        Vector3 cur_pos = rb.position;

        if (!moving)
        {
            if (Input.GetAxis("Horizontal") > 0) move(cur_pos, 1, 0, 0);
            if (Input.GetAxis("Horizontal") < 0) move(cur_pos, -1, 0, 0);
            if (Input.GetAxis("Vertical") > 0) move(cur_pos, 0, 0, 1);
            if (Input.GetAxis("Vertical") < 0) move(cur_pos, 0, 0, -1);
        }

        if (moving)
        {
            rb.MovePosition(cur_pos + direction * speed * Time.deltaTime);
            if ((direction.x > 0 && rb.position.x > next_pos.x) || (direction.x < 0 && rb.position.x < next_pos.x)
                || (direction.z > 0 && rb.position.z > next_pos.z) || (direction.z < 0 && rb.position.z < next_pos.z))
            {
                rb.MovePosition(next_pos);
                moving = false;
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
