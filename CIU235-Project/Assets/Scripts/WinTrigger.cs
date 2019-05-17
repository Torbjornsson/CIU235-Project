using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public const float LIGHT_ACTIVE = 1.5f;
    public const float LIGHT_INACTIVE = 0.5f;
    public const float LIGHT_CHANGE = 2f;

    public bool activated;

    private GameMasterScript game_master_script;

    private ArrayList triggers;

    public GameObject win_pad;
    public GameObject win_point_light;

    public Light win_point_light_comp;
    public Material win_pad_mat;
    public Color win_pad_col;

    // Start is called before the first frame update
    void Start()
    {
        game_master_script = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        triggers = new ArrayList(GameObject.FindGameObjectsWithTag("Trigger"));

        win_point_light_comp = win_point_light.GetComponent<Light>();
        win_point_light_comp.intensity = LIGHT_INACTIVE;

        win_pad_mat = win_pad.GetComponent<MeshRenderer>().materials[0];
        win_pad_mat.EnableKeyword("_EMISSION");
        win_pad_col = Color.black;

        TriggerActivated();

        if (activated)
        {
            win_point_light_comp.intensity = LIGHT_ACTIVE;
            win_pad_col = Color.white;
        }

        win_pad_mat.SetColor("_EmissionColor", win_pad_col);
    }

    // Update is called once per frame
    void Update()
    {
        //Change to som animation trigger and so on
        if (activated)
        {
            if (win_point_light_comp.intensity < LIGHT_ACTIVE)
                win_point_light_comp.intensity += LIGHT_CHANGE * Time.deltaTime;
            if (win_point_light_comp.intensity > LIGHT_ACTIVE)
                win_point_light_comp.intensity = LIGHT_ACTIVE;

            if (win_pad_col.r < 1)
            {
                win_pad_col.r += LIGHT_CHANGE * Time.deltaTime;
                win_pad_col.g += LIGHT_CHANGE * Time.deltaTime;
                win_pad_col.b += LIGHT_CHANGE * Time.deltaTime;
            }
            if (win_pad_col.r > 1)
                win_pad_col = Color.white;

            win_pad_mat.SetColor("_EmissionColor", win_pad_col);
        }
        else
        {
            if (win_point_light_comp.intensity > LIGHT_INACTIVE)
                win_point_light_comp.intensity -= LIGHT_CHANGE * Time.deltaTime;
            if (win_point_light_comp.intensity < LIGHT_INACTIVE)
                win_point_light_comp.intensity = LIGHT_INACTIVE;

            if (win_pad_col.r > 0)
            {
                win_pad_col.r -= LIGHT_CHANGE * Time.deltaTime;
                win_pad_col.g -= LIGHT_CHANGE * Time.deltaTime;
                win_pad_col.b -= LIGHT_CHANGE * Time.deltaTime;
            }
            if (win_pad_col.r < 0)
                win_pad_col = Color.black;

            win_pad_mat.SetColor("_EmissionColor", win_pad_col);
        }
    }

    // Checks for wins (that is, WinTrigger is active and player is present)
    void OnTriggerStay(Collider other) {
        if (other.gameObject.name == "Character" && activated &&
            !other.gameObject.GetComponent<CharacterControllerScript>().IsMoving()
            && !game_master_script.IsLevelTransition())
        {
            //game_master_script.SendMessage("LevelWin");
            game_master_script.SendMessage("GoalReached");
        }
    }

    // Called whenever a Trigger object is activated
    // Checks if ALL Triggers are active, and in that case activates itself
    public void TriggerActivated()
    {
        activated = true;
        foreach (GameObject item in triggers)
        {
            if (!item.gameObject.GetComponent<Trigger>().activated)
                activated = false;
        }
        //Debug.Log("Trigger activated? " + activated);
    }
}
