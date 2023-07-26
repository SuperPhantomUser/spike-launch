using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class HomeControl : MonoBehaviour
{

    public Animation HomeAnimation;
    public Animation SettingsAnimation;
    public Animation SelectAnimation;
    public Animation MapsAnimation;

    public LoadingData LoadingData;

    public GameObject AdsMenu;
    public bool creditsOpen;
    public bool adsOpen;

    public bool playSound;

    public Preview Preview;

    public AudioSource StartSource;

    public bool moving;

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
        SpikeData tempData = JsonUtility.FromJson<SpikeData>(Encoding.UTF8.GetString(Convert.FromBase64String(json)));
        return tempData;
    }

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        playSound = true;
        ReloadVolume();
        CheckReqs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckReqs() {
        Preview.LoadPreviewStats();
    }

    public void SettingsScreen(bool open) {
        if (moving == false) {
            moving = true;
            AdsMenu.SetActive(false);
            if (open) {
                HomeAnimation.Play("SlideRight");
                SettingsAnimation.Play("SlideRightSettings");
            } else {
                SettingsAnimation.Play("SlideLeftSettings");
                HomeAnimation.Play("SlideLeftBack");
            }
        }
    }

    public void PlayerSelectScreen(bool open) {
        if (moving == false) {
            //AdsMenu.SetActive(false);
            moving = true;
            if (open) {
                HomeAnimation.Play("SlideLeft");
                SelectAnimation.Play("SlideLeftSelect");
            } else {
                SelectAnimation.Play("SlideRightSelect");
                HomeAnimation.Play("SlideRightBack");
            }
        }
    }

    public void MapSelectScreen(bool open) {
        if (moving == false) {
            //AdsMenu.SetActive(false);
            moving = true;
            if (open) {
                HomeAnimation.Play("SlideUp");
                MapsAnimation.Play("SlideUpMaps");
            } else {
                MapsAnimation.Play("SlideDownMaps");
                HomeAnimation.Play("SlideDown");
            }
        }
    }

    public void LoadMap(string scene) {
        LoadingData.sceneToLoad = scene;
        StartSource.Play();
        SceneManager.LoadScene("Loading");
    }

    public void ReloadVolume() {
        Debug.Log("hi");
        if (PlayerPrefs.GetInt("MusicVolume") == -1 || PlayerPrefs.GetInt("CrowdedMode") == 1) {
            GetComponent<AudioSource>().mute = true;
        }
        else if (PlayerPrefs.GetInt("MusicVolume") == 50) {
            GetComponent<AudioSource>().volume = 0.5f;
            GetComponent<AudioSource>().mute = false;
        }
        else {
            GetComponent<AudioSource>().volume = 1f;
            GetComponent<AudioSource>().mute = false;
        }

        if (PlayerPrefs.GetInt("SoundVolume") == -1 || PlayerPrefs.GetInt("CrowdedMode") == 1) playSound = false;
        else playSound = true;
    }

    public void AdMenu() {
        if (adsOpen) {
            AdsMenu.SetActive(false);
            adsOpen = false;
        }
        else {
            AdsMenu.SetActive(true);
            adsOpen = true;
        }
    }

    public void OpenLink(string link) {
        Application.OpenURL(link);
    }

    public void Achievements() {
        Social.ShowAchievementsUI();
    }
}
