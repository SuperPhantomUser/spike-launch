using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;

public class Data : MonoBehaviour
{

    public class SpikeData106
    {
        public int coins = 0;
        public int equipped = 0;
        public int lifetime = 0;
        public int lifetimeClassic = 0;
        public int lifetimeStorm = 0;
        public int lifetimeOcean = 0;
        public int lifetimeSpace = 0;
        public int lifetimePowerups = 0;
        public bool[] spikes = new bool[] { true, false, false, false, false, false, false, false, false, false, false, false, false };
        public bool titleCard = false;
        public bool checkedCredits = false;
        public bool[] maps = new bool[] { false, false, false };
        public int[] highClassic = new int[] { 0, 0 }; // one for normal, one for relaxed
        public int[] highStorm = new int[] { 0, 0 };
        public int[] highOcean = new int[] { 0, 0 };
        public int[] highSpace = new int[] { 0, 0 };
    }

    [Serializable]
    public class SpikeData
    {
        public int coins = 0;
        public int equipped = 0;
        public int lifetime = 0;
        public int lifetimeClassic = 0;
        public int lifetimeStorm = 0;
        public int lifetimeOcean = 0;
        public int lifetimeSpace = 0;
        public int lifetimePowerups = 0;
        public bool[] spikes = new bool[] { true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        // skippit, boubola, doot, fren, comm, bubble, baller, qoob, asterisk, rose, lifa, jack, sunny
        // new: skippit, boubola, doot, fren, comm, bubble, baller, qoob, asterisk, rose, lifa, flippy, volley, jack, sunny, earth
        public bool titleCard = false;
        public bool checkedCredits = false;
        public bool[] maps = new bool[] { false, false, false };
        // storm, ocean, space
        public int[] highClassic = new int[] { 0, 0 }; // one for normal, one for relaxed
        public int[] highStorm = new int[] { 0, 0 };
        public int[] highOcean = new int[] { 0, 0 };
        public int[] highSpace = new int[] { 0, 0 };

        public bool checkedGooglePlay = false;
        public string[] dailyMissions = new string[] { "-1", "-1", "-1" }; // daily missions id for today (randomized everyday depending on reward tier)
        public string lastChecked = "0/0/0000";
        public bool[] completedMissions = new bool[] { false, false, false }; // daily missions completed today
        public int missionStreak = 0; // how many days they have completed missions
        public int missionTier = 0; // their current tier
        public bool claimedToday = false; // if they have claimed their mission reward today
        public bool[] postcards = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false };
        // 12 postcards
        public int lifetimeToday = 0;
        public int lifetimeClassicToday = 0;
        public int lifetimeStormToday = 0;
        public int lifetimeOceanToday = 0;
        public int lifetimeSpaceToday = 0;
        public int lifetimePowerupsToday = 0;
        public int shieldToday = 0;
        public int shooterToday = 0;
        public int crateceptionToday = 0;
        public int bubbleToday = 0;
        public int rocketToday = 0;

        public int adsWatched = 0;

        public bool[] achievements = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        /*
        novice launcher
        great launcher
        epic launcher
        stormy weather
        take a dive
        deep space
        lifetime launcher
        powerup connoisseur
        powerup avoidance
        overachiever
        cratephobia
        cratephobia 2
        is it a square
        knockoff
        */
    }

    public void SaveToFile(SpikeData data)
    {
        //SpikeData myData = GetFromFile();
        string json = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
        File.WriteAllText(Application.persistentDataPath + "/SpikeData.json", json);
    }

    public SpikeData GetFromFile()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/SpikeData.json");
        if (!PlayerPrefs.HasKey("Version"))
        {
            PlayerPrefs.SetInt("Version", 1);
            SpikeData data = JsonUtility.FromJson<SpikeData>(Encoding.UTF8.GetString(Convert.FromBase64String(json)));
            data.lifetimePowerups = 0;
            data.spikes = new bool[]
            {
                data.spikes[0], data.spikes[1], data.spikes[2], data.spikes[3], data.spikes[4], data.spikes[5], data.spikes[6],
                data.spikes[7], data.spikes[8], data.spikes[9], data.spikes[10], false, false, data.spikes[11], data.spikes[12], false
            };
            data.checkedGooglePlay = false;
            data.dailyMissions = new string[] { "-1", "-1", "-1" };
            data.lastChecked = "0/0/0000";
            data.completedMissions = new bool[] { false, false, false };
            data.missionStreak = 0;
            data.missionTier = 0;
            data.claimedToday = false;
            data.postcards = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false };
            // 12 postcards
            data.lifetimeToday = 0;
            data.lifetimeClassicToday = 0;
            data.lifetimeStormToday = 0;
            data.lifetimeOceanToday = 0;
            data.lifetimeSpaceToday = 0;
            data.lifetimePowerupsToday = 0;
            data.shieldToday = 0;
            data.shooterToday = 0;
            data.crateceptionToday = 0;
            data.bubbleToday = 0;
            data.rocketToday = 0;

            data.adsWatched = 0;

            data.achievements = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            return data;
        }
        else
        {
            SpikeData data = JsonUtility.FromJson<SpikeData>(Encoding.UTF8.GetString(Convert.FromBase64String(json)));
            return data;
        }
    }
}
