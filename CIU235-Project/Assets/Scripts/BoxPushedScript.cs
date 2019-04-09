using UnityEngine;

public class BoxPushedScript : Pusher
{
    public float speed;

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

    // LastUpdate is called once per frame, AFTER every normal Update()
    // -- This needs to be late because otherwise it will conflict with pushing mechanics from Character
    void LateUpdate()
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

    // Called every time the box is supposed to be pushed in some direction, by a pusher (Character)
    public void Pushed(GameObject pusher)
    {
        // Getting things to use
        GameObject c = pusher;
        CharacterControllerScript c_script = c.GetComponent<CharacterControllerScript>();
        Vector3 cur_pos = rb.position;

        // Checking character diff from original position
        Vector3 c_pos = c.GetComponent<Rigidbody>().position;
        Vector3 c_grid_pos = Utility.GetGridPos(c_pos);
        c_grid_pos.y = c_pos.y;
        Vector3 diff = c_pos - c_grid_pos;

        // Starting to move in the right direction
        direction = c_script.direction;
        speed = c_script.speed;
        next_pos = cur_pos + direction * Utility.GRID_SIZE;
        moving = true;

        // Updating position to be off exactly as much as character, from grid
        cur_pos += diff;
        rb.MovePosition(cur_pos);
    }
}
