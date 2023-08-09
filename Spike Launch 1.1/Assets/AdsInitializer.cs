using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
 
public class AdsInitializer : MonoBehaviour
{
    public Button watchAdButton;
    /*[SerializeField] string _androidGameId = "5173817";
    [SerializeField] string _iOSGameId = "5173816";
    [SerializeField] bool _testMode = true;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; // This will remain null for unsupported platforms
    private string _gameId;*/
    public bool loading;

    string id;

    public TMPro.TextMeshProUGUI coinText;
    public TMPro.TextMeshProUGUI adText;

    public Data Data;

    void Start()
    {
        #if UNITY_ANDROID
        id = "18d54d2bd";
        #else
        id = "1a0b5fe9d";
        #endif
        InitializeAds();
        loading = false;
        //Disable the button until the ad is ready to show:
        //watchAdButton.interactable = false;
        IronSourceRewardedVideoEvents.onAdClosedEvent += ClosedAd;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardAd;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += FailedAd;
    }

    void Update() 
    {
        if (loading) {
            bool available = IronSource.Agent.isRewardedVideoAvailable();
            if (available) {
                loading = false;
                LoadedAd();
            }
        }
    }
 
    public void InitializeAds()
    {
        //IronSource.Agent.setUserId("10101");
        IronSource.Agent.init(id, IronSourceAdUnits.REWARDED_VIDEO);
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
        Debug.Log("Spike Launch: Verifying Integration");
        IronSource.Agent.validateIntegration();
    }

    private void SdkInitializationCompletedEvent(){
        Debug.Log("Spike Launch: Ad Initialization Complete");
        LoadAd();
    }
 
    // Load content to the Ad Unit: 
    public void LoadAd()
    {
        loading = true;
        Debug.Log("Spike Launch: Loading Ad");
        adText.text = "Loading ad...";
    }

    void LoadedAd() {
    // Execute logic for the ad loading successfully.
        Debug.Log("Spike Launch: Loaded Ad Successfully");
        adText.text = "Loaded ad successfully.";
        IronSource.Agent.showRewardedVideo();
    }

    public void Show() {
        ShowAd();
    }

    void ShowAd() {
        Debug.Log("Spike Launch: Showing Ad");
        adText.text = "Preparing ad...";
        LoadAd();
    }

    void RewardAd(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        Debug.Log("Spike Launch: Rewarded Ad Completed");
        adText.text = "Success! 50 coins rewarded.";
        Data.SpikeData data = Data.GetFromFile();
        data.coins += 50;
        data.adsWatched += 1;
        coinText.text = $"{data.coins}";
        Data.SaveToFile(data);
    }

    void ClosedAd(IronSourceAdInfo adInfo)
    {
        Debug.Log("Spike Launch: Ad Closed");
        adText.text = "Hmm, looks like you closed the ad.";
    }

    void FailedAd(IronSourceError error, IronSourceAdInfo adInfo)
    {
        Debug.Log("Spike Launch: Ad Failed");
        adText.text = "Ad failed to load :(";

    }
 
    void OnDestroy()
    {
        // Clean up the button listeners: 
        watchAdButton.onClick.RemoveAllListeners();
    }

    void OnApplicationPause(bool isPaused) {                 
        IronSource.Agent.onApplicationPause(isPaused);
    }

}