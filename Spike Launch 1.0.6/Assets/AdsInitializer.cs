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
    string _adUnitId = null; // This will remain null for unsupported platforms*/
    private string _gameId;
    public bool loading;

    string id;

    public TMPro.TextMeshProUGUI coinText;

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
 
    void Awake()
    {
        #if UNITY_ANDROID
        id = "18d54d2bd";
        #else
        id = "1a0b5fe9d";
        #endif
        InitializeAds();
        loading = false;
        //Disable the button until the ad is ready to show:
        watchAdButton.interactable = false;
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
        IronSource.Agent.setUserId("10101");
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
    }

    void LoadedAd() {
    // Execute logic for the ad loading successfully.
        Debug.Log("Spike Launch: Loaded Ad Successfully");
        watchAdButton.interactable = true;
    }

    public void Show() {
        ShowAd();
    }

    void ShowAd() {
        Debug.Log("Spike Launch: Showing Ad");
        IronSource.Agent.showRewardedVideo();
    }

    void RewardAd(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        Debug.Log("Spike Launch: Rewarded Ad Completed");
        SpikeData data = GetFromFile();
        data.coins += 50;
        coinText.text = $"{data.coins}";
        SaveToFile(data);
    }

    void ClosedAd(IronSourceAdInfo adInfo)
    {
        Debug.Log("Spike Launch: Ad Closed");
    }

    void FailedAd(IronSourceError error, IronSourceAdInfo adInfo)
    {
        Debug.Log("Spike Launch: Ad Failed");
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