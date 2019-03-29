using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPusher
{
    void Stop(Vector3 position);
    bool CollisionCheckInFront(Vector3 direction);
}
