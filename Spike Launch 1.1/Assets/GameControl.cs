using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class GameControl : MonoBehaviour
{

    public string map;  

    public Shaking shakeScript;

    public bool inGame;
    public int before;
    private float frame;
    public bool speedingUp;

    public float frameColor;
    public SpriteRenderer oceanColor;

    public int direction; // 1 = down, 2 = left, 3 = up, 4 = right
    
    public GameObject Crate;
    public GameObject DangerSpike;
    public GameObject ElectricBox;

    public GameObject Fish;
    public GameObject Jelly;
    public GameObject Meteor;

    public GameObject Powerup;
    public GameObject TextObject;
    public TMPro.TextMeshProUGUI scoreText;

    public GameObject Spike;
    public Load SpikeScript;
    public GameObject ShockWave;
    public GameObject ShockWave2;

    public GameObject Speedlines;
    public float speedlinesSpeed;

    public GameObject SpeedUpText;
    public GameObject PowerupText;

    public float obstacleSpeed;
    public float timeBetween;

    public float obstacleSpeedStart;
    public float timeBetweenStart;
    public float secondWait;

    public int score;
    public int obstaclePoints;
    public int powerupBalancer;
    public int pID;
    public int speedUps;

    public int powerupOpportunity;
    public int powerup;
    public bool superspeed;
    public bool subspeed;
    public bool underspeed;
     // 1 = Crateception
     // 2 = Grow
     // 3 = Shield
     // 4 = Shockwave
     // 5 = Shooter
     // 6 = Speed
     // 7 = Bubble (Ocean)
     // 8 = Rocket (Space)

    public int crates;
    public int powerups;
    public int missed;
    public bool overY;

    public int lastHighscore;
    public bool[] lastAchievements;

    private bool highscoreNotif;

    public GameObject PauseMenu;
    public GameObject GameOverMenu;
    public GameObject TutorialMenu;
    public TMPro.TextMeshProUGUI Results;
    public TMPro.TextMeshProUGUI Highscore;
    public TMPro.TextMeshProUGUI NewHighscore;
    public GameObject NotifText;
    public GameObject Notice;

    public LoadingData LoadingData;

    public Sprite rocket;

    public int spikeID;

    public AudioSource WhooshSource;
    public AudioSource GameOverSource;
    public AudioSource PauseSource;
    public AudioSource BreakSource;
    public AudioSource ClangSource;
    public AudioSource ZapSource;
    public AudioSource CrushSource;
    public AudioSource PowerupSource;
    public AudioSource FishSource;
    public AudioSource MeteorSource;
    public AudioSource SpeedSource;

    private string[,,] requirements;
    private int[] previousCurrents;
    private int[] powerupCurrents;
    private int[] powerupGoals;
    private int[] powerupIndexes;

    public Data Data;

    public Stopwatch timer;

    // Start is called before the first frame update
    void Start()
    {
        Data.SpikeData data = Data.GetFromFile();
        spikeID = data.equipped;
        lastHighscore = GetHighscore(SceneManager.GetActiveScene().name);
        lastAchievements = data.achievements;
        inGame = true;
        before = 1;
        speedlinesSpeed = 25f;
        if (PlayerPrefs.GetInt("Relaxed") == 1) {
            obstacleSpeedStart = obstacleSpeedStart / 1.5f;
            timeBetweenStart = timeBetweenStart * 1.5f;
            speedlinesSpeed = speedlinesSpeed / 1.5f;
        }
        obstacleSpeed = obstacleSpeedStart;
        timeBetween = timeBetweenStart;
        frame = 0f;
        frameColor = 0f;
        score = 0;
        obstaclePoints = 0;
        powerupBalancer = 0;
        pID = 0;
        speedUps = 0;
        scoreText = TextObject.GetComponent<TMPro.TextMeshProUGUI>();
        scoreText.text = "0";
        speedingUp = false;
        superspeed = false;
        subspeed = false;
        direction = 1;
        if (map == "Ocean") direction = 3;
        secondWait = 0.3f;
        highscoreNotif = false;

        crates = 0;
        powerups = 0;
        missed = 0;
        overY = false;

        requirements = new string[4, 15, 2] {
            {
                {"lifetime", "300"},
                {"lifetimeClassic", "200"},
                {"lifetimeStorm", "100"},
                {"lifetimeOcean", "200"},
                {"lifetimeSpace", "150"},
                {"scoreClassic", "150"},
                {"scoreStorm", "30"},
                {"scoreOcean", "100"},
                {"scoreSpace", "50"},
                {"lifetimePowerups", "5"},
                {"shield", "1"},
                {"shooter", "1"},
                {"crateception", "1"},
                {"bubble", "1"},
                {"rocket", "1"}
            },
            {
                {"lifetime", "400"},
                {"lifetimeClassic", "250"},
                {"lifetimeStorm", "150"},
                {"lifetimeOcean", "250"},
                {"lifetimeSpace", "200"},
                {"scoreClassic", "150"},
                {"scoreStorm", "50"},
                {"scoreOcean", "125"},
                {"scoreSpace", "75"},
                {"lifetimePowerups", "6"},
                {"shield", "1"},
                {"shooter", "1"},
                {"crateception", "1"},
                {"bubble", "1"},
                {"rocket", "1"}
            },
            {
                {"lifetime", "600"},
                {"lifetimeClassic", "350"},
                {"lifetimeStorm", "250"},
                {"lifetimeOcean", "350"},
                {"lifetimeSpace", "300"},
                {"scoreClassic", "175"},
                {"scoreStorm", "70"},
                {"scoreOcean", "150"},
                {"scoreSpace", "75"},
                {"lifetimePowerups", "7"},
                {"shield", "2"},
                {"shooter", "2"},
                {"crateception", "2"},
                {"bubble", "2"},
                {"rocket", "2"}
            },
            {
                {"lifetime", "800"},
                {"lifetimeClassic", "450"},
                {"lifetimeStorm", "350"},
                {"lifetimeOcean", "450"},
                {"lifetimeSpace", "400"},
                {"scoreClassic", "175"},
                {"scoreStorm", "90"},
                {"scoreOcean", "175"},
                {"scoreSpace", "100"},
                {"lifetimePowerups", "10"},
                {"shield", "2"},
                {"shooter", "2"},
                {"crateception", "2"},
                {"bubble", "2"},
                {"rocket", "2"}
            }
        };

        previousCurrents = new int[] { 0, 0, 0, 0, 0, 0 };
        powerupCurrents = new int[] { 0, 0, 0, 0, 0, 0 };
        powerupGoals = new int[] { 0, 0, 0, 0, 0, 0 };
        powerupIndexes = new int[] { 0, 0, 0, 0, 0, 0 };
        LoadMissions(data);

        timer = new Stopwatch();

        //PlayerPrefs.SetInt("Tutorial", 0);
        if (PlayerPrefs.GetInt($"Tutorial{SceneManager.GetActiveScene().name}") == 0) TutorialStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (inGame && before == 0) {
            if (map == "Space") {
                if (obstaclePoints >= 15) {
                    if (direction == 3) {
                        if (!speedingUp) {
                            StartCoroutine(ChangeDirection(true));
                            var shape = Speedlines.GetComponent<ParticleSystem>().shape;
                            shape.position = new Vector3(0f, 0f, 6.5f);
                            shape.rotation = new Vector3(180f, 0f, 0f);
                            obstacleSpeed += 2f;
                            timeBetween = timeBetween * 0.81f;
                            secondWait += 0.2f;
                        }
                    }
                    else {
                        if (!speedingUp) {
                            StartCoroutine(ChangeDirection(false));
                            var shape = Speedlines.GetComponent<ParticleSystem>().shape;
                            shape.position = new Vector3(0f, 0f, -6.5f);
                            shape.rotation = new Vector3(180f, 180f, 0f);
                        }
                    }
                }
                else {
                    if (frame < timeBetween) {
                        frame += Time.deltaTime;
                    }
                    else {
                        if (inGame) SendObstacle(true);
                        frame = 0f;
                    }
                }
            } else {
                if (obstaclePoints >= 25) {
                    if (!speedingUp) {
                        speedingUp = true;
                        StartCoroutine(SpeedUpShow());
                        obstacleSpeed += 1f;
                        timeBetween = timeBetween * 0.85f;
                    }
                }
                else {
                    if (frame < timeBetween) {
                        frame += Time.deltaTime;
                    }
                    else {
                        if (inGame) SendObstacle(true);
                        frame = 0f;
                    }
                }
                if (frameColor > 0f && map == "Ocean")
                {
                    //Debug.Log(Time.deltaTime);
                    frameColor -= 0.01f;
                    float multiplier = 1.016f;
                    //if (multiplier <= 1f) multiplier = 1.0001f;
                    oceanColor.color = new Color(oceanColor.color.r / multiplier, oceanColor.color.g / multiplier, oceanColor.color.b / multiplier, 1f);
                }
            }
            if (score >= 100 && SceneManager.GetActiveScene().name == "Game" && powerups == 0) {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQBw", 100.0f, (bool success) => {});
                if (PlayerPrefs.GetInt("GooglePlay") == 0 && !lastAchievements[8])
                {
                    Notif("Achievement unlocked!");
                }
                GiveAchievement(8);
            }
            if (score >= 75 && SceneManager.GetActiveScene().name == "Storm" && missed == 0) {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQCA", 100.0f, (bool success) => {});
                if (PlayerPrefs.GetInt("GooglePlay") == 0 && !lastAchievements[9])
                {
                    Notif("Achievement unlocked!");
                }
                GiveAchievement(9);
            }
            if (score >= 50 && SceneManager.GetActiveScene().name == "Ocean" && crates == 0) {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQCQ", 100.0f, (bool success) => {});
                if (PlayerPrefs.GetInt("GooglePlay") == 0 && !lastAchievements[10])
                {
                    Notif("Achievement unlocked!");
                }
                GiveAchievement(10);
            }
            if (score > lastHighscore && lastHighscore != 0 && !highscoreNotif)
            {
                highscoreNotif = true;
                Notif("New highscore!");
            }
        }
        scoreText.text = $"{score}";
    }

    void SendObstacle(bool allow, bool gooded = false) {
        // good = is this obstacle good?
        // gooded = was previous obstacle in a double sent good?
        if (powerup == 1) {
            Instantiate(Crate);
        }
        else if (powerupBalancer >= 12 && UnityEngine.Random.Range(1, 5) > 3) {
            if (map == "Ocean") powerupOpportunity = UnityEngine.Random.Range(1, 8);
            else powerupOpportunity = UnityEngine.Random.Range(1, 7);
            //powerupOpportunity = 5;
            powerupBalancer = 0;
            Instantiate(Powerup, new Vector3(0f, 6f, 0f), Quaternion.identity);
        }
        else {
            bool good = false; // checks if obstacle sent is good (true) or bad (false)
            if (SceneManager.GetActiveScene().name == "Game") {
                int random = UnityEngine.Random.Range(1, 4);
                if (random < 3) {
                    good = true;
                    Instantiate(Crate, new Vector3(0f, 6f, 0f), Quaternion.identity);
                }
                else if (random == 3) {
                    if (!allow) Instantiate(Crate, new Vector3(0f, 6f, 0f), Quaternion.identity);
                    else Instantiate(DangerSpike, new Vector3(0f, 6f, 0f), Quaternion.identity);
                }
            }
            else if (SceneManager.GetActiveScene().name == "Storm") {
                int random = UnityEngine.Random.Range(1, 11);
                if (random < 7) {
                    good = true;
                    Instantiate(Crate, new Vector3(0f, 6f, 0f), Quaternion.identity);
                }
                else if (random < 10) {
                    if (!allow) Instantiate(Crate, new Vector3(0f, 6f, 0f), Quaternion.identity);
                    else Instantiate(DangerSpike, new Vector3(0f, 6f, 0f), Quaternion.identity);
                }
                else {
                    if (!allow) Instantiate(Crate, new Vector3(0f, 6f, 0f), Quaternion.identity);
                    else Instantiate(ElectricBox, new Vector3(0f, 6f, 0f), Quaternion.identity);
                }
            }
            else if (SceneManager.GetActiveScene().name == "Ocean") {
                int random = UnityEngine.Random.Range(1, 11);
                if (random < 6) {
                    good = true;
                    Instantiate(Crate, new Vector3(0f, 6f, 0f), Quaternion.identity);
                }
                else if (random < 9) {
                    if (!allow) Instantiate(Crate, new Vector3(0f, 6f, 0f), Quaternion.identity);
                    else Instantiate(DangerSpike, new Vector3(0f, 6f, 0f), Quaternion.identity);
                }
                else if (random < 10) {
                    good = true;
                    Instantiate(Fish, new Vector3(0f, 6f, 0f), Quaternion.identity);
                }
                else {
                    if (!allow) Instantiate(Fish, new Vector3(0f, 6f, 0f), Quaternion.identity);
                    else Instantiate(Jelly, new Vector3(0f, 6f, 0f), Quaternion.identity);
                }
            }
            else if (SceneManager.GetActiveScene().name == "Space") {
                int random = UnityEngine.Random.Range(1, 16);
                if (random < 8) {
                    good = true;
                    Instantiate(Crate, new Vector3(0f, 6f, 0f), Quaternion.identity);
                }
                else if (random < 15) {
                    Instantiate(DangerSpike, new Vector3(0f, 6f, 0f), Quaternion.identity);
                }
                else {
                    Instantiate(Meteor, new Vector3(0f, 6f, 0f), Quaternion.identity);
                }
            }
            int random2 = UnityEngine.Random.Range(1, 21);
            if (random2 > 19 && allow == true && map != "Space") StartCoroutine(SecondObstacle(good));
        }
    }

    public void GameOver() {
        inGame = false;
        timer.Stop();
        if (spikeID == 5) StartCoroutine(Spike.GetComponent<Load>().BubblePop());
        else Spike.GetComponent<SpriteRenderer>().sprite = Spike.GetComponent<Load>().spriteHurt;
        if (!SpikeScript) SpikeScript = Spike.GetComponent<Load>();
        if (SpikeScript.looped) SpikeScript.MusicLoop.Stop();
        else SpikeScript.MusicSource.Stop();
        SpikeScript.Shooter(false);
        SpikeScript.Shield(false);
        //Spike.transform.localScale = new Vector3(5f, 5f, 1f);
        Speedlines.GetComponent<ParticleSystem>().Pause();
        if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameOverSource.Play();
        int mapHighscore = GetHighscore(SceneManager.GetActiveScene().name);
        Highscore.text = $"Highscore: {mapHighscore}";
        if (score > mapHighscore) {
            NewHighscore.text = "NEW HIGHSCORE";
            SaveHighscore(SceneManager.GetActiveScene().name, score);
            Highscore.text = $"Highscore: {score}";
        }
        else NewHighscore.text = "";
        Results.text = $"Score: {score}";
        AddCoins(score);
        if (PlayerPrefs.GetInt("Relaxed") == 1) Notice.SetActive(true);
        else
        {
            Notice.SetActive(false);
            TimeSpan elapsed = timer.Elapsed;
            if (elapsed.Seconds > 15 || elapsed.Minutes > 0)
            {
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds);
                Notice.GetComponent<TMPro.TextMeshProUGUI>().text = $"CRATEPHOBIA TIMER: {elapsedTime}";
                Notice.GetComponent<TMPro.TextMeshProUGUI>().color = new Color(1f, 0.892163f, 0.3716981f, 1f);
                Notice.SetActive(true);
            }
        }
        StartCoroutine(GameOverShow());
    }

    IEnumerator GameOverShow() {
        yield return new WaitForSeconds(1.5f);
        GameOverMenu.SetActive(true);
        if (PlayerPrefs.GetInt("Relaxed") == 1) GameOverMenu.GetComponent<Image>().color = new Color(0f, 0.4498f, 0.7921f, 1f);
        GameOverMenu.GetComponent<Animator>().enabled = true;
    }

    public void EnablePowerup(int id) {
        if (powerup != 0) DisablePowerupNow(powerup, pID, true);
        string[] spriteList = { "CRATECEPTION", "GROW", "SHIELD", "SHOCKWAVE", "SHOOTER", "SPEED", "BUBBLE", "ROCKET" };
        Color[] colorList = { new Color(0.886f, 0.662f, 0.337f, 1f), new Color(0.7f, 0.726f, 0.174f, 1f), new Color(0.074f, 0.298f, 0.031f, 1f), new Color(0.896f, 0.789f, 0.468f, 1f), new Color(0.717f, 0f, 0.18f, 1f), new Color(1f, 0.941f, 0.298f, 1f), new Color(0.392f, 0.584f, 0.8f, 1f), new Color(0.886f, 0.603f, 0.721f, 1f) };
        PowerupText.GetComponent<Animator>().enabled = false;
        PowerupText.GetComponent<TMPro.TextMeshProUGUI>().enabled = true;
        PowerupText.GetComponent<TMPro.TextMeshProUGUI>().text = spriteList[id - 1];
        PowerupText.GetComponent<TMPro.TextMeshProUGUI>().text = spriteList[id - 1];
        PowerupText.GetComponent<TMPro.TextMeshProUGUI>().color = colorList[id - 1];
        powerup = id;
        Data.SpikeData data = Data.GetFromFile();
        if (!SpikeScript) SpikeScript = Spike.GetComponent<Load>();
        if (id == 1)
        {
            if (powerupGoals[3] != 0)
            {
                powerupCurrents[3] = powerupCurrents[3] + 1;
                if (powerupCurrents[3] == powerupGoals[3])
                {
                    data.completedMissions[powerupIndexes[3]] = true;
                    Notif("Mission completed!");
                }
            }
        }
        if (id == 2) SpikeScript.Grow(true);
        if (id == 3)
        {
            SpikeScript.Shield(true);
            if (powerupGoals[1] != 0)
            {
                powerupCurrents[1] = powerupCurrents[1] + 1;
                if (powerupCurrents[1] == powerupGoals[1])
                {
                    data.completedMissions[powerupIndexes[1]] = true;
                    Notif("Mission completed!");
                }
            }
        }
        if (id == 4) Shockwave();
        if (id == 5)
        {
            SpikeScript.Shooter(true);
            if (powerupGoals[2] != 0)
            {
                powerupCurrents[2] = powerupCurrents[2] + 1;
                if (powerupCurrents[2] == powerupGoals[2])
                {
                    data.completedMissions[powerupIndexes[2]] = true;
                    Notif("Mission completed!");
                }
            }
        }
        if (id == 6) {
            superspeed = true;
            Spike.GetComponent<SpriteRenderer>().sprite = Spike.GetComponent<Load>().spritePowered;
            var main = Speedlines.GetComponent<ParticleSystem>().main;
            speedlinesSpeed += 10f;
            main.startSpeed = speedlinesSpeed;
        }
        if (id == 7) {
            subspeed = true;
            var main = Speedlines.GetComponent<ParticleSystem>().main;
            speedlinesSpeed -= 10f;
            main.startSpeed = speedlinesSpeed;
            if (powerupGoals[4] != 0)
            {
                powerupCurrents[4] = powerupCurrents[4] + 1;
                if (powerupCurrents[4] == powerupGoals[4])
                {
                    data.completedMissions[powerupIndexes[4]] = true;
                    Notif("Mission completed!");
                }
            }
        }
        if (id == 8) {
            SpikeScript.Rocket(true);
            shakeScript.Shake();
            superspeed = true;
            var main = Speedlines.GetComponent<ParticleSystem>().main;
            speedlinesSpeed += 20f;
            main.startSpeed = speedlinesSpeed;
            if (powerupGoals[5] != 0)
            {
                powerupCurrents[5] = powerupCurrents[5] + 1;
                if (powerupCurrents[5] == powerupGoals[5])
                {
                    data.completedMissions[powerupIndexes[5]] = true;
                    Notif("Mission completed!");
                }
            }
        }

        data.lifetimePowerups += 1;
        data.lifetimePowerupsToday += 1;
        if (powerupGoals[0] != 0)
        {
            powerupCurrents[0] = powerupCurrents[0] + 1;
            if (powerupCurrents[0] == powerupGoals[0])
            {
                data.completedMissions[powerupIndexes[0]] = true;
                Notif("Mission completed!");
            }
        }
        Data.SaveToFile(data);
        pID++;
        StartCoroutine(DisablePowerupText());
        StartCoroutine(DisablePowerup(id, pID));
    }

    void Shockwave() {
        if (direction == 3 && map == "Space") {
            ShockWave2.transform.position = new Vector3(0f, -7f, 0f);
            ShockWave2.GetComponent<ShockwaveStart>().ShockUp();
        } else {
            ShockWave.transform.position = new Vector3(0f, -7f, 0f);
            ShockWave.GetComponent<ShockwaveStart>().ShockUp();
        }
    }

    IEnumerator SpeedUpShow() {
        speedUps++;
        yield return new WaitForSeconds(1f);
        SpeedSource.Play();
        SpeedUpText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        if (map == "Ocean") frameColor = 0.15f;
        yield return new WaitForSeconds(1.75f);
        SpeedUpText.SetActive(false);
        if (speedUps % 2 == 0) {
            var main = Speedlines.GetComponent<ParticleSystem>().main;
            speedlinesSpeed += 10f;
            main.startSpeed = speedlinesSpeed;
        }
        obstaclePoints = 0;
        if (map == "Space") direction = 1;
        speedingUp = false;
    }

    IEnumerator ChangeDirection(bool speedUp) {
        speedingUp = true;
        if (speedUp) {
            speedUps++;
            yield return new WaitForSeconds(1f);
            SpeedSource.Play();
            SpeedUpText.SetActive(true);
            yield return new WaitForSeconds(2.25f);
            SpeedUpText.SetActive(false);
            if (spikeID == 6) SpikeScript.Flip("FlipUp");
            if (speedUps % 2 == 0) {
                var main = Speedlines.GetComponent<ParticleSystem>().main;
                speedlinesSpeed += 10f;
                main.startSpeed = speedlinesSpeed;
            }
            direction = 1;
            obstaclePoints = 0;
        } else {
            direction = 0;
            yield return new WaitForSeconds(1);
            if (spikeID == 6) {
                if (!SpikeScript) SpikeScript = Spike.GetComponent<Load>();
                SpikeScript.Flip("FlipDown");
            }
            direction = 3;
            obstaclePoints = 0;
        }
        speedingUp = false;
    }

    IEnumerator DisablePowerupText() {
        yield return new WaitForSeconds(1f);
        Animator powerupAnim = PowerupText.GetComponent<Animator>();
        powerupAnim.Rebind();
        powerupAnim.Update(0f);
        powerupAnim.GetComponent<Animator>().enabled = true;
    }

    void DisablePowerupNow(int id, int thisID, bool now)
    {
        if (pID == thisID && powerup != 0)
        {
            powerup = 0;
            if (id == 2) SpikeScript.Grow(false);
            if (id == 3) SpikeScript.Shield(false, now);
            if (id == 5) SpikeScript.Shooter(false);
            if (id == 6)
            {
                superspeed = false;
                if (inGame) Spike.GetComponent<SpriteRenderer>().sprite = Spike.GetComponent<Load>().spriteDefault;
                var main = Speedlines.GetComponent<ParticleSystem>().main;
                speedlinesSpeed -= 10f;
                main.startSpeed = speedlinesSpeed;
            }
            if (id == 7)
            {
                subspeed = false;
                var main = Speedlines.GetComponent<ParticleSystem>().main;
                speedlinesSpeed += 10f;
                main.startSpeed = speedlinesSpeed;
            }
            if (id == 8)
            {
                SpikeScript.Rocket(false, now);
            }
        }
    }

    IEnumerator DisablePowerup(int id, int thisID) {
        if (PlayerPrefs.GetInt("Relaxed") == 1) yield return new WaitForSeconds(10f);
        else if (id == 8) yield return new WaitForSeconds(7.5f);
        else yield return new WaitForSeconds(5f);
        DisablePowerupNow(id, thisID, false);
    }

    public void ResetPowerup() {
        superspeed = false;
        powerup = 0;
        var main = Speedlines.GetComponent<ParticleSystem>().main;
        speedlinesSpeed -= 20f;
        main.startSpeed = speedlinesSpeed;
    }

    public void Pause(bool paused) {
        if (inGame && PlayerPrefs.GetInt($"Tutorial{SceneManager.GetActiveScene().name}") == 1) {
            if (paused) {
                Time.timeScale = 0;
                if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) PauseSource.Play();
                if (!SpikeScript) SpikeScript = Spike.GetComponent<Load>();
                if (SpikeScript.looped) SpikeScript.MusicLoop.Pause();
                else SpikeScript.MusicSource.Pause();
                timer.Stop();
            }
            else {
                Time.timeScale = 1;
                if (!SpikeScript) SpikeScript = Spike.GetComponent<Load>();
                if (TutorialMenu.activeSelf == true) {
                    TutorialMenu.SetActive(false);
                }
                else if (PlayerPrefs.GetInt("MusicVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0)  {
                    if (SpikeScript.looped && before == 2) SpikeScript.MusicLoop.Play();
                    else if (before == 2) SpikeScript.MusicSource.Play();
                }
                timer.Start();
            }
            PauseMenu.SetActive(paused);
            if (!paused && SceneManager.GetActiveScene().name == "Game") TutorialMenu.SetActive(false);
        }
    }

    void TutorialStart() {
        Pause(true);
        TutorialMenu.SetActive(true);
        PlayerPrefs.SetInt($"Tutorial{SceneManager.GetActiveScene().name}", 1);
        Time.timeScale = 0;
    }

    public void LoadScene(string scene) {
        Time.timeScale = 1;
        if (scene == "Game") LoadingData.sceneToLoad = SceneManager.GetActiveScene().name;
        else LoadingData.sceneToLoad = scene;
        if (PauseMenu.activeSelf == true) AddCoins(score, true);
        else SceneManager.LoadScene("Loading");
    }

    void SaveHighscore(string map, int highscore) {
	    Data.SpikeData data = Data.GetFromFile();
        int num = 0;
        if (PlayerPrefs.GetInt("Relaxed") == 1) num = 1;
        if (map == "Storm") data.highStorm[num] = highscore;
        else if (map == "Ocean") data.highOcean[num] = highscore;
        else if (map == "Space") data.highSpace[num] = highscore;
	    else data.highClassic[num] = highscore;
	    Data.SaveToFile(data);
    }

    int GetHighscore(string map) {
        Data.SpikeData data = Data.GetFromFile();
        int num = 0;
        if (PlayerPrefs.GetInt("Relaxed") == 1) num = 1;
        if (map == "Storm") return data.highStorm[num];
        else if (map == "Ocean") return data.highOcean[num];
        else if (map == "Space") return data.highSpace[num];
	    else return data.highClassic[num];
    }

    void GiveAchievement(int achievement)
    {
        if (!lastAchievements[achievement])
        {
            Data.SpikeData data = Data.GetFromFile();
            lastAchievements[achievement] = true;
            data.achievements[achievement] = true;
            Data.SaveToFile(data);
        }
    }

    public IEnumerator TimerTime() {
        yield return new WaitForSeconds(180);
        if (PlayerPrefs.GetInt("Relaxed") == 0 && crates == 0 && SceneManager.GetActiveScene().name == "Game") {
            if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQCg", 100.0f, (bool success) => {});
            if (PlayerPrefs.GetInt("GooglePlay") == 0 && !lastAchievements[11])
            {
                Notif("Achievement unlocked!");
            }
            GiveAchievement(11);
        }
    }

    /*

        REQUIREMENTS:

        Skippit: default
        Boubola: 150 lifetime points
        Doot: 1000 lifetime points
        Fren: score of 150 in Classic without powerups
        Comm: score of 100 in Storm without missing a crate
        A literal bubble: score of 50 in Ocean without touching crates
        Baller: highscore of 100 in Space while staying in the lower half
        Qoob: check out the credits
        Asterisk: knock it off the title card
        Rose: purchase
        Earth: purchase
        Cory: purchase
        Sunny: purchase

     */

    void AddCoins(int score, bool loadScene = false) {
		Data.SpikeData data = Data.GetFromFile();
        float fscore = (float)score;
        if (PlayerPrefs.GetInt("Relaxed") == 0) data.coins = data.coins + (int)Mathf.Ceil(fscore / 2);
        else data.coins = data.coins + (int)Mathf.Ceil(fscore / 3);

        data.lifetime += score;
        data.lifetimeToday += score;
        if (PlayerPrefs.GetInt("GooglePlay") == 1) {
            #if UNITY_ANDROID
            PlayGamesPlatform.Instance.IncrementAchievement("CgkIqPj-8swdEAIQAw", score, (bool success) => {});
            #endif
        }

        if (PlayerPrefs.GetInt("GooglePlay") == 1)
        {
        #if UNITY_ANDROID
            PlayGamesPlatform.Instance.IncrementAchievement("CgkIqPj-8swdEAIQEA", powerups, (bool success) => { });
        #endif
        }

        bool notif = false;

        if (data.lifetime >= 5000 && !data.achievements[6])
        {
            data.achievements[6] = true;
            if (PlayerPrefs.GetInt("GooglePlay") == 0 && !notif)
            {
                notif = true;
                Notif("Achievement unlocked!");
            }
        }
        if (data.lifetimePowerups >= 100 && !data.achievements[7])
        {
            data.achievements[7] = true;
            if (PlayerPrefs.GetInt("GooglePlay") == 0 && !notif)
            {
                notif = true;
                Notif("Achievement unlocked!");
            }
        }
        if (SceneManager.GetActiveScene().name == "Game") {
            data.lifetimeClassic += score;
            data.lifetimeClassicToday += score;
        }
        if (SceneManager.GetActiveScene().name == "Storm") {
            data.lifetimeStorm += score;
            data.lifetimeStormToday += score;
        }
        if (SceneManager.GetActiveScene().name == "Ocean") {
            data.lifetimeOcean += score;
            data.lifetimeOceanToday += score;
        }
        if (SceneManager.GetActiveScene().name == "Space") {
            data.lifetimeSpace += score;
            data.lifetimeSpaceToday += score;
        }

        if (data.lifetime >= 150 && !data.spikes[1]) data.spikes[1] = true;
        if (data.lifetime >= 2500 && !data.spikes[2]) data.spikes[2] = true;
        if (data.lifetimeClassic >= 500 && SceneManager.GetActiveScene().name == "Game" && !data.spikes[3]) data.spikes[3] = true;
        if (data.lifetimeStorm >= 500 && SceneManager.GetActiveScene().name == "Storm" && !data.spikes[4]) data.spikes[4] = true;
        if (data.lifetimeOcean >= 500 && SceneManager.GetActiveScene().name == "Ocean" && !data.spikes[5]) data.spikes[5] = true;
        if (data.lifetimeSpace >= 500 && SceneManager.GetActiveScene().name == "Space" && !data.spikes[6]) data.spikes[6] = true;

        if (score >= 100 && SceneManager.GetActiveScene().name == "Game") {
            if (!data.maps[0]) data.maps[0] = true;
        }
        if (score >= 50 && SceneManager.GetActiveScene().name == "Storm") {
            if (!data.maps[1]) data.maps[1] = true;
        }
        if (score >= 100 && SceneManager.GetActiveScene().name == "Ocean") {
            if (!data.maps[2]) data.maps[2] = true;
        }

        if (PlayerPrefs.GetInt("Relaxed") == 0) {
            if (score >= 100 && SceneManager.GetActiveScene().name == "Game") {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQAA", 100.0f, (bool success) => {});
                if (PlayerPrefs.GetInt("GooglePlay") == 0 && !data.achievements[0] && !notif)
                {
                    notif = true;
                    Notif("Achievement unlocked!");
                }
                data.achievements[0] = true;
            }
            if (score >= 150 && SceneManager.GetActiveScene().name == "Game") {
                data.achievements[1] = true;
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQAQ", 100.0f, (bool success) => {});
                if (PlayerPrefs.GetInt("GooglePlay") == 0 && !data.achievements[1] && !notif)
                {
                    notif = true;
                    Notif("Achievement unlocked!");
                }
                data.achievements[1] = true;
            }
            if (score >= 200 && SceneManager.GetActiveScene().name == "Game") {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQAg", 100.0f, (bool success) => {});
                if (PlayerPrefs.GetInt("GooglePlay") == 0 && !data.achievements[2] && !notif)
                {
                    notif = true;
                    Notif("Achievement unlocked!");
                }
                data.achievements[2] = true;
            }
            if (score >= 100 && SceneManager.GetActiveScene().name == "Storm") {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQBA", 100.0f, (bool success) => {});
                if (PlayerPrefs.GetInt("GooglePlay") == 0 && !data.achievements[3] && !notif)
                {
                    notif = true;
                    Notif("Achievement unlocked!");
                }
                data.achievements[3] = true;
            }
            if (score >= 150 && SceneManager.GetActiveScene().name == "Ocean") {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQBQ", 100.0f, (bool success) => {});
                if (PlayerPrefs.GetInt("GooglePlay") == 0 && !data.achievements[4] && !notif)
                {
                    notif = true;
                    Notif("Achievement unlocked!");
                }
                data.achievements[4] = true;
            }
            if (score >= 80 && SceneManager.GetActiveScene().name == "Space") {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQBg", 100.0f, (bool success) => {});
                if (PlayerPrefs.GetInt("GooglePlay") == 0 && !data.achievements[5] && !notif)
                {
                    notif = true;
                    Notif("Achievement unlocked!");
                }
                data.achievements[5] = true;
            }
        }

        bool notEqual = false;
        if (previousCurrents.Length != powerupCurrents.Length) notEqual = true;
        else
        {
            for (int i = 0; i < previousCurrents.Length; i++)
            {
                if (previousCurrents[i] != powerupCurrents[i]) notEqual = true;
            }
        }

        if (!notEqual)
        {
            data.lifetimePowerupsToday = powerupCurrents[0];
            data.shieldToday = powerupCurrents[1];
            data.shooterToday = powerupCurrents[2];
            data.crateceptionToday = powerupCurrents[3];
            data.bubbleToday = powerupCurrents[4];
            data.rocketToday = powerupCurrents[5];
        }

        for (int i = 0; i < 3; i++)
        {
            if (!data.completedMissions[i])
            {
                string mission = data.dailyMissions[i];
                int id;
                string property;
                int objective;
                if (mission.Length == 2) id = int.Parse(mission.Substring(1, 1));
                else id = int.Parse(mission.Substring(1, 2));
                property = requirements[data.missionTier, id, 0];
                objective = int.Parse(requirements[data.missionTier, id, 1]);
                if (id < 5)
                {
                    int value = 0;
                    if (id == 0) value = data.lifetimeToday;
                    if (id == 1) value = data.lifetimeClassicToday;
                    if (id == 2) value = data.lifetimeStormToday;
                    if (id == 3) value = data.lifetimeOceanToday;
                    if (id == 4) value = data.lifetimeSpaceToday;
                    if (value >= objective)
                    {
                        data.completedMissions[i] = true;
                        if (!notif)
                        {
                            notif = true;
                            Notif("Mission completed!");
                        }
                    }
                }
                if (id < 9)
                {
                    bool qualify = false;
                    if (id == 5 && score >= objective && SceneManager.GetActiveScene().name == "Game") qualify = true;
                    if (id == 6 && score >= objective && map == "Storm") qualify = true;
                    if (id == 7 && score >= objective && map == "Ocean") qualify = true;
                    if (id == 8 && score >= objective && map == "Space") qualify = true;
                    if (qualify)
                    {
                        data.completedMissions[i] = true;
                        if (!notif)
                        {
                            notif = true;
                            Notif("Mission completed!");
                        }
                    }
                }
            }
        }

	    Data.SaveToFile(data);
        if (loadScene) SceneManager.LoadScene("Loading");
    }

    IEnumerator SecondObstacle(bool good) {
        yield return new WaitForSeconds(secondWait);
        SendObstacle(false, good);
    }

    void Notif(string text)
    {
        NotifText.SetActive(true);
        NotifText.GetComponent<TMPro.TextMeshProUGUI>().text = text;
        NotifText.GetComponent<Animation>().Play();
    }

    void LoadMissions(Data.SpikeData data)
    {
        for (int i = 0; i < 3; i++)
        {
            if (!data.completedMissions[i])
            {
                string mission1 = data.dailyMissions[i];
                int id1;
                string value1;
                string objective1;
                if (mission1.Length == 2) id1 = int.Parse(mission1.Substring(1, 1));
                else id1 = int.Parse(mission1.Substring(1, 2));
                value1 = requirements[data.missionTier, id1, 0];
                objective1 = requirements[data.missionTier, id1, 1];
                if (id1 > 8)
                {
                    int index = id1 - 9;
                    if (index == 0) previousCurrents[index] = data.lifetimePowerupsToday;
                    if (index == 1) previousCurrents[index] = data.shieldToday;
                    if (index == 2) powerupCurrents[index] = data.shooterToday;
                    if (index == 3) powerupCurrents[index] = data.crateceptionToday;
                    if (index == 4) powerupCurrents[index] = data.bubbleToday;
                    if (index == 5) powerupCurrents[index] = data.rocketToday;
                    powerupCurrents = previousCurrents;
                    powerupGoals[index] = int.Parse(objective1);
                    powerupIndexes[index] = i;
                }
            }
        }
    }
}
