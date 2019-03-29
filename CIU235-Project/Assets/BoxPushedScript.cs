using UnityEngine;

public class BoxPushedScript : MonoBehaviour, IPusher
{
    public float speed;

    private Rigidbody rb;

    private bool moving;
    private Vector3 direction;
    private Vector3 next_pos;

    //public GameObject pusher;
    //public IPusher pusher_script;
    //public GameObject in_front;
    //public IPusher in_front_script;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = new Vector3();
        next_pos = rb.position;
        moving = false;
        //pusher = null;
        //pusher_script = null;
        //in_front = null;
        //in_front_script = null;
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

            //pusher = c;
            //pusher_script = c_script;

            //c_script.SetInFront(gameObject, this);

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

        if (moving && other.gameObject.tag == "Wall")
        {
            Vector3 grid_pos = Utility.GetGridPos(rb.position, Utility.GRID_SIZE);
            grid_pos.y = rb.position.y;
            Stop(grid_pos);

            Debug.Log("BOX - Collision with wall, new pos: " + grid_pos);
        }
    }

    //public void SetInFront(GameObject in_front, IPusher in_front_script)
    //{
    //    //this.in_front = in_front;
    //    //this.in_front_script = in_front_script;
    //}

    public void Stop(Vector3 position)
    {
        moving = false;
        rb.MovePosition(position);

        //if (pusher != null && pusher_script != null)
        //{
        //    Vector3 p_pos = pusher.GetComponent<Rigidbody>().position;
        //    Vector3 p_grid_pos = Utility.GetGridPos(p_pos, Utility.GRID_SIZE);
        //    p_grid_pos.y = p_pos.y;
        //    pusher_script.Stop(p_grid_pos);

        //    pusher = null;
        //    pusher_script = null;
        //}

        //in_front = null;
        //in_front_script = null;
    }

    //public bool CollisionCheckInFront(Vector3 direction)
    //{
    //    bool collision = false;

    //    if (in_front_script != null)
    //    {
    //        collision = in_front_script.CollisionCheckInFront(direction);
    //    }
    //    else
    //    {
    //        RaycastHit hit = new RaycastHit();
    //        rb.SweepTest(direction, out hit);
    //        collision |= (hit.collider != null && hit.collider.gameObject.name == "Wall" && hit.distance < Utility.GRID_SIZE);
    //    }

    //    return collision;
    //}

    public bool CollisionCheckInFront(Vector3 direction)
    {
        bool collision = false;

        RaycastHit hit = new RaycastHit();
        rb.SweepTest(direction, out hit);
        if (hit.collider != null && hit.distance < Utility.GRID_SIZE)
        {
            collision |= (hit.collider.gameObject.tag == "Wall");

            if (hit.collider.gameObject.tag == "Box")
            {
                collision |= hit.collider.gameObject.GetComponent<BoxPushedScript>().CollisionCheckInFront(direction);
            }
        }

        return collision;
    }
}
