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
        rb.SweepTest(direction, out hit);
        if (hit.collider != null && hit.distance < Utility.GRID_SIZE)
        {
            collision |= (hit.collider.gameObject.tag == "Wall");

            if (hit.collider.gameObject.tag == "Box")
            {
                if (gameObject.name == "Character")
                    collision |= hit.collider.gameObject.GetComponent<BoxPushedScript>().CollisionCheckInFront(direction);
                else
                    collision = true;
            }
        }

        return collision;
    }
}
