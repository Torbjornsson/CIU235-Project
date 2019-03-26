using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{

    private Rigidbody rb;
    public float speed;
    public float grid_size;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.GetAxis("Vertical") + " - " + Input.GetAxis("Horizontal"));
        Vector3 pos = rb.position;
        pos.x += Input.GetAxis("Horizontal") * speed;
        pos.z += Input.GetAxis("Vertical") * speed;
        rb.MovePosition(pos);

        //Debug.Log("");
    }
}
