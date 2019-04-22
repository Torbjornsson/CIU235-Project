using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public int level;
    public Vector3 direction;
    public Rigidbody rb;
    private Vector3 new_pos, c_new_pos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.name == "Character")
        {
            direction.y *= -1;
            new_pos = rb.position + direction;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Character"){
            Debug.Log(direction);
            if (level == 0 && direction.Equals(Vector3.up))
            {
                Move(other);
                if (rb.position.y >= new_pos.y){
                    rb.MovePosition(new_pos);
                    other.GetComponent<Rigidbody>().MovePosition(new_pos + direction/2);
                    direction = Vector3.up;
                    level = 1;
                }
                
            }
            if(level == 1 && direction.Equals(Vector3.down))
            {
                Move(other);
                if (rb.position.y <= new_pos.y){
                    rb.MovePosition(new_pos);
                    other.GetComponent<Rigidbody>().MovePosition(new_pos - direction/2);
                    direction = Vector3.down;
                    level = 0;
                }
            }
        }
    }

    void Move(Collider other){
        Vector3 cur_pos = other.GetComponent<Rigidbody>().position;
        other.GetComponent<Rigidbody>().MovePosition(cur_pos + direction * Time.deltaTime);
        rb.MovePosition(rb.position + direction * Time.deltaTime);
    }
}
