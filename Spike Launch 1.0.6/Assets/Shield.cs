using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    GameObject GoTo;
    public GameControl Control;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = GoTo.transform.position;
    }

    public void LoadGoTo(GameObject obj) {
        GoTo = obj;
    }

    public IEnumerator GoodBye() {
        this.gameObject.SetActive(false);
        Control.powerup = 0;
        yield return new WaitForSeconds(0);
    }
}
