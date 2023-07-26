using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{

    public bool og;

    public GameObject GameControlObject;
    public GameControl GameControl;

    public string map;
    public int dir;
    public float rotate;

    public GameObject Break;

    // Start is called before the first frame update
    void Start()
    {
        GameControl = GameControlObject.GetComponent<GameControl>();
        map = GameControl.map;
        dir = GameControl.direction;
        if (transform.position.y == 0f) og = true;
        else {
            og = false;
            if (dir == 1) transform.position = new Vector3(Random.Range(-2f, 2f), 6f, 0f);
            else if (dir == 2) transform.position = new Vector3(8f, Random.Range(-4f, 4f), 0f);
            else if (dir == 3) transform.position = new Vector3(Random.Range(-2f, 2f), -6f, 0f);
            else transform.position = new Vector3(-8f, Random.Range(-4f, 4f), 0f);
            rotate = 360f;
            GameControl.MeteorSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameControl.inGame && GameControl.before == 0 && !og) {
            float xSpeed = 0f;
            float ySpeed;
            ySpeed = 0f - GameControl.obstacleSpeed * Time.deltaTime * 1.25f;
            if (ySpeed > 17.5f) ySpeed = 17.5f;
            dir = GameControl.direction;
            if (dir == 2 || dir == 4) {
                xSpeed = ySpeed;
                ySpeed = 0f;
                if (dir == 4) xSpeed = 0f - xSpeed;
            }
            if (dir == 3) ySpeed = 0f - ySpeed;
            transform.Translate(xSpeed, ySpeed, 0f, Space.World);
        }
        transform.Rotate(0f, 0f, rotate * Time.deltaTime);
        if (dir == 2) {
            if (transform.position.x < -8f) Destroy(this.gameObject);
        } else if (dir == 4) {
            if (transform.position.x > 8f) Destroy(this.gameObject);
        } else if (dir == 3) {
            if (transform.position.y > 6.5f) Destroy(this.gameObject);
        } else {
            if (transform.position.y < -6.5f) Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Spike") {
            if (GameControl.powerup == 8) {
                if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameControl.CrushSource.Play();
            }
            else GameControl.GameOver();
            Destroy(this.gameObject);
        }
    }
}
