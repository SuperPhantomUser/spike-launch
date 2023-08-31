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

    string id;

    public TMPro.TextMeshProUGUI coinText;
    public TMPro.TextMeshProUGUI adText;

    public Data Data;

    private bool initialized = false;
    private bool available = false;
    private bool loading = false;

    void Start()
    {
        #if UNITY_ANDROID
        id = "18d54d2bd";
        #else
        id = "1a0b5fe9d";
        #endif
        InitializeAds();
        //Disable the button until the ad is ready to show:
        //watchAdButton.interactable = false;
    }

    void Update() 
    {
        
    }
 
    public void InitializeAds()
    {
        initialized = false;
        available = false;
        loading = false;
        //IronSource.Agent.setUserId("10101");
        IronSourceRewardedVideoEvents.onAdClosedEvent += ClosedAd;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardAd;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += FailedAd;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += AvailableAd;
        IronSource.Agent.init(id, IronSourceAdUnits.REWARDED_VIDEO);
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
        Debug.Log("Spike Launch: Verifying Integration");
        IronSource.Agent.validateIntegration();
    }

    void AvailableAd(IronSourceAdInfo adInfo)
    {
        loading = false;
        available = true;
        Debug.Log("Spike Launch: Loaded Ad Successfully");
    }

    private void SdkInitializationCompletedEvent() 
    {
        Debug.Log("Spike Launch: Ad Initialization Complete");
        LoadAd();
    }
 
    // Load content to the Ad Unit: 
    public void LoadAd()
    {
        Debug.Log("Spike Launch: Loading Ad");
        loading = true;
    }

    void ShowAd() { // this is the button method
        Debug.Log("Spike Launch: Preparing Ad");
        adText.text = "Preparing ad...";
        bool tempAvailable = IronSource.Agent.isRewardedVideoAvailable();
        if (tempAvailable || available) DisplayAd();
        else if (loading) adText.text = "Loading ad...";
    }

    void DisplayAd()
    {
        available = false;
        loading = false;
        Debug.Log("Spike Launch: Displaying Ad");
        adText.text = "Loaded ad successfully.";
        IronSource.Agent.showRewardedVideo();
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