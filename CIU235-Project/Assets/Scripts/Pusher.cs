using UnityEngine;

public abstract class Pusher : MonoBehaviour
{
    protected Rigidbody rb;
    protected bool moving;

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
                BoxPushedScript box_script = hit.collider.gameObject.GetComponent<BoxPushedScript>();
                collision |= box_script.CollisionCheckInFront(direction);
                if (!collision)
                {
                    box_script.Pushed(gameObject);
                    gameObject.GetComponent<CharacterControllerScript>().pushing = true;
                }
            }

            //Debug.Log("Collision registered by [" + gameObject.name + "]: " + hit.collider.gameObject.name);
        }

        return collision;
    }

    // Simple getter
    public bool IsMoving()
    {
        return moving;
    }
}
