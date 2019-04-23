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
        direction = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.position.y + 0.5f != GameObject.Find("Character").GetComponent<Rigidbody>().position.y){
            Vector3 pos = rb.position;
            pos.y = GameObject.Find("Character").GetComponent<Rigidbody>().position.y;
            pos.y -= 0.5f;
            rb.MovePosition(pos);
            if (rb.position.y == 0.5){
                level = 1;
                direction = Vector3.zero;
            }
            else if (rb.position.y == -0.5){
                level = 0;
                direction = Vector3.zero;
            }

        }
    }

    void OnTriggerEnter(Collider other){
        if (other.gameObject.name == "Character")
        {
            if (level == 0)
                    direction = Vector3.up;
                else
                    direction = Vector3.down;
            new_pos = rb.position + direction;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Box"){
            BoxPushedScript BPS = other.gameObject.GetComponent<BoxPushedScript>();
            if (!BPS.IsMoving()){
                BPS.Pushed(gameObject);
            }
            
        }
        if (other.gameObject.name == "Character")
        {
            CharacterControllerScript CCS = other.GetComponent<CharacterControllerScript>();
            Rigidbody CRB = other.GetComponent<Rigidbody>();
            
            if (!CCS.IsMoving() && CRB.position.y == level)
            {
                if (level == 1 && direction.y == -1 || level == 0 && direction.y == 1){
                    CCS.SetDir(direction.x, direction.y, direction.z);
                    Vector3 cur_pos = CRB.position;
                    CCS.SetNextPos(cur_pos, direction);
                    CCS.SetMoving(true);
                }
            }
        }
    }
}
