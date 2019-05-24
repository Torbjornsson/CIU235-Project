using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMasterScript : MonoBehaviour
{
    public enum System
    {
        OSX, LIN, WIN
    }
    private System system;

    public const float INTRO_PROGRESS_PACE = 3;
    public const float OUTRO_PROGRESS_PACE = 3;

    public const float INTRO_FADE_SPEED = 4;
    public const float OUTRO_FADE_SPEED = 4;

    private string button_reset;
    private string button_menu;
    private string button_accept;
    private string button_cancel;

    public Stack undo_stack = new Stack();
    //bool level_win;
    bool level_transition;
    bool level_intro;
    bool level_outro;
    float transition_progress;
    int reset_fade;
    bool exit_outro;

    private int elevator_level;
    GameObject[] elevators;

    public float EPSILON = 0.00001f;

    public Canvas pause_menu;

    private GameObject character;
    private CharacterControllerScript c_script;

    public GameObject achievement;
    private AchievementSystem achievement_script;

    public GameObject fade_obj;
    private Fade fade_script;

    private static GameMasterScript instance = null;
    public static GameMasterScript Instance {
        get { return instance; }
    }

    private void Awake() {
        if (instance != null && instance != this) 
        {
            Destroy(this.gameObject);
            return;
        } 
        else 
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //level_win = false;
        level_transition = true;
        level_intro = true;
        level_outro = false;
        reset_fade = 0;
        exit_outro = false;

        if (Application.platform.Equals(RuntimePlatform.OSXEditor) || Application.platform.Equals(RuntimePlatform.OSXPlayer))
        {
            system = System.OSX;
        }
        else if (Application.platform.Equals(RuntimePlatform.LinuxEditor) || Application.platform.Equals(RuntimePlatform.LinuxPlayer))
        {
            system = System.LIN;
        }
        else if (Application.platform.Equals(RuntimePlatform.WindowsEditor) || Application.platform.Equals(RuntimePlatform.WindowsPlayer))
        {
            system = System.WIN;
        }
        Debug.Log("Running platform: "+system);

        button_reset = (system == System.OSX) ? "ResetOSX" : "Reset";
        button_menu = (system == System.OSX) ? "MenuOSX" : "Menu";
        button_accept = (system == System.OSX) ? "AcceptOSX" : "Accept";
        button_cancel = (system == System.OSX) ? "CancelOSX" : "Cancel";

        //fade_script = fade_obj.GetComponent<Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Scene was loaded: " + SceneManager.GetActiveScene().name);

        if (!IsLevelTransition() && !IsResetFade() && !IsExitOutro())
        {
            DuringGamePlay();
        }
        else // if transition is in progress
        {
            DuringTransition();
        }
    }

    private void DuringGamePlay()
    {
        if (Input.GetButtonDown(button_reset))
        {
            ResetLevel();
        }

        if (Input.GetButtonDown(button_menu) && SceneManager.GetActiveScene().buildIndex > 0)
        {
            if (!pause_menu.isActiveAndEnabled)
            {
                Pause();
            }
            else if (pause_menu.isActiveAndEnabled)
            {
                Resume();
            }
        }

        if (Input.GetButtonDown(button_accept))
        {
            Debug.Log("ACCEPT");
            //ChangeElevatorLevel();
        }

        if (Input.GetButtonDown(button_cancel))
        {
            Debug.Log("CANCEL");
        }

        CheckElevatorTriggering();
    }

    public void DuringTransition()
    {
        //if (IsLevelIntro() && fade_script.fade_done || IsLeveOutro())
        if (ShowBeam())
        {
            float pace = IsLeveOutro() ? OUTRO_PROGRESS_PACE : INTRO_PROGRESS_PACE;
            if (transition_progress < 1)
                transition_progress += pace * Time.deltaTime;
            if (transition_progress > 1)
                transition_progress = 1;
        }

        if (IsLevelIntro())
        {
            if (fade_script.fade_done)
            {
                if (!character.activeSelf) character.SetActive(true);
                //Debug.Log("INTRO - Tranition progress: " + transition_progress);
                c_script.LevelIntro(transition_progress);
                if (transition_progress >= 1) GiveBackControlToPlayer();
            }
            else if (character.activeSelf)
            {
                character.SetActive(false);
            }
        }

        if (IsLeveOutro())
        {
            //Debug.Log("OUTRO - Tranition progress: " + transition_progress);
            c_script.LevelOutro(transition_progress);
            if (transition_progress >= 1)
            {
                fade_obj.SetActive(true);
                if (!fade_script.fading) fade_script.StartFade(OUTRO_FADE_SPEED, 1);
                if (fade_script.fade_done) NextLevel();
            }
        }

        if (IsResetFade())
        {
            if (reset_fade == 1 && fade_script.fade_done)
            {
                undo_stack.Clear();
                LoadLevel(SceneManager.GetActiveScene().buildIndex);

                reset_fade = -1;
                fade_script.Reset();
                fade_script.StartFade(INTRO_FADE_SPEED, -1);
            }
            else if (reset_fade == -1 && fade_script.fade_done)
            {
                reset_fade = 0;
                fade_obj.SetActive(false);
            }
        }

        if (IsExitOutro() && fade_script.fade_done)
        {
            fade_script.Reset();
            LoadLevel(0);
        }
    }

    public void CheckElevatorTriggering()
    {
        if (!c_script.IsMoving()
            && c_script.elevator_trigger_pos != Utility.GetGridPos(c_script.rb.position))
        {
            bool on_elevator = false;
            foreach (GameObject e in elevators)
            {
                if (e.GetComponent<Elevator>().trigger_script.CharacterOnElevator())
                {
                    on_elevator = true;
                    break;
                }
            }

            //Debug.Log("Character on elevator - el trigger? " + c_script.elevator_trigger + ", el trigger pos: " + c_script.elevator_trigger_pos + ", char pos: " + Utility.GetGridPos(c_script.rb.position));

            if (on_elevator && !c_script.elevator_trigger)
            {
                ChangeElevatorLevel();
            }

            c_script.elevator_trigger = on_elevator;
            c_script.elevator_trigger_pos = Utility.GetGridPos(c_script.rb.position);
        }
    }

    public void ChangeElevatorLevel()
    {
        if (!c_script.IsMoving() || c_script.IsFalling())
        {
            bool ok_to_move = true;
            foreach (GameObject elevator in elevators)
            {
                ok_to_move &= !elevator.GetComponent<Elevator>().IsMoving();
            }

            if (ok_to_move)
            {
                elevator_level = (elevator_level + 1) % 2;
                //Debug.Log("Change elevator level to: " + elevator_level);

                foreach (GameObject elevator in elevators)
                {
                    elevator.GetComponent<Elevator>().MoveToLevel(elevator_level);
                }
            }
        }
    }

    private void ResetElevatorLevel(int level)
    {
        //Debug.Log("Reset elevator level! Current: " + elevator_level + ", should be: " + level);

        bool on_elevator = false;
        foreach (GameObject elevator in elevators)
        {
            elevator.GetComponent<Elevator>().ResetToLevel(level);
            on_elevator |= elevator.GetComponent<Elevator>().trigger_script.CharacterOnElevator();
        }

        c_script.elevator_trigger = on_elevator;
        c_script.elevator_trigger_pos = Utility.GetGridPos(c_script.rb.position);

        elevator_level = level;
    }

    public void GoalReached()
    {
        level_transition = true;
        level_outro = true;
        level_intro = false;
        transition_progress = 0;
        fade_script.Reset();
        Debug.Log("Let the level transition begin!");
    }

    public void NextLevel()
    {
        level_outro = false;
        fade_script.Reset();

        AchievementM();
        int bIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (bIndex >= SceneManager.sceneCountInBuildSettings){
            Debug.Log("Congraturations U win tHe Games!?");
        }
        else
        {
            Debug.Log("Start new level");
            undo_stack.Clear();
            LoadLevel(bIndex);
        }
    }

    public void GiveBackControlToPlayer()
    {
        level_transition = false;
        level_outro = false;
        level_intro = false;
        fade_obj.SetActive(false);
    }

    public void ResetLevel()
    {
        reset_fade = 1;
        fade_obj.SetActive(true);
        fade_script.Reset();
        fade_script.StartFade(OUTRO_FADE_SPEED, 1);
    }

    public void ExitToMenu()
    {
        exit_outro = true;
        fade_obj.SetActive(true);
        fade_script.Reset();
        fade_script.StartFade(OUTRO_FADE_SPEED, 1);
    }

    public bool UndoAvailable()
    {
        return undo_stack.Count > 0;
    }

    public void Undo()
    {
        if (UndoAvailable())
        {
            StatePackage state = (StatePackage) undo_stack.Pop();
            state.ResetState();
            ResetElevatorLevel(state.elevator_level);
            state.Destroy();
        }
    }

    public void RecordUndo()
    {
        RecordUndo(character);
    }
    public void RecordUndo(GameObject character)
    {
        StatePackage state = new StatePackage(character);
        //Debug.Log("Recorded " + character.name + " at pos " + position);

        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        foreach(GameObject box in boxes)
        {
            state.AddObject(box);
        }

        state.elevator_level = elevator_level;

        undo_stack.Push(state);
    }

    public System GetSystem()
    {
        return system;
    }

    public void Pause(){
        Debug.Log("Menu");
        pause_menu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume(){
        Debug.Log("Resume");
        pause_menu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void LoadLevel(int n){
        //Debug.Log("Load level: " + SceneManager.GetSceneByBuildIndex(n).name);
        Debug.Log("Load level with index: " + n);
        SceneManager.LoadScene(n);
    }

    public void AchievementM()
    {
        int b_index = SceneManager.GetActiveScene().buildIndex;
        if(UndoAvailable())
        {
            Debug.Log(b_index);
            achievement_script.AddSteps(b_index, undo_stack.Count);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene was loaded: " + SceneManager.GetActiveScene().name);

        if (IsExitOutro()) // If the level is about to exit, nothing of the normal stuff should happen
        {
            fade_obj.SetActive(false);
            exit_outro = false;
            GameObject.Find("MainMenu").GetComponent<GUI>().MenuIntro();
            Destroy(gameObject);
            return;
        }

        character = GameObject.Find("Character");
        c_script = character.GetComponent<CharacterControllerScript>();
        elevator_level = (character.GetComponent<Rigidbody>().position.y > 0.5f) ? 1 : 0;
        elevators = GameObject.FindGameObjectsWithTag("Elevator");

        if (IsResetFade()) return; // If a reset is in progress, all fading and intro stuff is aborted

        level_intro = true;
        transition_progress = 0;

        fade_script = fade_obj.GetComponent<Fade>();
        fade_obj.SetActive(true);
        fade_script.StartFade(INTRO_FADE_SPEED, -1);

        achievement_script = achievement.GetComponent<AchievementSystem>();
    }

    public bool IsLevelTransition()
    {
        return level_transition;
    }
    public bool IsLevelIntro()
    {
        return level_intro;
    }
    public bool IsLeveOutro()
    {
        return level_outro;
    }
    public bool IsResetFade()
    {
        return reset_fade != 0;
    }
    public bool IsExitOutro()
    {
        return exit_outro;
    }

    public bool ShowBeam()
    {
        return ( ( IsLevelIntro() && fade_script.fade_done ) || IsLeveOutro() );
    }
}
