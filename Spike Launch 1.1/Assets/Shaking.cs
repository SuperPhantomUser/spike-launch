using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaking : MonoBehaviour
{

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.4f;
    private float dampingSpeed = 1f;
    Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = new Vector3(0f, 0f, -10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeDuration > 0f) {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else transform.position = initialPosition;
    }

    public void Shake() {
        shakeDuration = 8.5f;
    }
}
