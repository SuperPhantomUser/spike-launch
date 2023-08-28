using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public int id;
    public bool og;

    public GameObject GameControlObject;
    public GameControl GameControl;

    public string map;
    public int dir;
    public float rotate;

    public GameObject Break;
    public AudioSource BreakSource;

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
            rotate = Random.Range(1, 3);
            if (rotate == 1) rotate = 180f;
            else rotate = -180f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameControl.inGame && GameControl.before == 0 && !og) {
            float xSpeed = 0f;
            if (id == 4) xSpeed = 2f;
            if (id == 5) xSpeed = -1f;
            xSpeed = xSpeed * Time.deltaTime;
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
        if (id == 2) transform.Rotate(0f, 0f, rotate * Time.deltaTime);
        if (id == 3) transform.Rotate(0f, 0f, rotate * Time.deltaTime * 0.5f);
        if (dir == 2) {
            if (transform.position.x < -8f) Missed();
        } else if (dir == 4) {
            if (transform.position.x > 8f) Missed();
        } else if (dir == 3) {
            if (transform.position.y > 6.5f) Missed();
        } else {
            if (transform.position.y < -6.5f) Missed();
        }
    }

    void Missed() {
        Destroy(this.gameObject);
        if (id == 1) GameControl.missed++;
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Spike") {
            if (id == 1 || id == 4) {
                if (id == 1) {
                    GameControl.score++;
                    GameControl.crates++;
                    GameControl.obstaclePoints++;
                    if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameControl.BreakSource.Play();
                    GameControl.timer.Stop();
                }
                else {
                    GameControl.score += 2;
                    GameControl.obstaclePoints += 2;
                    if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameControl.FishSource.Play();
                }
                GameControl.powerupBalancer++;
                if (id == 1) {
                    GameObject NewBreak = Instantiate(Break, transform.position, Quaternion.identity);
                    NewBreak.SetActive(true);
                }
                Destroy(this.gameObject);
            }
            if (id == 2) {
                if (GameControl.powerup == 3 || GameControl.powerup == 8) {
                    GameObject NewBreak = Instantiate(Break, transform.position, Quaternion.identity);
                    if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameControl.ClangSource.Play();
                    NewBreak.SetActive(true);
                }
                else {
                    if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameControl.ClangSource.Play();
                    GameControl.GameOver();
                }
                Destroy(this.gameObject);
            }
            if (id == 3) {
                GameObject NewBreak = Instantiate(Break, transform.position, Quaternion.identity);
                NewBreak.SetActive(true);
                if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameControl.ZapSource.Play();
                GameControl.GameOver();
                Destroy(this.gameObject);
            }
            if (id == 5) {
                if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameControl.ZapSource.Play();
                if (GameControl.powerup != 3) GameControl.GameOver();
                Destroy(this.gameObject);
            }
        }
        if (col.gameObject.tag == "Shockwave") {
            if (id == 1) {
                GameControl.score++;
                GameControl.obstaclePoints++;
                GameObject NewBreak = Instantiate(Break, transform.position, Quaternion.identity);
                NewBreak.SetActive(true);
                if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameControl.BreakSource.Play();
                GameControl.timer.Stop();
                Destroy(this.gameObject);
            }
            if (id == 2) {
                GameObject NewBreak = Instantiate(Break, transform.position, Quaternion.identity);
                NewBreak.SetActive(true);
                if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameControl.ClangSource.Play();
                Destroy(this.gameObject);
            }
            if (id == 3) {
                GameObject NewBreak = Instantiate(Break, transform.position, Quaternion.identity);
                NewBreak.SetActive(true);
                if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameControl.ZapSource.Play();
                Destroy(this.gameObject);
            }
            if (id == 4 || id == 5) {
                if (id == 4) {
                    GameControl.score += 2;
                    GameControl.obstaclePoints++;
                    if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameControl.FishSource.Play();
                }
                Destroy(this.gameObject);
            }
        }
        if (col.gameObject.tag == "Bullet") {
            if (id == 1) {
                GameControl.score++;
                GameControl.obstaclePoints++;
                GameObject NewBreak = Instantiate(Break, transform.position, Quaternion.identity);
                NewBreak.SetActive(true);
                if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameControl.BreakSource.Play();
                GameControl.timer.Stop();
                Destroy(this.gameObject);
            }
            if (id == 2) {
                GameObject NewBreak = Instantiate(Break, transform.position, Quaternion.identity);
                NewBreak.SetActive(true);
                if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameControl.ClangSource.Play();
                Destroy(this.gameObject);
            }
            if (id == 4) {
                GameControl.score += 2;
                GameControl.obstaclePoints++;
                if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameControl.FishSource.Play();
                Destroy(this.gameObject);
            }
            if (id == 5) {
                if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameControl.ZapSource.Play();
                Destroy(this.gameObject);
            }
        }
    }
}
