using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goodbye : MonoBehaviour
{
    #if UNITY_IOS
    void Start()
    {
        this.gameObject.SetActive(false);
    }
    #endif
}
