using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPushedScript : MonoBehaviour
{
    private Rigidbody rb;
    private BoxCollider bc;
    //private GameObject character;
    private bool moving;
    private Vector3 direction;
    private Vector3 next_pos;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        //character = GameObject.Find("Character");
        direction = new Vector3();
        next_pos = rb.position;
        moving = false;
    }

    // Update is called once per frame
    void Update() { }

    void FixedUpdate()
    {
        if (moving)
        {
            Vector3 cur_pos = rb.position;
            rb.MovePosition(cur_pos + direction * speed * Time.deltaTime);
            if ((direction.x > 0 && rb.position.x > next_pos.x) || (direction.x < 0 && rb.position.x < next_pos.x)
                || (direction.z > 0 && rb.position.z > next_pos.z) || (direction.z < 0 && rb.position.z < next_pos.z))
            {
                //while(rb.position != next_pos)
                //{
                rb.MovePosition(next_pos);
                //}
                moving = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision? name: "+ other.gameObject.name);

        if (!moving && other.gameObject.name == "Character")
        {
            GameObject c = other.gameObject;
            CharacterControllerScript script = c.GetComponent<CharacterControllerScript>();
            Vector3 cur_pos = rb.position;

            direction = script.direction;
            speed = script.speed;
            //next_pos = new Vector3(cur_pos.x + script.grid_size * x, cur_pos.y + script.grid_size * y, cur_pos.z + script.grid_size * z);
            next_pos = cur_pos + direction * script.grid_size;
            moving = true;

            //rb.MovePosition
        }
    }
}
