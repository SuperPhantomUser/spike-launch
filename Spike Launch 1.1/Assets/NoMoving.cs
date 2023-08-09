using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMoving : MonoBehaviour
{

    public HomeControl control;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Controller() {
        control.moving = false;
        yield return new WaitForSeconds(0);
    }
}
