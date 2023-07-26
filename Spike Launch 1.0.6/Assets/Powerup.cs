using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    public bool og;
    public int id;

    public GameObject GameControlObject;
    public GameControl GameControl;

    public Sprite powerupCrateception; // 1
    public Sprite powerupGrow; // 2
    public Sprite powerupShield; // 3
    public Sprite powerupShockwave; // 4
    public Sprite powerupShooter; // 5
    public Sprite powerupSpeed; // 6

    public Sprite powerupBubble; // 7
    public Sprite powerupRocket; // 8

    public string map;
    public int dir;

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
            id = GameControl.powerupOpportunity;
            if (id == 5 && map == "Space") id = 8;
            Sprite[] spriteList = {powerupCrateception, powerupGrow, powerupShield, powerupShockwave, powerupShooter, powerupSpeed, powerupBubble, powerupRocket};
            GetComponent<SpriteRenderer>().sprite = spriteList[id - 1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameControl.inGame && GameControl.before == 0 && !og) {
            float xSpeed = 0f;
            float ySpeed;
            if (GameControl.superspeed) ySpeed = 0f - GameControl.obstacleSpeed * Time.deltaTime * 1.25f;
            else if (GameControl.subspeed) ySpeed = 0f - GameControl.obstacleSpeed * Time.deltaTime * 0.75f;
            else ySpeed = 0f - GameControl.obstacleSpeed * Time.deltaTime;
            dir = GameControl.direction;
            if (dir == 2 || dir == 4) {
                xSpeed = ySpeed;
                ySpeed = 0f;
                if (dir == 4) xSpeed = 0f - xSpeed;
            }
            if (dir == 3) ySpeed = 0f - ySpeed;
            if (dir != 0) transform.Translate(xSpeed, ySpeed, 0f, Space.World);
        }
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
            GameControl.EnablePowerup(id);
            GameControl.powerups++;
            if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameControl.PowerupSource.Play();
            Destroy(this.gameObject);
        }
    }
}
