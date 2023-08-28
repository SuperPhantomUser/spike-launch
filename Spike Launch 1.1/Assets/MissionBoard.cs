using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class MissionBoard : MonoBehaviour
{

    public bool boardOpen;
    public Animation Board;

    private string[,,] missions;
    private string[,,] requirements;
    public Sprite[] icons;

    public GameObject Mission1;
    public GameObject Mission2;
    public GameObject Mission3;

    public GameObject AdMenu;
    public GameObject RateMenu;

    public TMPro.TextMeshProUGUI CoinText;
    public TMPro.TextMeshProUGUI StreakText;
    public TMPro.TextMeshProUGUI RewardText;

    private int column;

    public Data Data;

    public GameObject[] objectList;

    public GameObject Scroll;

    // Start is called before the first frame update
    void Start()
    {

        missions = new string[4, 15, 2] {
            {
                {"Obtain 300 combined points in all maps", "-1"},
                {"Obtain 200 combined points in Classic", "-1"},
                {"Obtain 100 combined points in Storm", "0"},
                {"Obtain 200 combined points in Ocean", "1"},
                {"Obtain 150 combined points in Space", "2"},
                {"Reach a score of 150 in Classic", "-1"},
                {"Reach a score of 30 in Storm", "0"},
                {"Reach a score of 100 in Ocean", "1"},
                {"Reach a score of 50 in Space", "2"},
                {"Use any 5 powerups", "-1"},
                {"Use 1 Shield powerup", "-1"},
                {"Use 1 Shooter powerup", "-1"},
                {"Use 1 Crateception powerup", "-1"},
                {"Use 1 Bubble powerup", "1"},
                {"Use 1 Rocket powerup", "2"}
            },
            {
                {"Obtain 400 combined points in all maps", "-1"},
                {"Obtain 250 combined points in Classic", "-1"},
                {"Obtain 150 combined points in Storm", "0"},
                {"Obtain 250 combined points in Ocean", "1"},
                {"Obtain 200 combined points in Space", "2"},
                {"Reach a score of 150 in Classic", "-1"},
                {"Reach a score of 50 in Storm", "0"},
                {"Reach a score of 125 in Ocean", "1"},
                {"Reach a score of 75 in Space", "2"},
                {"Use any 6 powerups", "-1"},
                {"Use 1 Shield powerup", "-1"},
                {"Use 1 Shooter powerup", "-1"},
                {"Use 1 Crateception powerup", "-1"},
                {"Use 1 Bubble powerup", "1"},
                {"Use 1 Rocket powerup", "2"}
            },
            {
                {"Obtain 600 combined points in all maps", "-1"},
                {"Obtain 350 combined points in Classic", "-1"},
                {"Obtain 250 combined points in Storm", "0"},
                {"Obtain 350 combined points in Ocean", "1"},
                {"Obtain 300 combined points in Space", "2"},
                {"Reach a score of 175 in Classic", "-1"},
                {"Reach a score of 70 in Storm", "0"},
                {"Reach a score of 150 in Ocean", "1"},
                {"Reach a score of 75 in Space", "2"},
                {"Use any 7 powerups", "-1"},
                {"Use 2 Shield powerups", "-1"},
                {"Use 2 Shooter powerups", "-1"},
                {"Use 2 Crateception powerups", "-1"},
                {"Use 2 Bubble powerups", "1"},
                {"Use 2 Rocket powerups", "2"}
            },
            {
                {"Obtain 800 combined points in all maps", "-1"},
                {"Obtain 450 combined points in Classic", "-1"},
                {"Obtain 350 combined points in Storm", "0"},
                {"Obtain 450 combined points in Ocean", "1"},
                {"Obtain 400 combined points in Space", "2"},
                {"Reach a score of 175 in Classic", "-1"},
                {"Reach a score of 90 in Storm", "0"},
                {"Reach a score of 175 in Ocean", "1"},
                {"Reach a score of 100 in Space", "2"},
                {"Use any 10 powerups", "-1"},
                {"Use 2 Shield powerups", "-1"},
                {"Use 2 Shooter powerups", "-1"},
                {"Use 2 Crateception powerups", "-1"},
                {"Use 2 Bubble powerups", "1"},
                {"Use 2 Rocket powerups", "2"}
            }
        };

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

        /* mission id = tier (0-3) followed by index (0-14)
        
        ending in 0-4 = lifetime (combined) points
        ending in 5-8 = highscore
        ending in 9-14 = powerups

        */

        boardOpen = false;
        column = 0;

        Data.SpikeData data = Data.GetFromFile();
        string todayDate = DateTime.Now.Date.ToString("d");
        //data.lastChecked = "8/8/2023";
        //data.missionStreak = 0;
        //data.missionTier = 0;
        if (data.lastChecked != todayDate)
        {
            data.dailyMissions = new string[] { "-1", "-1", "-1" };
            data.lastChecked = todayDate;
            data.completedMissions = new bool[] { false, false, false };
            data.lifetimeToday = 0;
            data.lifetimePowerupsToday = 0;
            data.lifetimeClassicToday = 0;
            data.lifetimeStormToday = 0;
            data.lifetimeOceanToday = 0;
            data.lifetimeSpaceToday = 0;
            data.shieldToday = 0;
            data.shooterToday = 0;
            data.crateceptionToday = 0;
            data.bubbleToday = 0;
            data.rocketToday = 0;
            if (!data.claimedToday)
            {
                data.missionStreak = 0;
                if (data.missionTier > 0) data.missionTier -= 0;
            }
            if (data.missionStreak >= 2 && data.missionTier == 0) data.missionTier = 1;
            if (data.missionStreak >= 5 && data.missionTier == 1) data.missionTier = 2;
            if (data.missionStreak >= 10 && data.missionTier == 2) data.missionTier = 3;
            data.claimedToday = false;
            GenerateMissions(data);
        }
        else UpdateMissions(data);
        UpdateBoard(column);
        UpdateAchievements();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenBoard()
    {
        if (AdMenu.GetComponent<RectTransform>().anchoredPosition.y == 500f && RateMenu.activeSelf == false)
        {
            if (!boardOpen) Board.GetComponent<Animation>().Play("BoardOpen");
            else Board.GetComponent<Animation>().Play("BoardClose");
            boardOpen = !boardOpen;
        }
    }

    void GenerateMissions(Data.SpikeData data)
    {
        bool powerup = false;
        string missionPicked1 = "";
        while (missionPicked1.Length == 0)
        {
            int random = UnityEngine.Random.Range(1, 16) - 1;
            string randomMission = missions[data.missionTier, random, 1];
            if (randomMission == "-1" || data.maps[int.Parse(randomMission)] == true)
            {
                missionPicked1 = $"{data.missionTier}{random}";
                if (random > 9) powerup = true;
                string progress;
                if (random < 5) progress = $"0/{requirements[data.missionTier, random, 1]} points";
                else if (random < 9) progress = "Not achieved";
                else progress = $"0/{requirements[data.missionTier, random, 1]} powerups";
                SetMission(Mission1, random, missions[data.missionTier, random, 0], progress);
            }
        }
        string missionPicked2 = "";
        while (missionPicked2.Length == 0)
        {
            int random = UnityEngine.Random.Range(1, 16) - 1;
            string randomMission = missions[data.missionTier, random, 1];
            if (randomMission == "-1" || data.maps[int.Parse(randomMission)] == true)
            {
                string tempMission = $"{data.missionTier}{random}";
                if (tempMission != missionPicked1 && (random < 10 || !powerup))
                {
                    missionPicked2 = tempMission;
                    if (random > 9) powerup = true;
                    string progress;
                    if (random < 5) progress = $"0/{requirements[data.missionTier, random, 1]} points";
                    else if (random < 9) progress = "Not achieved";
                    else progress = $"0/{requirements[data.missionTier, random, 1]} powerups";
                    SetMission(Mission2, random, missions[data.missionTier, random, 0], progress);
                }
            }
        }
        string missionPicked3 = "";
        while (missionPicked3.Length == 0)
        {
            int random = UnityEngine.Random.Range(1, 16) - 1;
            string randomMission = missions[data.missionTier, random, 1];
            if (randomMission == "-1" || data.maps[int.Parse(randomMission)] == true)
            {
                string tempMission = $"{data.missionTier}{random}";
                if (tempMission != missionPicked1 && tempMission != missionPicked2 && (random < 10 || !powerup))
                {
                    missionPicked3 = tempMission;
                    if (random > 9) powerup = true;
                    string progress;
                    if (random < 5) progress = $"0/{requirements[data.missionTier, random, 1]} points";
                    else if (random < 9) progress = "Incomplete";
                    else progress = $"0/{requirements[data.missionTier, random, 1]} powerups";
                    SetMission(Mission3, random, missions[data.missionTier, random, 0], progress);
                }
            }
        }

        data.dailyMissions = new string[] { missionPicked1, missionPicked2, missionPicked3 };
        if (data.missionTier == 0) RewardText.text = "Reward: $350";
        if (data.missionTier == 1) RewardText.text = "Reward: $400";
        if (data.missionTier == 2) RewardText.text = "Reward: $450";
        if (data.missionTier == 3) RewardText.text = "Reward: $500";
        StreakText.text = $"Daily streak: {data.missionStreak}";
        Data.SaveToFile(data);
    }

    void UpdateMissions(Data.SpikeData data)
    {
        for (int i = 0; i < 3; i++)
        {
            string mission = data.dailyMissions[i];
            int id;
            string property;
            int objective;
            if (mission.Length == 2) id = int.Parse(mission.Substring(1, 1));
            else id = int.Parse(mission.Substring(1, 2));
            property = requirements[data.missionTier, id, 0];
            objective = int.Parse(requirements[data.missionTier, id, 1]);
            string progress = "Incomplete";
            if (id < 5)
            {
                int value = 0;
                if (id == 0) value = data.lifetimeToday;
                if (id == 1) value = data.lifetimeClassicToday;
                if (id == 2) value = data.lifetimeStormToday;
                if (id == 3) value = data.lifetimeOceanToday;
                if (id == 4) value = data.lifetimeSpaceToday;
                if (data.completedMissions[i] == true) progress = "Complete!";
                else progress = $"{value}/{objective} points";
            }
            else if (id < 9)
            {
                if (data.completedMissions[i] == true) progress = "Complete!";
                else progress = "Incomplete";
            }
            else
            {
                int value = 0;
                if (id == 9) value = data.lifetimePowerupsToday;
                if (id == 10) value = data.shieldToday;
                if (id == 11) value = data.shooterToday;
                if (id == 12) value = data.crateceptionToday;
                if (id == 13) value = data.bubbleToday;
                if (id == 14) value = data.rocketToday;
                if (data.completedMissions[i] == true) progress = "Complete!";
                else progress = $"{value}/{objective} powerups";
            }
            GameObject ThisMission;
            if (i == 0) ThisMission = Mission1;
            else if (i == 1) ThisMission = Mission2;
            else ThisMission = Mission3;
            SetMission(ThisMission, id, missions[data.missionTier, id, 0], progress);
        }
        if (data.completedMissions[0] && data.completedMissions[1] && data.completedMissions[2] && !data.claimedToday)
        {
            data.claimedToday = true;
            data.missionStreak += 1;
            int reward = 0;
            if (data.missionTier == 0) reward = 350;
            if (data.missionTier == 1) reward = 400;
            if (data.missionTier == 2) reward = 450;
            if (data.missionTier == 3) reward = 500;
            data.coins += reward;
            CoinText.text = $"${data.coins}";
        }

        if (data.claimedToday)
        {
            if (data.missionTier == 0) RewardText.text = "Reward: $350 (Collected!)";
            if (data.missionTier == 1) RewardText.text = "Reward: $400 (Collected!)";
            if (data.missionTier == 2) RewardText.text = "Reward: $450 (Collected!)";
            if (data.missionTier == 3) RewardText.text = "Reward: $500 (Collected!)";
        } else
        {
            if (data.missionTier == 0) RewardText.text = "Reward: $350";
            if (data.missionTier == 1) RewardText.text = "Reward: $400";
            if (data.missionTier == 2) RewardText.text = "Reward: $450";
            if (data.missionTier == 3) RewardText.text = "Reward: $500";
        }
        StreakText.text = $"Daily streak: {data.missionStreak}";
        Data.SaveToFile(data);
    }

    void SetMission(GameObject Mission, int id, string text, string progress)
    {
        Mission.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = text;
        Mission.transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = progress;
        GameObject iconObj = Mission.transform.GetChild(1).gameObject;
        int icon = 0;
        if (id == 0 || id == 1 || id == 5) icon = 0;
        else if (id == 2 || id == 6) icon = 1;
        else if (id == 3 || id == 7) icon = 2;
        else if (id == 4 || id == 8) icon = 3;
        else if (id == 9) icon = 4;
        else if (id == 10) icon = 5;
        else if (id == 11) icon = 6;
        else if (id == 12) icon = 7;
        else if (id == 13) icon = 8;
        else if (id == 14) icon = 9;
        iconObj.GetComponent<Image>().sprite = icons[icon];
    }

    public void UpdateBoard(int col)
    {
        column = col;
        if (column == 0)
        {
            Mission1.SetActive(true);
            Mission2.SetActive(true);
            Mission3.SetActive(true);
            RewardText.gameObject.SetActive(true);
            StreakText.gameObject.SetActive(true);
            Scroll.SetActive(false);
        }
        else
        {
            UpdateAchievements();
            Mission1.SetActive(false);
            Mission2.SetActive(false);
            Mission3.SetActive(false);
            RewardText.gameObject.SetActive(false);
            StreakText.gameObject.SetActive(false);
            Scroll.SetActive(true);
        }
    }

    void UpdateAchievements()
    {
        Data.SpikeData data = Data.GetFromFile();
        for (int i = 0; i < 14; i++)
        {
            GameObject Ach = objectList[i];
            TMPro.TextMeshProUGUI achText = Ach.transform.Find("Status").GetComponent<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI descText = Ach.transform.Find("Desc").GetComponent<TMPro.TextMeshProUGUI>();
            if (descText.text.Contains("Storm") && !data.maps[0]) descText.text = descText.text.Replace("Storm", "the 2nd map");
            if (descText.text.Contains("Ocean") && !data.maps[1]) descText.text = descText.text.Replace("Ocean", "the 3rd map");
            if (descText.text.Contains("Space") && !data.maps[2]) descText.text = descText.text.Replace("Space", "the 4th map");
            if (data.achievements[i] == true)
            {
                achText.text = "Complete!";
                if (i == 13)
                {
                    descText.text = "Knock the asterisk off the title";
                    Ach.transform.Find("Icon").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                }
            }
            else
            {
                if (i == 6) achText.text = $"{data.lifetime}/5000 points";
                else if (i == 7) achText.text = $"{data.lifetimePowerups}/100 powerups";
                else achText.text = "Incomplete";
                Ach.GetComponent<Image>().color = new Color(0.8301887f, 0.8301887f, 0.8301887f, 1f);
                Ach.transform.Find("Icon").GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
            }
        }
    }
}
