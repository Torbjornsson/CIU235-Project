using UnityEngine;

public abstract class Pusher : MonoBehaviour
{
    protected Rigidbody rb;
    protected bool moving;

    public void Stop(Vector3 position)
    {
        moving = false;
        rb.MovePosition(position);
    }

    public bool CollisionCheckInFront(Vector3 direction)
    {
        bool collision = false;

        RaycastHit hit = new RaycastHit();
        Physics.Raycast(rb.position, direction,out hit, Utility.GRID_SIZE);
        if (hit.collider != null && hit.distance < Utility.GRID_SIZE)
        {
            collision |= (hit.collider.gameObject.tag == "Wall");

            if (hit.collider.gameObject.tag == "Box")
            {
                if (gameObject.name == "Character")
                {
                    BoxPushedScript box_script = hit.collider.gameObject.GetComponent<BoxPushedScript>();
                    collision |= box_script.CollisionCheckInFront(direction);
                    if (!collision)
                    {
                        box_script.Pushed(gameObject);
                    }
                }
                else
                {
                    collision = true;
                }
            }
        }

        return collision;
    }
}
