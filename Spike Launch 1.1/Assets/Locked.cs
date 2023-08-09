using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locked : MonoBehaviour
{

    public int id;

    public GameObject RealButton;
    public GameObject Locked1;
    public GameObject Locked2;
    
    public Preview Preview;

    // Start is called before the first frame update
    void Start()
    {
        if (id == 1) {
            if (Preview.stormUnlocked) {
                RealButton.SetActive(true);
                this.gameObject.SetActive(false);
            } else {
                Locked1.SetActive(false);
                Locked2.SetActive(true);
            }
        } else if (id == 2) {
            if (Preview.oceanUnlocked) {
                RealButton.SetActive(true);
                this.gameObject.SetActive(false);
            } else if (Preview.stormUnlocked) {
                Locked1.SetActive(false);
                Locked2.SetActive(true);
            } else {
                Locked1.SetActive(true);
                Locked2.SetActive(false);
            }
        } else {
            if (Preview.spaceUnlocked) {
                RealButton.SetActive(true);
                this.gameObject.SetActive(false);
            } else if (Preview.oceanUnlocked) {
                Locked1.SetActive(false);
                Locked2.SetActive(true);
            } else {
                Locked1.SetActive(true);
                Locked2.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
