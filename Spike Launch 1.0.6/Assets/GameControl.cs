using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
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

    public GameObject PauseMenu;
    public GameObject GameOverMenu;
    public GameObject TutorialMenu;
    public TMPro.TextMeshProUGUI Results;
    public TMPro.TextMeshProUGUI Highscore;
    public TMPro.TextMeshProUGUI NewHighscore;
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

    [Serializable]
    class SpikeData
    {
        public int coins;
        public int equipped;
        public int lifetime;
        public int lifetimeClassic;
        public int lifetimeStorm;
        public int lifetimeOcean;
        public int lifetimeSpace;
        public bool[] spikes = new bool[] {true, false, false, false, false, false, false, false, false, false, false, false, false};
        public bool titleCard;
        public bool checkedCredits;
        public bool[] maps = new bool[] {false, false, false};
        public int[] highClassic = new int[] {0, 0};
        public int[] highStorm = new int[] {0, 0};
        public int[] highOcean = new int[] {0, 0};
        public int[] highSpace = new int[] {0, 0};
        public bool[] achievements = new bool[] {false, false, false, false, false, false, false, false, false, false, false, false, false};
    }

    void SaveToFile(SpikeData data) {
        string json = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
        File.WriteAllText(Application.persistentDataPath + "/SpikeData.json", json);
    }

    SpikeData GetFromFile() {
        string json = File.ReadAllText(Application.persistentDataPath + "/SpikeData.json");
        return JsonUtility.FromJson<SpikeData>(Encoding.UTF8.GetString(Convert.FromBase64String(json)));
    }


    // Start is called before the first frame update
    void Start()
    {
        SpikeData data = GetFromFile();
        spikeID = data.equipped;
        inGame = true;
        before = 1;
        if (PlayerPrefs.GetInt("Relaxed") == 1) {
            obstacleSpeedStart = obstacleSpeedStart / 2f;
            timeBetweenStart = timeBetweenStart * 2f;
        }
        obstacleSpeed = obstacleSpeedStart;
        timeBetween = timeBetweenStart;
        frame = 0f;
        score = 0;
        obstaclePoints = 0;
        powerupBalancer = 0;
        speedUps = 0;
        scoreText = TextObject.GetComponent<TMPro.TextMeshProUGUI>();
        scoreText.text = "0";
        speedingUp = false;
        superspeed = false;
        subspeed = false;
        speedlinesSpeed = 25f;
        direction = 1;
        if (map == "Ocean") direction = 3;
        secondWait = 0.3f;

        crates = 0;
        powerups = 0;
        missed = 0;
        overY = false;

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
            }
            if (score >= 100 && SceneManager.GetActiveScene().name == "Game" && powerups == 0) {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQBw", 100.0f, (bool success) => {});
            }
            if (score >= 75 && SceneManager.GetActiveScene().name == "Storm" && missed == 0) {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQCA", 100.0f, (bool success) => {});
            }
            if (score >= 50 && SceneManager.GetActiveScene().name == "Ocean" && crates == 0) {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQCQ", 100.0f, (bool success) => {});
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
                    if (!allow && !gooded) Instantiate(Crate, new Vector3(0f, 6f, 0f), Quaternion.identity);
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
                    if (!allow && !gooded) Instantiate(Crate, new Vector3(0f, 6f, 0f), Quaternion.identity);
                    else Instantiate(DangerSpike, new Vector3(0f, 6f, 0f), Quaternion.identity);
                }
                else {
                    if (!allow && !gooded) Instantiate(Crate, new Vector3(0f, 6f, 0f), Quaternion.identity);
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
                    if (!allow && !gooded) Instantiate(Crate, new Vector3(0f, 6f, 0f), Quaternion.identity);
                    else Instantiate(DangerSpike, new Vector3(0f, 6f, 0f), Quaternion.identity);
                }
                else if (random < 10) {
                    good = true;
                    Instantiate(Fish, new Vector3(0f, 6f, 0f), Quaternion.identity);
                }
                else {
                    if (!allow && !gooded) Instantiate(Fish, new Vector3(0f, 6f, 0f), Quaternion.identity);
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
        Spike.GetComponent<SpriteRenderer>().sprite = Spike.GetComponent<Load>().spriteHurt;
        if (!SpikeScript) SpikeScript = Spike.GetComponent<Load>();
        if (SpikeScript.looped) SpikeScript.MusicLoop.Stop();
        else SpikeScript.MusicSource.Stop();
        SpikeScript.Shooter(false);
        SpikeScript.Shield(false);
        Spike.transform.localScale = new Vector3(5f, 5f, 1f);
        if (PlayerPrefs.GetInt("SoundVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0) GameOverSource.Play();
        int mapHighscore = GetHighscore(SceneManager.GetActiveScene().name, score);
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
        else Notice.SetActive(false);
        StartCoroutine(GameOverShow());
    }

    IEnumerator GameOverShow() {
        yield return new WaitForSeconds(1.5f);
        GameOverMenu.SetActive(true);
        GameOverMenu.GetComponent<Animator>().enabled = true;
    }

    public void EnablePowerup(int id) {
        string[] spriteList = {"CRATECEPTION", "GROW", "SHIELD", "SHOCKWAVE", "SHOOTER", "SPEED", "BUBBLE", "ROCKET"};
        Color[] colorList = {new Color(0.886f,  0.662f, 0.337f, 1f), new Color(0.7f, 0.726f, 0.174f, 1f), new Color(0.074f, 0.298f, 0.031f, 1f), new Color(0.896f, 0.789f, 0.468f, 1f), new Color(0.717f, 0f, 0.18f, 1f), new Color(1f, 0.941f, 0.298f, 1f), new Color(0.392f, 0.584f, 0.8f, 1f), new Color(0.886f, 0.603f, 0.721f, 1f)};
        PowerupText.GetComponent<Animator>().enabled = false;
        PowerupText.GetComponent<TMPro.TextMeshProUGUI>().enabled = true;
        PowerupText.GetComponent<TMPro.TextMeshProUGUI>().text = spriteList[id - 1];
        PowerupText.GetComponent<TMPro.TextMeshProUGUI>().text = spriteList[id - 1];
        PowerupText.GetComponent<TMPro.TextMeshProUGUI>().color = colorList[id - 1];
        powerup = id;
        if (!SpikeScript) SpikeScript = Spike.GetComponent<Load>();
        if (id == 2) SpikeScript.Grow(true);
        if (id == 3) SpikeScript.Shield(true);
        if (id == 4) Shockwave();
        if (id == 5) SpikeScript.Shooter(true);
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
        }
        if (id == 8) {
            SpikeScript.Rocket(true);
            shakeScript.Shake();
            superspeed = true;
            var main = Speedlines.GetComponent<ParticleSystem>().main;
            speedlinesSpeed += 20f;
            main.startSpeed = speedlinesSpeed;
        }
        // Configure bubble and rocket powerups later
        StartCoroutine(DisablePowerupText());
        StartCoroutine(DisablePowerup(id));
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
        SpeedUpText.SetActive(true);
        yield return new WaitForSeconds(2.25f);
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

    IEnumerator DisablePowerup(int id) {
        if (id == 8) yield return new WaitForSeconds(7.5f);
        else yield return new WaitForSeconds(5f);
        if (id != 3 && id != 8) powerup = 0;
        if (id == 2) SpikeScript.Grow(false);
        if (id == 3) SpikeScript.Shield(false);
        if (id == 5) SpikeScript.Shooter(false);
        if (id == 6) {
            superspeed = false;
            if (inGame) Spike.GetComponent<SpriteRenderer>().sprite = Spike.GetComponent<Load>().spriteDefault;
            var main = Speedlines.GetComponent<ParticleSystem>().main;
            speedlinesSpeed -= 10f;
            main.startSpeed = speedlinesSpeed;
        }
        if (id == 7) {
            subspeed = false;
            var main = Speedlines.GetComponent<ParticleSystem>().main;
            speedlinesSpeed += 10f;
            main.startSpeed = speedlinesSpeed;
        }
        if (id == 8) {
            SpikeScript.Rocket(false);
        }
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
            }
            else {
                Time.timeScale = 1;
                if (!SpikeScript) SpikeScript = Spike.GetComponent<Load>();
                if (TutorialMenu.activeSelf == true) {
                    TutorialMenu.SetActive(false);
                }
                else if (PlayerPrefs.GetInt("MusicVolume") != -1 && PlayerPrefs.GetInt("CrowdedMode") == 0)  {
                    if (SpikeScript.looped) SpikeScript.MusicLoop.Play();
                    else SpikeScript.MusicSource.Play();
                }
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
        SceneManager.LoadScene("Loading");
    }

    void SaveHighscore(string map, int highscore) {
	    SpikeData data = GetFromFile();
        int num = 0;
        if (PlayerPrefs.GetInt("Relaxed") == 1) num = 1;
        if (map == "Storm") data.highStorm[num] = highscore;
        else if (map == "Ocean") data.highOcean[num] = highscore;
        else if (map == "Space") data.highSpace[num] = highscore;
	    else data.highClassic[num] = highscore;
	    SaveToFile(data);
    }

    int GetHighscore(string map, int score) {
        SpikeData data = GetFromFile();
        int num = 0;
        if (PlayerPrefs.GetInt("Relaxed") == 1) num = 1;
        if (map == "Storm") return data.highStorm[num];
        else if (map == "Ocean") return data.highOcean[num];
        else if (map == "Space") return data.highSpace[num];
	    else return data.highClassic[num];
    }

    public IEnumerator TimerTime() {
        yield return new WaitForSeconds(60);
        if (PlayerPrefs.GetInt("Relaxed") == 0 && crates == 0) {
            if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQCg", 100.0f, (bool success) => {});
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

    void AddCoins(int score) {
		SpikeData data = GetFromFile();
        float fscore = (float)score;
        data.coins = data.coins + (int)Mathf.Ceil(fscore / 2);

        data.lifetime += score;
        if (PlayerPrefs.GetInt("GooglePlay") == 1) {
            #if UNITY_ANDROID
            PlayGamesPlatform.Instance.IncrementAchievement("CgkIqPj-8swdEAIQAw", score, (bool success) => {});
            #endif
        }
        if (SceneManager.GetActiveScene().name == "Game") data.lifetimeClassic += score;
        if (SceneManager.GetActiveScene().name == "Storm") data.lifetimeStorm += score;
        if (SceneManager.GetActiveScene().name == "Ocean") data.lifetimeOcean += score;
        if (SceneManager.GetActiveScene().name == "Space") data.lifetimeSpace += score;

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
            }
            if (score >= 150 && SceneManager.GetActiveScene().name == "Game") {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQAQ", 100.0f, (bool success) => {});
            }
            if (score >= 200 && SceneManager.GetActiveScene().name == "Game") {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQAg", 100.0f, (bool success) => {});
            }
            if (score >= 100 && SceneManager.GetActiveScene().name == "Storm") {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQBA", 100.0f, (bool success) => {});
            }
            if (score >= 150 && SceneManager.GetActiveScene().name == "Ocean") {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQBQ", 100.0f, (bool success) => {});
            }
            if (score >= 80 && SceneManager.GetActiveScene().name == "Space") {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQBg", 100.0f, (bool success) => {});
            }
        }

	    SaveToFile(data);
    }

    IEnumerator SecondObstacle(bool good) {
        yield return new WaitForSeconds(secondWait);
        SendObstacle(false, good);
    }
}
