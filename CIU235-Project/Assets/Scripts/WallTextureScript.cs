using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTextureScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Material m = gameObject.GetComponent<MeshRenderer>().material;
        Transform t = gameObject.GetComponent<Transform>();
        m.mainTextureScale = new Vector2(t.localScale.x, t.localScale.z);
        Debug.Log("Scale: " + t.localScale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
