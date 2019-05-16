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

    private string button_reset;
    private string button_menu;
    private string button_accept;
    private string button_cancel;

    public Stack undo_stack = new Stack();
    //public Stack undoStackC = new Stack();
    bool level_win;

    private int elevator_level;
    GameObject[] elevators;

    public float EPSILON = 0.00001f;

    public Canvas pause_menu;

    private GameObject character;
    private CharacterControllerScript c_script;

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
        level_win = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Scene was loaded: " + SceneManager.GetActiveScene().name);

        if ((Input.GetButtonDown(button_reset)))
        {
            ResetLevel();
        }

        if ((Input.GetButtonDown(button_menu)) && SceneManager.GetActiveScene().buildIndex > 0)
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
            //Debug.Log("ACCEPT");
            ChangeElevatorLevel();
        }

        if ((Input.GetButtonDown(button_cancel)))
        {
            Debug.Log("CANCEL");
        }

        CheckElevatorTriggering();
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
        //CharacterControllerScript c_script = character.GetComponent<CharacterControllerScript>();
        if (!c_script.IsMoving() || c_script.IsFalling())
        {
            //GameObject[] elevators = GameObject.FindGameObjectsWithTag("Elevator");
            //GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
            bool ok_to_move = true;
            foreach (GameObject elevator in elevators)
            {
                ok_to_move &= !elevator.GetComponent<Elevator>().IsMoving();
            }

            if (ok_to_move)
            {
                elevator_level = (elevator_level + 1) % 2;
                Debug.Log("Change elevator level to: " + elevator_level);

                foreach (GameObject elevator in elevators)
                {
                    elevator.GetComponent<Elevator>().MoveToLevel(elevator_level);
                }
            }
        }
    }

    private void ResetElevatorLevel(int level)
    {
        Debug.Log("Reset elevator level! Current: " + elevator_level + ", should be: " + level);

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

    public void LevelWin()
    {
        level_win = true;
        Debug.Log("Start new level");
        int bIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (bIndex >= SceneManager.sceneCountInBuildSettings){
            Debug.Log("Congraturations U win tHe Games!?");
        }
        else
        {
            undo_stack.Clear();
            SceneManager.LoadScene(bIndex);
        }
        
    }

    public void ResetLevel()
    {
        Debug.Log("Reset level");
        undo_stack.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Debug.Log("Quit application");
        Application.Quit();
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
            Debug.Log("Undo !");
            state.ResetState();
            ResetElevatorLevel(state.elevator_level);
            state.Destroy();
        }
    }

    public void RecordUndo()
    {
        //GameObject character = GameObject.FindWithTag("Player");
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
        Debug.Log(SceneManager.GetSceneByBuildIndex(n).name);
        SceneManager.LoadScene(n);
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

        character = GameObject.Find("Character");
        c_script = character.GetComponent<CharacterControllerScript>();
        elevator_level = (character.GetComponent<Rigidbody>().position.y > 0.5f) ? 1 : 0;
        //Debug.Log("Elevator level: "+elevator_level);
        elevators = GameObject.FindGameObjectsWithTag("Elevator");
    }
}
