using UnityEngine;

public class BoxPushedScript : Pusher
{
    public float speed;

    //private Rigidbody rb;

    //private bool moving;
    private Vector3 direction;
    private Vector3 next_pos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = new Vector3();
        next_pos = rb.position;
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cur_pos = rb.position;

        if (moving)
        {
            Vector3 new_pos = cur_pos + direction * speed * Time.deltaTime;

            if ((direction.x > 0 && new_pos.x >= next_pos.x) || (direction.x < 0 && new_pos.x <= next_pos.x)
                || (direction.z > 0 && new_pos.z >= next_pos.z) || (direction.z < 0 && new_pos.z <= next_pos.z))
            {
                Stop(next_pos);
            }
            else
            {
                rb.MovePosition(new_pos);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!moving && other.gameObject.name == "Character")
        {
            // Getting things to use
            GameObject c = other.gameObject;
            CharacterControllerScript c_script = c.GetComponent<CharacterControllerScript>();
            Vector3 cur_pos = rb.position;

            // Checking character diff from original position
            Vector3 c_pos = c.GetComponent<Rigidbody>().position;
            Vector3 c_grid_pos = Utility.GetGridPos(c_pos, Utility.GRID_SIZE);
            c_grid_pos.y = c_pos.y;
            Vector3 diff = c_pos - c_grid_pos;

            // Starting to move in the right direction
            direction = c_script.direction;
            speed = c_script.speed;
            next_pos = cur_pos + direction * Utility.GRID_SIZE;
            moving = true;

            if (CollisionCheckInFront(direction))
            {
                Stop(cur_pos);
            }
            else
            {
                // Updating position to be off exactly as much as character, from grid
                cur_pos += diff;
                rb.MovePosition(cur_pos);
            }
        }

        if (moving && (other.gameObject.tag == "Wall"))
        {
            Vector3 grid_pos = Utility.GetGridPos(rb.position, Utility.GRID_SIZE);
            grid_pos.y = rb.position.y;
            Stop(grid_pos);

            Debug.Log("BOX - Collision with wall, new pos: " + grid_pos);
        }
    }

    //public override void Stop(Vector3 position)
    //{
    //    moving = false;
    //    rb.MovePosition(position);
    //}

    //public override bool CollisionCheckInFront(Vector3 direction)
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
