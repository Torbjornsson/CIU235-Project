using UnityEngine;

public class BoxPushedScript : MonoBehaviour, IPusher
{
    public float speed;

    private Rigidbody rb;

    private bool moving;
    private Vector3 direction;
    private Vector3 next_pos;

    public GameObject pusher;
    public IPusher pusher_script;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = new Vector3();
        next_pos = rb.position;
        moving = false;
        pusher = null;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cur_pos = rb.position;

        if (moving)
        {
            Vector3 new_pos = cur_pos + direction * speed * Time.deltaTime;

            if ((direction.x > 0 && rb.position.x >= next_pos.x) || (direction.x < 0 && rb.position.x <= next_pos.x)
                || (direction.z > 0 && rb.position.z >= next_pos.z) || (direction.z < 0 && rb.position.z <= next_pos.z))
            {
                Stop(next_pos);
            }
            else
            {
                rb.MovePosition(new_pos);
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!moving && other.gameObject.name == "Character")
    //    {
    //        // Getting things to use
    //        GameObject c = other.gameObject;
    //        CharacterControllerScript c_script = c.GetComponent<CharacterControllerScript>();
    //        Vector3 cur_pos = rb.position;

    //        // Checking character diff from original position
    //        Vector3 c_pos = c.GetComponent<Rigidbody>().position;
    //        Vector3 c_grid_pos = Utility.GetGridPos(c_pos, Utility.GRID_SIZE);
    //        c_grid_pos.y = c_pos.y;
    //        Vector3 diff = c_pos - c_grid_pos;

    //        // Starting to move in the right direction
    //        direction = c_script.direction;
    //        speed = c_script.speed;
    //        next_pos = cur_pos + direction * Utility.GRID_SIZE;
    //        moving = true;

    //        // Updating position to be off exactly as much as character, from grid
    //        cur_pos += diff;
    //        rb.MovePosition(cur_pos);

    //        pusher = c;
    //        pusher_script = c_script;
    //    }
    //}

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

            pusher = c;
            pusher_script = c_script;

            RaycastHit hit = new RaycastHit();
            rb.SweepTest(direction, out hit);
            if (hit.collider != null && hit.collider.gameObject.name == "Wall" && hit.distance < Utility.GRID_SIZE)
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

        if (moving && other.gameObject.name == "Wall")
        {
            Vector3 grid_pos = Utility.GetGridPos(rb.position, Utility.GRID_SIZE);
            grid_pos.y = rb.position.y;
            Stop(grid_pos);

            Debug.Log("BOX - Collision with wall, new pos: " + grid_pos);
        }
    }

    public void Stop(Vector3 position)
    {
        moving = false;
        rb.MovePosition(position);

        if (pusher != null && pusher_script != null)
        {
            Vector3 p_pos = pusher.GetComponent<Rigidbody>().position;
            Vector3 p_grid_pos = Utility.GetGridPos(p_pos, Utility.GRID_SIZE);
            p_grid_pos.y = p_pos.y;
            pusher_script.Stop(p_grid_pos);

            pusher = null;
            pusher_script = null;
        }
    }
}
