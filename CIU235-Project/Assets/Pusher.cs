using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pusher : MonoBehaviour
{
    protected Rigidbody rb;
    protected bool moving;

    //abstract public void Stop(Vector3 position);
    public void Stop(Vector3 position)
    {
        moving = false;
        rb.MovePosition(position);
    }

    //abstract public bool CollisionCheckInFront(Vector3 direction);
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
                collision |= hit.collider.gameObject.GetComponent<BoxPushedScript>().CollisionCheckInFront(direction);
            }
        }

        return collision;
    }
}
