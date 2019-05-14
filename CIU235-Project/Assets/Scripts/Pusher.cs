using UnityEngine;

public abstract class Pusher : MonoBehaviour
{
    protected bool moving;
    protected bool falling;

    protected Vector3 next_pos;
    protected Vector3 direction;

    public Rigidbody rb;
    protected GameMasterScript game_master_script;

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
        CharacterControllerScript char_script = null;

        RaycastHit hit = new RaycastHit();
        Vector3 pos = rb.position;

        if (gameObject.tag == "Elevator")
        {
            if (direction.y > 0)    pos.y -= 0.1f;
            else                    pos.y += 1.1f;
        }
        else
        {
            pos.y += 0.1f;
        }

        Physics.Raycast(pos, direction,out hit, Utility.GRID_SIZE);
        if (hit.collider != null && hit.distance < Utility.GRID_SIZE)
        {
            // Nothing can move through walls
            collision |= (hit.collider.gameObject.tag == "Wall");

            // Boxes can't move through goals
            collision |= (gameObject.tag == "Box" && hit.collider.gameObject.tag == "Goal");

            // Boxes can't move through other boxes
            collision |= (gameObject.tag == "Box" && hit.collider.gameObject.tag == "Box");

            // Boxes or character don't fall through elevators
            collision |= ((gameObject.tag == "Box" || gameObject.name == "Character") && hit.collider.gameObject.tag == "Elevator");

            // Character can push a box, thus being able to continue move,
            // IF the box does not collide, in turn
            if (gameObject.name == "Character" && hit.collider.gameObject.tag == "Box")
            {
                box_script = hit.collider.gameObject.GetComponent<BoxPushedScript>();
                if (!box_script.falling) box_script.CheckForFall();
                collision |= box_script.IsFalling();
                collision |= box_script.CollisionCheckInFront(direction);
                //Debug.Log("Character vs box, box falling: "+box_script.IsFalling()+", box pos: "+hit.rigidbody.position+", character pos: "+rb.position);
                if (!collision)
                {
                    box_script.Pushed(gameObject);
                    gameObject.GetComponent<CharacterControllerScript>().pushing = true;
                }
            }

            if (gameObject.tag == "Elevator")
            {
                if (hit.collider.gameObject.tag == "Box")
                {
                    box_script = hit.collider.gameObject.GetComponent<BoxPushedScript>();
                    box_script.Pushed(gameObject);
                }
                else if (hit.collider.gameObject.name == "Character")
                {
                    char_script = hit.collider.gameObject.GetComponent<CharacterControllerScript>();
                    char_script.Pushed(gameObject);
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

    // Simple getter
    public bool IsFalling()
    {
        return falling;
    }

    public virtual void Pushed(GameObject pusher) { }


    public virtual void SetDir(float dir_x, float dir_y, float dir_z)
    {
        direction = new Vector3(dir_x, dir_y, dir_z);
    }

    public Vector3 GetDir()
    {
        return direction;
    }

    public void SetNextPos(Vector3 cur_pos, Vector3 dir)
    {
        SetNextPos(cur_pos, dir.x, dir.y, dir.z);
    }

    public void SetNextPos(Vector3 cur_pos, float dir_x, float dir_y, float dir_z)
    {
        next_pos = new Vector3(cur_pos.x + Utility.GRID_SIZE * dir_x, cur_pos.y + Utility.GRID_SIZE * dir_y, cur_pos.z + Utility.GRID_SIZE * dir_z);
        next_pos = Utility.GetGridPos(next_pos);
    }

    public void CheckForFall()
    {
        falling = false;

        if (!CollisionCheckInFront(Vector3.down))
        {
            if (moving)
            {
                Stop(next_pos);
            }
            if (!moving)
            {
                SetDir(0, -1, 0);
                moving = true;
                SetNextPos(rb.position, direction);
            }

            falling = true;
        }
    }
}
