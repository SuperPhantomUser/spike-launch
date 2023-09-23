using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class Load : MonoBehaviour
{

    public GameObject GameControlObject;
    public GameControl GameControl;
    public GameObject ShieldObj;
    public GameObject BulletObj;
    public GameObject Cannon;
    public GameObject Speedlines;
    public GameObject LaunchText;
    public GameObject Ocean;
    public Camera Cam;

    public float rotatingSpeed;

    public float frame;
    public bool shooter;

    public Sprite spriteDefault;
    public Sprite spriteHurt;
    public Sprite spritePowered;
    public Sprite spriteHit;

    public Sprite bubble1;
    public Sprite bubble2;
    public Sprite bubble3;

    public int myID;

    public AudioSource WhooshSource;
    public AudioSource SplashSource;
    public AudioSource MusicSource;
    public AudioSource MusicLoop;
    public bool looped;

    public bool tapping;

    // Start is called before the first frame update
    void Start()
    {
        looped = false;
        tapping = false;
        GameControl = GameControlObject.GetComponent<GameControl>();
        if (GameControl.spikeID != myID) Destroy(this.gameObject);
        if (GameControl.spikeID == myID) GameControl.Spike = gameObject;
        GetComponent<SpriteRenderer>().sprite = spriteDefault;
        transform.position = new Vector3(0f, -3f, 0f);
        ShieldObj = transform.parent.gameObject.GetComponent<Stuff>().ShieldObj;
        BulletObj = transform.parent.gameObject.GetComponent<Stuff>().BulletObj;
        Cannon = transform.parent.gameObject.GetComponent<Stuff>().Cannon;
        Speedlines = transform.parent.gameObject.GetComponent<Stuff>().Speedlines;
        LaunchText = transform.parent.gameObject.GetComponent<Stuff>().LaunchText;
        WhooshSource = transform.parent.gameObject.GetComponent<Stuff>().WhooshSource;
        SplashSource = transform.parent.gameObject.GetComponent<Stuff>().SplashSource;
        MusicSource = transform.parent.gameObject.GetComponent<Stuff>().MusicSource;
        MusicLoop = transform.parent.gameObject.GetComponent<Stuff>().MusicLoop;
        if (transform.parent.gameObject.GetComponent<Stuff>().Ocean) Ocean = transform.parent.gameObject.GetComponent<Stuff>().Ocean;
        frame = 0f;
        Cam = Camera.main;
        TouchSimulation.Enable();
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += get_touch_details;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += get_touch_details;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += seeya;
    }

    void OnDestroy()
    {
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= get_touch_details;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= get_touch_details;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= seeya;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameControl.inGame) {
            if (GameControl.powerup == 8) {
                if (GameControl.direction == 1) transform.eulerAngles = new Vector3(0f, 0f, 0f);
                else if (GameControl.direction == 2) transform.eulerAngles = new Vector3(0f, 0f, -90f);
                else if (GameControl.direction == 3) transform.eulerAngles = new Vector3(0f, 0f, 180f);
                else transform.eulerAngles = new Vector3(0f, 0f, 90f);
            }
            else if (myID != 6) transform.Rotate(0f, 0f, 0f - (rotatingSpeed * Time.deltaTime), Space.Self);
        }
        if (Input.GetMouseButton(0) && GameControl.inGame && Time.timeScale == 1) {
            if (GameControl.before == 1) StartCoroutine(Launch());
            else if (GameControl.before == 0) GoTo(Input.mousePosition);
        }
        if (shooter) {
            frame += Time.deltaTime;
            if (frame >= 0.2f) {
                Instantiate(BulletObj, transform.position, Quaternion.identity);
                frame = 0f;
            }
        }
    }

    void get_touch_details(Finger fin) {
        if (GameControl.before == 1 && Time.timeScale == 1) StartCoroutine(Launch());
        else if (GameControl.before == 0 && Time.timeScale == 1) GoTo(new Vector3(fin.screenPosition.x, fin.screenPosition.y, 0f));
    }

    void seeya(Finger fin) {
        if (tapping) tapping = false;
    }

    void GoTo(Vector3 screen) {
        if (GameControl.inGame) {
            Vector3 worldPoint = Cam.ScreenToWorldPoint(screen);
            if (GameControl.map == "Space") {
                float x = worldPoint.x;
                if (x > 2f) x = 2f;
                if (x < -2f) x = -2f;
                float y = worldPoint.y;
                if (y > 3.5f) y = 3.5f;
                if (y < -3.5f) y = -3.5f;
                if (y < 4f || !tapping) {
                    tapping = true;
                    transform.position = new Vector3(x, y, 0f);
                }
                
                if (transform.position.y > 0f) GameControl.overY = true;
            }
            else {
                float y = -3f;
                if (Ocean) y = 3f;
                if (worldPoint.y < 3.5f || !tapping) {
                    tapping = true;
                    if (worldPoint.x > 2f) transform.position = new Vector3(2f, y, 0f);
                    else if (worldPoint.x < -2f) transform.position = new Vector3(-2f, y, 0f);
                    else transform.position = new Vector3(worldPoint.x, y, 0f);
                }
            }
        }
    }

    public void Grow(bool enabled) {
        if (enabled) {
            if (GameControl.inGame) GetComponent<SpriteRenderer>().sprite = spritePowered;
            transform.localScale = new Vector3(transform.localScale.x * 1.45f, transform.localScale.x * 1.45f, 1f);
        }
        else {
            if (GameControl.inGame) GetComponent<SpriteRenderer>().sprite = spriteDefault;
            transform.localScale = new Vector3(transform.localScale.x / 1.45f, transform.localScale.x / 1.45f, 1f);
        }
    }

    public void Shield(bool enabled) {
        if (enabled) {
            ShieldObj.SetActive(true);
            ShieldObj.GetComponent<SpriteRenderer>().enabled = true;
            ShieldObj.GetComponent<Shield>().LoadGoTo(this.gameObject);
        }
        else {
            ShieldObj.GetComponent<Animation>().Play();
        }
    }

    public void Shooter(bool enabled) {
        shooter = enabled;
        if (enabled && GameControl.inGame) GetComponent<SpriteRenderer>().sprite = spritePowered;
        else if (GameControl.inGame) GetComponent<SpriteRenderer>().sprite = spriteDefault;
    }

    public void Rocket(bool enabled) {
        if (enabled && GameControl.inGame) GetComponent<SpriteRenderer>().sprite = GameControl.rocket;
        else if (GameControl.inGame) StartCoroutine(RocketChange());
    }

    IEnumerator Launch() {
        GameControl.before = 2;
        LaunchText.SetActive(false);
        Cannon.GetComponent<Animator>().enabled = true;
        if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) WhooshSource.Play();
        if (PlayerPrefs.GetInt("MusicVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) {
            MusicSource.Play();
            StartCoroutine(Music());
        }
        if (!Ocean) Speedlines.SetActive(true);
        GetComponent<Animator>().enabled = true;
        if (Ocean) Ocean.GetComponent<Animator>().enabled = true;
        if (Ocean) {
            yield return new WaitForSeconds(0.7f);
            if (myID == 6) GetComponent<Animation>().Play("FlipDown");
            yield return new WaitForSeconds(0.3f);
            Speedlines.SetActive(true);
            yield return new WaitForSeconds(2f);
        }
        else yield return new WaitForSeconds(2.5f);
        GetComponent<Animator>().enabled = false;
        GameControl.before = 0;
        GameControl.timer.Start();
        StartCoroutine(GameControl.TimerTime());
    }

    IEnumerator Music() {
        yield return new WaitUntil(() => MusicSource.isPlaying == false);
        looped = true;
        if (GameControl.inGame) MusicLoop.Play();
    }

    IEnumerator Splish() {
        SplashSource.Play();
        yield return new WaitForSeconds(0);
    }

    IEnumerator RocketChange() {
        GetComponent<SpriteRenderer>().color = new Color(0.8396226f, 0.645242f, 0.2724813f, 1f);
        yield return new WaitForSeconds(1);
        if (GameControl.inGame) GetComponent<SpriteRenderer>().sprite = spriteDefault;
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        GameControl.ResetPowerup();
    }

    public void Flip(string anim) {
        GetComponent<Animation>().Play(anim);
    }

    public IEnumerator BubblePop()
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        GetComponent<SpriteRenderer>().sprite = bubble1;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().sprite = bubble2;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().sprite = bubble3;
    }
}
