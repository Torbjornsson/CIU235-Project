using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPusher
{
    void Stop(Vector3 position);
    //void SetInFront(GameObject in_front, IPusher in_front_script);
    bool CollisionCheckInFront(Vector3 direction);
}
