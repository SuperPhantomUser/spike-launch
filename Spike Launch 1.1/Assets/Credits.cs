using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine.SocialPlatforms;
#if UNITY_ANDROID
using GooglePlayGames;
#endif

public class Credits : MonoBehaviour
{

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
        if (!data.checkedCredits) {
            data.spikes[7] = true;
            data.checkedCredits = true;
            data.achievements[12] = true;
            SaveToFile(data);
            if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQCw", 100.0f, (bool success) => {});
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cya() {
        SceneManager.LoadScene("Home");
    }
}
