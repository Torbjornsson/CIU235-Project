
using System.Collections;
using UnityEngine;

public class StatePackage
{
    private Hashtable objects = new Hashtable();
    public int elevator_level;

    public StatePackage()
    {
        elevator_level = 0;
    }
    public StatePackage(GameObject obj) : this()
    {
        AddObject(obj);
    }
    public StatePackage(GameObject obj, MiniPackage mp) : this()
    {
        AddObject(obj, mp);
    }

    public void AddObject(GameObject obj)
    {
        MiniPackage mp = new MiniPackage(obj);
        AddObject(obj, mp);
    }
    public void AddObject(GameObject obj, MiniPackage mp)
    {
        objects.Add(obj, mp);
    }

    public void ResetState()
    {
        foreach (GameObject obj in objects.Keys)
        {
            MiniPackage mp = (MiniPackage)objects[obj];
            Vector3 pos = mp.position;
            Quaternion rot = mp.rotation;
            obj.GetComponent<Rigidbody>().MovePosition(pos);
            obj.GetComponent<Rigidbody>().MoveRotation(rot);
        }
    }

    public void Destroy()
    {
        objects.Clear();
    }
}

public class MiniPackage
{
    public GameObject game_object;
    public Vector3 position;
    public Quaternion rotation;

    public MiniPackage(GameObject go){
        game_object = go;
        Rigidbody rb = go.GetComponent<Rigidbody>();
        position = rb.position;
        rotation = rb.rotation;
    }
}
