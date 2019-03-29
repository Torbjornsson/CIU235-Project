using UnityEngine;

public class BoxPushedScript : MonoBehaviour
{
    private Rigidbody rb;

    private bool moving;
    private Vector3 direction;
    private Vector3 next_pos;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
                rb.MovePosition(next_pos);
                moving = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!moving && other.gameObject.name == "Character")
        {
            // Getting things to use
            GameObject c = other.gameObject;
            CharacterControllerScript c_script = c.GetComponent<CharacterControllerScript>();
            Vector3 cur_pos = rb.position;

            // Checking character diff from original position
            Vector3 c_pos = c.GetComponent<Rigidbody>().position;
            Vector3 diff = c_pos - Utility.GetGridPos(c_pos, c_script.grid_size);

            // Starting to move in the right direction
            direction = c_script.direction;
            speed = c_script.speed;
            next_pos = cur_pos + direction * c_script.grid_size;
            moving = true;

            // Updating position to be off exactly as much as character, from grid
            cur_pos += diff;
            rb.MovePosition(cur_pos);
        }
    }
}
