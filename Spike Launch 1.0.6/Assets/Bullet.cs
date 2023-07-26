using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    bool og;
    public string map;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.y == 0f) og = true;
        else og = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!og) {
            if (map == "Ocean") transform.Translate(0f, -8f * Time.deltaTime, 0f);
            else transform.Translate(0f, 8f * Time.deltaTime, 0f);
        }
        if (map == "Ocean") {
            if (transform.position.y <= -4f) Destroy(this.gameObject);
        } else {
            if (transform.position.y >= 4f) Destroy(this.gameObject);
        }
    }
}
