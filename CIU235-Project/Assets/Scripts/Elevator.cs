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

    void OnTriggerEnter(Collider other){
        if (other.gameObject.name == "Character")
        {
            direction.y *= -1;
            new_pos = rb.position + direction;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Character")
        {
            CharacterControllerScript CCS = other.GetComponent<CharacterControllerScript>();
            Rigidbody CRB = other.GetComponent<Rigidbody>();
            
            if (!CCS.IsMoving() && CRB.position.y == level)
            {
                Debug.Log("Level " + level + "Direction "+ direction);
                if ((level == 0 && direction.Equals(Vector3.up)) || level == 1 && direction.Equals(Vector3.down)){
                CCS.SetDir(direction.x, direction.y, direction.z);
                Vector3 cur_pos = CRB.position;
                CCS.SetNextPos(cur_pos, direction);
                CCS.SetMoving(true);
                }
            }
            
            if (level == 0 && direction.Equals(Vector3.up) && CRB.position.x == rb.position.x && CRB.position.z == rb.position.z)
            {
                rb.MovePosition(rb.position + direction * Time.deltaTime * CCS.speed);
                if (rb.position.y >= new_pos.y)
                {
                    rb.MovePosition(new_pos);
                    level = 1;
                    CCS.SetMoving(true);
                }
                
            }

            if(level == 1 && direction.Equals(Vector3.down) && CRB.position.x == rb.position.x && CRB.position.z == rb.position.z)
            {
                rb.MovePosition(rb.position + direction * Time.deltaTime * CCS.speed);
                if (rb.position.y <= new_pos.y){
                    rb.MovePosition(new_pos);
                    level = 0;
                    //CCS.SetMoving(false);
                }
            }
        }
    }
}
