using UnityEngine;

public abstract class Pusher : MonoBehaviour
{
    public Rigidbody rb;
    protected bool moving;
    protected GameMasterScript gameMasterScript;

    // Stops moving at a certain position
    public virtual void Stop(Vector3 position)
    {
        moving = false;
        rb.MovePosition(position);
    }

    // Checks for collision in front of pusher, and if there's an obstacle, behaves accordingly
    public bool CollisionCheckInFront(Vector3 direction)
    {
        bool collision = false;
        BoxPushedScript box_script = null;
        GameObject box = null;

        RaycastHit hit = new RaycastHit();
        Physics.Raycast(rb.position, direction,out hit, Utility.GRID_SIZE);
        if (hit.collider != null && hit.distance < Utility.GRID_SIZE)
        {
            // Nothing can move through walls
            collision |= (hit.collider.gameObject.tag == "Wall");

            // Boxes can't move through goals
            collision |= (gameObject.tag == "Box" && hit.collider.gameObject.tag == "Goal");

            // Boxes can't move through other boxes
            collision |= (gameObject.tag == "Box" && hit.collider.gameObject.tag == "Box");

            // Character can push a box, thus being able to continue move,
            // IF the box does not collide, in turn
            if (gameObject.name == "Character" && hit.collider.gameObject.tag == "Box")
            {
                box_script = hit.collider.gameObject.GetComponent<BoxPushedScript>();
                collision |= box_script.CollisionCheckInFront(direction);
                if (!collision)
                {
                    box_script.Pushed(gameObject);
                    gameObject.GetComponent<CharacterControllerScript>().pushing = true;
                    box = hit.collider.gameObject;
                }
            }

            //Debug.Log("Collision registered by [" + gameObject.name + "]: " + hit.collider.gameObject.name);
        }

        // Registering undo-states when needed
        if (gameObject.name == "Character" && !collision) gameMasterScript.RecordUndo(gameObject, rb.position);
        if (box != null) gameMasterScript.RecordUndo(box, box_script.rb.position);

        return collision;
    }

    // Simple getter
    public bool IsMoving()
    {
        return moving;
    }
}
