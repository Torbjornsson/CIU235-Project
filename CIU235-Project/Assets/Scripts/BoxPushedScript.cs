using UnityEngine;

public class BoxPushedScript : Pusher
{
    private Color COLOR_CORRECT = Color.white;
    private Color COLOR_WRONG = Color.red;

    public enum State
    {
        IDLE, CORRECT, WRONG
    }

    public float speed;
    public GameObject shine;
    public GameObject shine_point_light;

    //private Vector3 direction;
    //private Vector3 next_pos;

    private State state;
    private float color_alpha;
    private int color_dir;

    // Start is called before the first frame update
    void Start()
    {
        game_master_script = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        rb = GetComponent<Rigidbody>();
        direction = new Vector3();
        next_pos = rb.position;
        moving = false;
        state = State.WRONG;
        SetState(State.IDLE);
    }

    void Update()
    {
        if (!moving)
        {
            CheckForFall();
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
        //if (!moving)
        //{
        //    if (!CollisionCheckInFront(Vector3.down) && !moving)
        //    {
        //        RaycastHit hit = new RaycastHit();
        //        Vector3 pos = rb.position;
        //        Physics.Raycast(pos, Vector3.down, out hit, Utility.GRID_SIZE * 0.5f);
        //        if (hit.collider != null) return;

        //        pos.y -= 2;
        //        Physics.Raycast(pos, Vector3.up, out hit, Utility.GRID_SIZE * 2);
        //        if (hit.collider != null && hit.collider.gameObject.tag == "Elevator"){
        //            return;
        //        }
        //        if (hit.collider == null || hit.collider.gameObject.tag == "Box"){
        //            Debug.Log("Box y pos" + cur_pos.y);
                    
        //            if (!moving && cur_pos.y == 1){
        //                direction = Vector3.down;
        //                moving = true;
        //                next_pos = cur_pos + direction * Utility.GRID_SIZE;
        //                next_pos = Utility.GetGridPos(next_pos);
        //            }   
        //        }
        //    }
        //}

        if (state == State.WRONG)
        {
            color_alpha += Time.deltaTime * color_dir;

            if ((color_dir < 0 && color_alpha <= 0) || (color_dir > 0 && color_alpha >= 1))
            {
                color_alpha = Mathf.Min(Mathf.Max(color_alpha, 0), 1);
                color_dir *= -1;
            }
            //Debug.Log("Color alpha: "+color_alpha+", color dir: "+color_dir);
            shine.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", new Color(color_alpha, COLOR_WRONG.g, COLOR_WRONG.b, 1));
            //shine.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", new Color(COLOR_WRONG.r, COLOR_WRONG.g, COLOR_WRONG.b, color_alpha));
            shine.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", new Color(color_alpha, COLOR_WRONG.g, COLOR_WRONG.b, 1));

            shine_point_light.GetComponent<Light>().intensity = color_alpha * 0.5f + 1f;
        }
    }

    // Called every time the box is supposed to be pushed in some direction, by a pusher (Character)
    public override void Pushed(GameObject pusher)
    {
        // Getting things to use
        GameObject c = pusher;
        if (c.name == "Character")
        {
            CharacterControllerScript c_script = c.GetComponent<CharacterControllerScript>();
            direction = c_script.GetDir();
            //speed = c_script.speed_push;
            speed = Utility.PUSHING_SPEED;
        }
        else if (c.tag == "Elevator")
        {
            Elevator c_script = c.GetComponent<Elevator>();
            direction = c_script.GetDir();
            //speed = 5;
            speed = Utility.ELEVATOR_SPEED;
        }
        
        Vector3 cur_pos = rb.position;

        // Checking character diff from original position
        Vector3 c_pos = c.GetComponent<Rigidbody>().position;
        Vector3 c_grid_pos = Utility.GetGridPos(c_pos);
        //c_grid_pos.y = c_pos.y;
        Vector3 diff = c_pos - c_grid_pos;
        //if (c.tag == "Elevator")
            //diff.y += 0.5f;

        // Starting to move in the right direction
        //direction = c_script.direction;
        //speed = c_script.speed_push;

        if (!moving)
        {
            next_pos = cur_pos + direction * Utility.GRID_SIZE;
            next_pos = Utility.GetGridPos(next_pos);
            moving = true;
            //if (next_pos.y < 0 || next_pos.y > 1)
            //{
            //    next_pos = cur_pos;
            //    moving = false;
            //}
        }
        if (moving)
        {
            //Check if box is on top of box if so move it also
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(rb.position, Vector3.up, out hit, Utility.GRID_SIZE);
            if (hit.collider != null && hit.collider.tag == "Box"){
                hit.collider.GetComponent<BoxPushedScript>().Pushed(c);
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
                    shine.SetActive(false);
                    shine_point_light.SetActive(false);
                    //shine.GetComponent<MeshRenderer>().materials[0].DisableKeyword("_EMISSION");
                    break;
                case State.CORRECT:
                    shine.SetActive(true);
                    shine.GetComponent<MeshRenderer>().materials[0].EnableKeyword("_EMISSION");
                    shine.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", COLOR_CORRECT);
                    shine.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", COLOR_CORRECT);
                    shine_point_light.SetActive(true);
                    shine_point_light.GetComponent<Light>().color = COLOR_CORRECT;
                    shine_point_light.GetComponent<Light>().intensity = 0.5f;
                    break;
                case State.WRONG:
                    shine.SetActive(true);
                    shine.GetComponent<MeshRenderer>().materials[0].EnableKeyword("_EMISSION");
                    shine.GetComponent<MeshRenderer>().materials[0].SetColor("_Color", COLOR_WRONG);
                    shine.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", COLOR_WRONG);
                    shine_point_light.SetActive(true);
                    shine_point_light.GetComponent<Light>().color = COLOR_WRONG;
                    shine_point_light.GetComponent<Light>().intensity = 0f;
                    color_alpha = 0;
                    color_dir = 1;
                    break;
            }
            this.state = state;
        }

    }
}
