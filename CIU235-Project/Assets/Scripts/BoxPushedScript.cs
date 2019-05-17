using UnityEngine;

public class BoxPushedScript : Pusher
{
    private Color COLOR_CORRECT = Color.white;
    private Color COLOR_WRONG = Color.red;
    private const float LIGHT_INTENSITY = 1.1f;
    private const float INTENSITY_CHANGE = 7f;

    public enum State
    {
        IDLE, CORRECT, WRONG
    }

    public float speed;

    public GameObject shine;
    public GameObject shineY;
    public GameObject shineX;
    public GameObject shineZ;
    public GameObject shine_light;

    public Material shineY_mat;
    public Material shineX_mat;
    public Material shineZ_mat;
    public Light shine_light_light;

    private State state;
    private float state_intensity;
    private Color state_color;

    // Start is called before the first frame update
    void Start()
    {
        game_master_script = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        rb = GetComponent<Rigidbody>();

        shineX_mat = shineX.GetComponent<MeshRenderer>().materials[0];
        shineY_mat = shineY.GetComponent<MeshRenderer>().materials[0];
        shineZ_mat = shineZ.GetComponent<MeshRenderer>().materials[0];
        shine_light_light = shine_light.GetComponent<Light>();

        direction = new Vector3();
        next_pos = rb.position;
        moving = false;
        falling = false;

        state_intensity = 0;

        state = State.WRONG; // Both these are needed, because of the way SetState() works!
        SetState(State.IDLE);
    }

    void Update()
    {
        if (!moving)
        {
            CheckForFall();
        }
    }

    public override void Fall()
    {
        base.Fall();

        RaycastHit hit = new RaycastHit();
        Vector3 pos = rb.position;
        pos.y += 0.9f;
        Physics.Raycast(pos, Vector3.up, out hit, Utility.GRID_SIZE);
        if (hit.collider != null && hit.distance < Utility.GRID_SIZE)
        {
            if (hit.collider.gameObject.tag == "Box" || hit.collider.gameObject.name == "Character")
            {
                Pusher pusher_script = hit.collider.gameObject.GetComponent<Pusher>();
                pusher_script.Fall();
            }
        }
    }

    // LastUpdate is called once per frame, AFTER every normal Update()
    // -- This needs to be late because otherwise it will conflict with pushing mechanics from Character
    void LateUpdate()
    {
        Vector3 cur_pos = rb.position;

        if (moving)
        {
            float temp_speed = speed;
            if (falling) temp_speed = Utility.FALLING_SPEED;
            Vector3 new_pos = cur_pos + direction * temp_speed * Time.deltaTime;

            if ((direction.x > 0 && new_pos.x >= next_pos.x) || (direction.x < 0 && new_pos.x <= next_pos.x)
                || (direction.y > 0 && new_pos.y >= next_pos.y) || (direction.y < 0 && new_pos.y <= next_pos.y)
                || (direction.z > 0 && new_pos.z >= next_pos.z) || (direction.z < 0 && new_pos.z <= next_pos.z))
            {
                Stop(next_pos);
            }
            else
            {
                rb.MovePosition(new_pos);
            }
        }

        // Light color and intensity change
        if (state == State.IDLE)
        {
            if (state_intensity > 0)
                state_intensity -= INTENSITY_CHANGE * Time.deltaTime;

            if (state_intensity <= 0 && shine.activeSelf)
            {
                shine.SetActive(false);
                shine_light.SetActive(false);
                state_intensity = 0;
            }
        }
        else
        {
            if (state_intensity < 1)
                state_intensity += INTENSITY_CHANGE * Time.deltaTime;
            if (state_intensity > 1)
                state_intensity = 1;
        }
        SetLight(state_color, state_intensity);
    }

    // Called every time the box is supposed to be pushed in some direction, by a pusher (Character)
    public override void Pushed(GameObject pusher)
    {
        // Getting things to use
        GameObject p = pusher;
        if (p.name == "Character")
        {
            CharacterControllerScript c_script = p.GetComponent<CharacterControllerScript>();
            direction = c_script.GetDir();
            speed = Utility.PUSHING_SPEED;
        }
        else if (p.tag == "Elevator")
        {
            Elevator e_script = p.GetComponent<Elevator>();
            direction = e_script.GetDir();
            speed = Utility.ELEVATOR_SPEED;
        }

        Vector3 cur_pos = rb.position;

        // Checking character diff from original position
        Vector3 p_pos = p.GetComponent<Rigidbody>().position;
        Vector3 p_grid_pos = Utility.GetGridPos(p_pos);
        Vector3 diff = p_pos - p_grid_pos;

        // Starting to move in the right direction
        if (!moving)
        {
            next_pos = cur_pos + direction * Utility.GRID_SIZE;
            next_pos = Utility.GetGridPos(next_pos);
            moving = true;
        }
        if (moving)
        {
            //Check if box is on top of box if so move it also
            RaycastHit hit = new RaycastHit();
            Vector3 ray_pos = rb.position;
            ray_pos.y += 0.1f;
            Physics.Raycast(ray_pos, Vector3.up, out hit, Utility.GRID_SIZE);
            if (hit.collider != null && hit.collider.tag == "Box")
            {
                hit.collider.GetComponent<BoxPushedScript>().Pushed(p);
            }

            // Updating position to be off exactly as much as character, from grid
            cur_pos += diff;
            rb.MovePosition(cur_pos);
        }
    }

    public void SetState(State state)
    {
        if (this.state != state)
        {
            switch (state)
            {
                case State.IDLE:
                    break;
                case State.CORRECT:
                    shine.SetActive(true);
                    shine_light.SetActive(true);
                    state_color = COLOR_CORRECT;
                    break;
                case State.WRONG:
                    shine.SetActive(true);
                    shine_light.SetActive(true);
                    state_color = COLOR_WRONG;
                    break;
            }
            this.state = state;
        }

    }

    private void SetLight(Color color, float intensity)
    {
        color.r = color.r * intensity;
        color.g = color.g * intensity;
        color.b = color.b * intensity;
        color.a = intensity;

        shineX_mat.SetColor("_EmissionColor", color);
        shineX_mat.color = color;
        shineY_mat.SetColor("_EmissionColor", color);
        shineY_mat.color = color;
        shineZ_mat.SetColor("_EmissionColor", color);
        shineZ_mat.color = color;

        shine_light_light.color = color;
        shine_light_light.intensity = intensity * LIGHT_INTENSITY;
    }
}
