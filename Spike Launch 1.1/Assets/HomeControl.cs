using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#elif UNITY_IOS
using UnityEngine.iOS;
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

    public GameObject RatesMenu;
    public GameObject MissionMenu;
    public Toggle ratesToggle;
    public TMPro.TextMeshProUGUI ratesText;

    public bool playSound;

    public Preview Preview;

    public AudioSource StartSource;

    public bool moving;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        playSound = true;
        ReloadVolume();
        CheckReqs();
        if (!PlayerPrefs.HasKey("LoadNumber")) PlayerPrefs.SetInt("LoadNumber", 0);
        if (!PlayerPrefs.HasKey("NoRate") || PlayerPrefs.GetInt("NoRate") == 0)
        {
            PlayerPrefs.SetInt("LoadNumber", PlayerPrefs.GetInt("LoadNumber") + 1);
            if (PlayerPrefs.GetInt("LoadNumber") >= 10)
            {
                PlayerPrefs.SetInt("LoadNumber", 0);
                RateMenu();
            }
        }
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
            //AdsMenu.SetActive(false);
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
        Debug.Log("Character");
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
        if (scene == "Credits") SceneManager.LoadScene("Credits");
        else
        {
            LoadingData.sceneToLoad = scene;
            StartSource.Play();
            SceneManager.LoadScene("Loading");
        }
    }

    public void ReloadVolume() {
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
        if (MissionMenu.GetComponent<RectTransform>().anchoredPosition.y == 500f && RatesMenu.activeSelf == false)
        {
            if (adsOpen)
            {
                GetComponent<AdsInitializer>().StopLoading();
                AdsMenu.GetComponent<Animation>().Play("BoardClose");
                adsOpen = false;
            }
            else
            {
                AdsMenu.GetComponent<Animation>().Play("BoardOpen");
                adsOpen = true;
            }
        }
    }

    public void RateMenu()
    {
        RatesMenu.SetActive(true);
#if UNITY_ANDROID
        ratesText.text = "Why not rate it on the Google Play Store? It's a free way to support the game!";
#else
        ratesText.text = "Why not rate it on the App Store? It's a free way to support the game!";
#endif
    }

    public void RateGame()
    {
        RatesMenu.SetActive(false);
        PlayerPrefs.SetInt("NoRate", 1);
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + Application.identifier);
#elif UNITY_IOS
        //Application.OpenURL("itms://itunes.apple.com/us/app/apple-store/6449191718?mt=8");
        Device.RequestStoreReview();
#endif
    }

    public void CloseRateMenu()
    {
        if (ratesToggle.isOn) PlayerPrefs.SetInt("NoRate", 1);
        else PlayerPrefs.SetInt("NoRate", 0);
        RatesMenu.SetActive(false);
    }

    public void OpenLink(string link) {
        Application.OpenURL(link);
    }

    public void Achievements() {
        Social.ShowAchievementsUI();
    }
}
