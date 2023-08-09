using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveStart : MonoBehaviour
{

    public string map;
    //public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShockUp() {
        if (map == "Ocean") transform.position = new Vector3(0f, 7f, 0f);
        else transform.position = new Vector3(0f, -7f, 0f);
        GetComponent<Animation>().Play();
    }
}
