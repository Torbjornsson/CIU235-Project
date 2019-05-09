
using System.Collections;
using UnityEngine;

public class StatePackage
{
    private Hashtable objects = new Hashtable();

    public StatePackage(GameObject obj)
    {
        AddObject(obj);
    }
    public StatePackage(GameObject obj, Vector3 pos)
    {
        AddObject(obj, pos);
    }

    public void AddObject(GameObject obj)
    {
        AddObject(obj, obj.GetComponent<Rigidbody>().position);
    }
    public void AddObject(GameObject obj, Vector3 pos)
    {
        objects.Add(obj, pos);
    }

    public void ResetState()
    {
        foreach (GameObject obj in objects.Keys)
        {
            obj.GetComponent<Rigidbody>().MovePosition((Vector3)objects[obj]);
        }
    }
}
