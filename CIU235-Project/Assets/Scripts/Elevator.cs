using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Pusher
{
    //public int level;
    //public Vector3 direction;
    //public Rigidbody rb;
    //private Vector3 new_pos, c_new_pos;

    //private GameObject character;
    //private Rigidbody char_rb;

    //public float speed;
    //private Vector3 next_pos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //direction = Vector3.zero;
        //character = GameObject.Find("Character");
        //char_rb = character.GetComponent<Rigidbody>();

        moving = false;
        next_pos = rb.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if (rb.position.y != char_rb.position.y)
        //{
        //    Vector3 pos = rb.position;
        //    pos.y = char_rb.position.y;

        //    //pos.y -= 0.5f;
        //    if(pos.y - rb.position.y < -0.1f){
        //        direction = Vector3.down;
        //    }
        //    else if (pos.y - rb.position.y > 0.1f){
        //        direction = Vector3.up;
        //    }
        //    rb.MovePosition(pos);
        //    if (rb.position.y == 1){
        //        level = 1;
        //        direction = Vector3.zero;
        //    }
        //    else if (rb.position.y == 0){
        //        level = 0;
        //        direction = Vector3.zero;
        //    }

        //}
    }

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

    //void OnTriggerEnter(Collider other){
    //    if (other.gameObject.name == "Character")
    //    {
    //        CharacterControllerScript ccs = other.gameObject.GetComponent<CharacterControllerScript>();
    //        if (!ccs.IsMoving()) return;

    //        Vector3 inverse = new Vector3(-1.0f,-1.0f,-1.0f);
    //        Vector3 pos = rb.position;
    //        pos.y -= 0.5f;

    //        //Check from where the character entered
    //        RaycastHit hit = new RaycastHit();
    //        Physics.Raycast(pos, Vector3.Scale(ccs.direction, inverse), out hit, Utility.GRID_SIZE);
    //        if (hit.collider != null)
    //        {
    //            Debug.Log("Previous floor: "+hit.collider.gameObject.tag);
    //        }

    //        if (hit.collider == null || (hit.collider != null && hit.collider.gameObject.tag != "Elevator"))
    //        {
    //            if (level == 0)
    //                direction = Vector3.up;
    //            else
    //                direction = Vector3.down;
    //        }
    //    }
    //}
    //void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.name == "Character")
    //    {
    //        CharacterControllerScript CCS = other.GetComponent<CharacterControllerScript>();
    //        Rigidbody CRB = other.GetComponent<Rigidbody>();

    //        if (!CCS.IsMoving() && CRB.position.y == level)
    //        {
    //            if (level == 1 && direction.y == -1 || level == 0 && direction.y == 1){
    //                CCS.SetDir(direction.x, direction.y, direction.z);
    //                Vector3 cur_pos = CRB.position;
    //                CCS.SetNextPos(cur_pos, direction);
    //                CCS.SetMoving(true);
    //            }
    //        }
    //    }
    //    else if (other.gameObject.tag == "Box")
    //    {
    //        BoxPushedScript BPS = other.gameObject.GetComponent<BoxPushedScript>();
    //        Debug.Log("Move box? is moving: " + BPS.IsMoving() + ", direction: " + direction+", level: "+level);
    //        if (!BPS.IsMoving() && !direction.Equals(Vector3.zero))
    //        {
    //            //Debug.Log("Move box? direction: " + direction);
    //            BPS.Pushed(gameObject);
    //        }

    //    }
    //}
}
