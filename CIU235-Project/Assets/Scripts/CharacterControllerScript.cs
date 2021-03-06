﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : Pusher
{
    public const float DELAY_DEFAULT = 0.1f;
    public const float EPSILON = 0.0001f;
    public const float DEAD_ZONE = 0.3f;
    public const float SQUEEZE_SIZE = 0.2f;

    public const float GOAL_CLOSE_INTENSITY = 0.6f;
    public const float GOAL_CLOSE_DISTANCE = 1.5f;
    public Color EMISSION_COLOR = Color.white;

    public const float INTRO_HEIGHT = 5f;
    public const float INTRO_WIDTH = 0.05f;
    public const float OUTRO_HEIGHT = 4f;
    public const float OUTRO_WIDTH = 0.1f;
    public const float BEAM_WIDTH = 0.4f;

    public float rotation;
    public float speed;

    public bool pushing;
    public float speed_push;

    public bool elevator_trigger;
    public Vector3 elevator_trigger_pos;

    public GameObject eye;
    private Transform eye_trans;
    private Material eye_mat;
    private Color emission_color;
    private float intensity;

    private bool beaming;
    public GameObject beam;
    //private Material beam_mat;
    private Transform beam_trans;

    private CameraControls camera_script;

    //private GameObject win_trigger;
    private WinTrigger win_trigger_script;
    private Transform win_trigger_trans;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        game_master_script = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        camera_script = GameObject.Find("Main Camera").GetComponent<CameraControls>();

        GameObject win_trigger = GameObject.FindWithTag("Goal");
        win_trigger_trans = win_trigger.GetComponent<Transform>();
        win_trigger_script = win_trigger.GetComponent<WinTrigger>();

        eye_trans = eye.GetComponent<Transform>();
        eye_mat = eye.GetComponent<MeshRenderer>().materials[0];

        //beam_mat = beam.GetComponent<MeshRenderer>().materials[0];
        beam_trans = beam.GetComponent<Transform>();
        beam.SetActive(false);
        beaming = false;

        moving = false;
        direction = new Vector3();
        next_pos = rb.position;
        
        rotation = rb.rotation.y;
        pushing = false;
        speed = Utility.CHARACTER_SPEED;

        elevator_trigger = false;

        //emission_color = Color.black;
        intensity = 0;
        SetLight(intensity);
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 cur_pos = rb.position;
        bool move_input = false;

        Vector3 prev_dir = direction;

        if (!moving)
        {
            CheckForFall();
            if (IsFalling()) game_master_script.ChangeElevatorLevel();
        }

        if (!game_master_script.IsLevelTransition() && !falling && !moving)
        {
            if (game_master_script.UndoAvailable() && (Input.GetButtonDown("Undo")
                || (game_master_script.GetSystem() == GameMasterScript.System.OSX && Input.GetButtonDown("UndoOSX"))))
            {
                game_master_script.Undo();
                cur_pos = rb.position;
                Vector3 dir = GetDirFromRot(rb.rotation);
                SetDir(dir.x, dir.y, dir.z);
            }

            if (camera_script.GetFacing() != CameraControls.Facing.ROTATING)
            {
                if (Input.GetAxis("Horizontal") > DEAD_ZONE
                    || Input.GetAxis("HorizontalDpad" + game_master_script.GetSystem()) > DEAD_ZONE)
                {
                    SetDir(1, 0, 0);
                    moving = true;
                }
                if (Input.GetAxis("Horizontal") < -DEAD_ZONE
                    || Input.GetAxis("HorizontalDpad" + game_master_script.GetSystem()) < -DEAD_ZONE)
                {
                    SetDir(-1, 0, 0);
                    moving = true;
                }
                if (Input.GetAxis("Vertical") > DEAD_ZONE
                    || Input.GetAxis("VerticalDpad" + game_master_script.GetSystem()) > DEAD_ZONE)
                {
                    SetDir(0, 0, 1);
                    moving = true;
                }
                if (Input.GetAxis("Vertical") < -DEAD_ZONE
                    || Input.GetAxis("VerticalDpad" + game_master_script.GetSystem()) < -DEAD_ZONE)
                {
                    SetDir(0, 0, -1);
                    moving = true;
                }
                move_input |= moving;
            }

            // After getting a direction and starts to move, checks for collision in that direction
            if (moving && CollisionCheckInFront(direction))
            {
                Stop(cur_pos);
                move_input = false;
            }
            else if (moving)
            {
                SetNextPos(cur_pos, direction);
            }

            if (move_input) game_master_script.RecordUndo();

            //Debug.Log("Character trying to move - moving: "+moving+", move_input: "+move_input+", falling: "+falling);
        }

    }

    private void LateUpdate()
    {
        if (moving)
        {
            Vector3 cur_pos = rb.position;

            float temp_speed = speed;
            if (falling) temp_speed = Utility.FALLING_SPEED;
            else if (pushing) temp_speed = Utility.PUSHING_SPEED;
            else if (direction.y != 0) temp_speed = Utility.ELEVATOR_SPEED;

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

            if (direction.y == 0)
            {
                // For walking animation:
                float distance = Vector3.Distance(rb.position, next_pos); // Should be between [0,1]
                float height = (1 - SQUEEZE_SIZE) + Mathf.Abs(0.5f - distance) * SQUEEZE_SIZE * 2; // Only change constant, no terms here
                Squeeze(height);
            }
        }
        if (Vector3.zero != direction)
            UpdateFacing();

        if (!game_master_script.IsLevelTransition())
        {
            if (win_trigger_script.activated)
            {
                // How long character is from goal
                float goal_dist = Vector3.Distance(rb.position, win_trigger_trans.position);
                // If that distance is closer than a certain treshold, convert it into a ratio-value
                float dist_ratio = (goal_dist > GOAL_CLOSE_DISTANCE) ? 0 : (GOAL_CLOSE_DISTANCE - goal_dist) / GOAL_CLOSE_DISTANCE;
                // And scale it by how much it actually should be
                intensity = dist_ratio * GOAL_CLOSE_INTENSITY;
                //Debug.Log("Distance between win trigger and character: " + goal_dist+", and ratio: "+dist_ratio+", and finally, intensity: "+intensity);
                SetLight(intensity);
            }
            else if (intensity > 0)
            {
                intensity = 0;
                SetLight(intensity);
            }
        }

        beam.SetActive(game_master_script.ShowBeam());
    }

    // Updates facing of player
    private void UpdateFacing()
    {
        if (direction.x > 0)
        {
            rotation = 0;
        }
        else if (direction.z < 0)
        {
            rotation = 90;
        }
        else if (direction.x < 0)
        {
            rotation = 180;
        }
        else if (direction.z > 0)
        {
            rotation = 270;
        }

        Quaternion target = Quaternion.Euler(0, rotation, 0);
        gameObject.GetComponent<Transform>().rotation = target;
    }

    public override void SetDir(float dir_x, float dir_y, float dir_z)
    {
        direction = Utility.RotateInputVector(dir_x, dir_y, dir_z, camera_script.GetFacing());
    }

    public override void Stop(Vector3 position)
    {
        base.Stop(position);
        pushing = false;
    }

    public void SetMoving(bool ismoving)
    {
        moving = ismoving;
    }

    public Vector3 GetDirFromRot(Quaternion rot)
    {
        Vector3 dir = new Vector3();

        if (rot.y == 0)
            dir = new Vector3(1,0,0);
        else if (rot.y == 90)
            dir = new Vector3(0,0,-1);
        else if (rot.y == 180)
            dir = new Vector3(-1,0,0);
        else if (rot.y == 270)
            dir = new Vector3(0,0,1);

        return dir;
    }

    public override void Pushed(GameObject pusher)
    {
        // Getting things to use
        GameObject c = pusher;
        if (c.tag == "Elevator")
        {
            Elevator c_script = c.GetComponent<Elevator>();
            direction = c_script.GetDir();
        }

        Vector3 cur_pos = rb.position;

        // Checking character diff from original position
        Vector3 c_pos = c.GetComponent<Rigidbody>().position;
        Vector3 c_grid_pos = Utility.GetGridPos(c_pos);
        Vector3 diff = c_pos - c_grid_pos;

        if (!moving)
        {
            SetNextPos(cur_pos, direction);
            moving = true;
        }

        if (moving)
        {
            // Updating position to be off exactly as much as character, from grid
            cur_pos += diff;
            rb.MovePosition(cur_pos);
        }
    }

    public void Squeeze(float height)
    {
        Squeeze(height, 1);
    }
    public void Squeeze(float height, float width) // height & width can be anything, but 1 is the default size
    {
        Vector3 scale = new Vector3(width, height, width);
        eye_trans.localScale = scale;

        Vector3 eye_new_pos = eye_trans.position;
        eye_new_pos.y = rb.position.y + height / 2.0f;
        eye_trans.position = eye_new_pos;
    }

    public void SetLight(float intensity) // intensity should be between [0, 1]
    {
        emission_color.r = EMISSION_COLOR.r * intensity;
        emission_color.g = EMISSION_COLOR.g * intensity;
        emission_color.b = EMISSION_COLOR.b * intensity;
        eye_mat.SetColor("_EmissionColor", emission_color);
    }

    public void LevelIntro(float progress) // progress should be between [0, 1]
    {
        float prog_quad = progress * progress;

        // Light intensity
        float treshold = 0.7f;
        float factor = 1 / ((1 - treshold)* (1 - treshold));
        float half_quad = (progress < treshold) ? 0 : ((progress - treshold) * (progress - treshold)) * factor;
        float light_intensity = (1 - half_quad);
        SetLight(light_intensity);

        // Squeezing player
        float height = 1 + (INTRO_HEIGHT - 1) * (1 - prog_quad);
        float width = 1 + (INTRO_WIDTH - 1) * (1 - prog_quad);
        Squeeze(height, width);

        // Un-Levitating to the ground
        Vector3 pos = eye_trans.position;
        pos.y += (1 - prog_quad);
        eye_trans.position = pos;

        // Fading in
        Color c = eye_mat.color;
        c.a = prog_quad;
        eye_mat.color = c;

        // Beaming
        Beam(progress);
    }

    public void LevelOutro(float progress) // progress should be between [0, 1]
    {
        float prog_quad = progress * progress * progress;

        // Light intensity
        float light_intensity = GOAL_CLOSE_INTENSITY + (1 - GOAL_CLOSE_INTENSITY) * prog_quad;
        SetLight(light_intensity);

        // Squeezing player
        float height = 1 + (OUTRO_HEIGHT - 1) * prog_quad;
        float width = 1 + (OUTRO_WIDTH - 1) * prog_quad;
        Squeeze(height, width);

        // Levitating off the ground
        Vector3 pos = eye_trans.position;
        pos.y += prog_quad * 2;
        eye_trans.position = pos;

        // Fading away
        Color c = eye_mat.color;
        c.a = 1 - prog_quad;
        eye_mat.color = c;
        //Debug.Log("Character alfa: " + (1 - prog_quad));

        // Beaming
        Beam(progress);
    }

    private void Beam(float progress)
    {
        // Width calculation
        float beam_w = 0;
        if (progress < 0.2f) beam_w = (progress * 5) * BEAM_WIDTH;
        else if (progress > 0.8f) beam_w = ((1 - progress) * 5) * BEAM_WIDTH;
        else beam_w = BEAM_WIDTH;

        // Scaling
        Vector3 scale = beam_trans.localScale;
        scale.x = beam_w;
        scale.z = beam_w;
        beam_trans.localScale = scale;
    }
}
