using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFlicker : MonoBehaviour
{

    public float frame;
    public TMPro.TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        frame = 0f;
        scoreText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        frame += Time.deltaTime;
        if (frame >= 0.65f) {
            if (scoreText.enabled) scoreText.enabled = false;
            else scoreText.enabled = true;
            frame = 0f;
        }
    }
}
