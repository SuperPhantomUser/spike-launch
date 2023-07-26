using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{

    public GameObject Asterisk;
    public Animation Anim;
    public int times;
    public bool canDo;

    public Preview Preview;
    public HomeControl Control;

    public AudioSource BounceSource;
    public AudioSource FallSource;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animation>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Boing() {
        Anim.Rewind();
        Anim.Play();
        if (Control.playSound) BounceSource.Play();
        times++;
        if (times == 20 && canDo) {
            Asterisk.SetActive(true);
            if (Control.playSound) FallSource.Play();
            Preview.AsteriskGot();
        }
    }
}
