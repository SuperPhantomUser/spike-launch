using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class Preview : MonoBehaviour
{

    public int selectedSpike;
    public int equippedSpike;
    public int selectedPostcard;
    public int selectedChoice;

    public GameObject skinsButton;
    public GameObject postcardsButton;

    private string[] titles;
    private string[] descs;
    private string[] reqs;
    private string[,] progress;
    private string[] pTitles;
    private string[] pDescs;
    private string[] pReqs;
    private string[,] pProgress;
    public Sprite[] sprites;
    public Sprite[] postcards;

    public bool[] skinsUnlocked;
    public bool[] postcardsUnlocked;

    public Image selectSprite;
    public TMPro.TextMeshProUGUI selectTitle;
    public TMPro.TextMeshProUGUI selectDesc;
    public GameObject selectButton;
    public TMPro.TextMeshProUGUI selectText;
    public TMPro.TextMeshProUGUI coinText;

    public HomeControl Control;

    public GameObject TitleCard;

    public Sprite AlternateSprite;

    public bool stormUnlocked;
    public bool oceanUnlocked;
    public bool spaceUnlocked;

    public AudioSource PurchaseSource;
    public AudioSource YupSource;
    public AudioSource NupSource;

    public Image ViewImage;
    public TMPro.TextMeshProUGUI ViewTitle;

    public Data Data;

    /*

    IDs:
    Skippit = 0
    Boubola = 1
    Doot = 2
    Fren = 3
    Comm = 4
    A literal bubble = 5
    Baller = 6
    Qoob = 7
    Asterisk = 8
    Rose = 9
    Lifa = 10
    Jack = 11
    Sunny = 12
    Flippy = 13
    Volley = 14
    Earth = 15

    */

    // Start is called before the first frame update
    public void LoadPreviewStats()
    {
        titles = new string[] { 
            "SKIPPIT", 
            "BOUBOLA", 
            "DOOT", 
            "FREN", 
            "COMM", 
            "A LITERAL BUBBLE", 
            "BALLER", 
            "QOOB", 
            "ASTERISK", 
            "ROSE", 
            "LIFA", 
            "FLIPPY", 
            "VOLLEY", 
            "JACK", 
            "SUNNY", 
            "EARTH" 
        };
        pTitles = new string[] { 
            "A PAINLESS PROCEDURE", 
            "HEADS OR TAILS", 
            "COMM'S CONFESSION", 
            "TO THE POINT", 
            "THE LOCAL HOBO", 
            "FIRST SPIKE LAUNCH", 
            "LANDING?", 
            "DANGER SPIKE", 
            "NEW FREN", 
            "THE BRICK WALL", 
            "LETHAL EMBRACE", 
            "THE PLAN"
        };
        descs = new string[] {
            "Known for her generic looks, which can be a bother at times... She was hit by a Danger Spike one day, and since then swore to clear them out for good.",
            "An overall well-rounded guy. They once saw a sharper version of them fly face first into a brick wall above. Decided to help them with... that.",
            "Had fren. Accidentally hurt fren. Lost fren. Now trying to regain fren.",
            "are fren. tis nice",
            "No one knows their way around the planet quite like Comm does. Of course, it's a risk having magnets injected into your spikes. Hurt quite a lot.",
            "Quite literally, a bubble. He has a deadly fear of sharp objects. He strives to join the military one day.",
            "Sent on a failed expedition to the Moon, and were quickly forgotten about. They've been stuck on the moon for so long... and they want revenge.",
            "It's... a cube? Or is it just a square? Either way, it just randomly Snap!'d into existence.",
            "See what happens when you touch stuff? Now this fell, and you can't put it back!",
            "Comm's greatest ally, and possibly even more? She's an activist and knows how to keep plants alive. Bees and butterflies keep on landing on her face.",
            "Oh look, it's a leaflike blob! Seems this lil' fella wrong-warped to here, and is willing to stay to help.",
            "They say if you flip this coin, it gives you money in return. I bet if you flipped one enough, you could buy out every store... Too bad this one's set on crate-breaking.",
            "This spikeball was born without a mouth... or spikes. It seems to like lounging about in pools on hot summer days. What it doesn't like? Being spiked around.",
            "It's Jack, the adorable green jelly who will go to extreme lengths to annoy people! The others tried to leave him, he came along anyway.",
            "Likes playing Among Us. She wants to experience being eaten, even though she has no idea what eating is...",
            "Oh hey, it's the third planet from the Sun! Unsure how, but now you've got it! Just... don't blow it up, okay?"
            // He has a lot of crowns, but they're all actually just spikes! Natively speaks Mandarin for some reason... Also likes beer.
            // 
            // Sometimes, you have to defy the laws of physics for a few micro-transactions.
        };
        pDescs = new string[] {
            "Postcard bought for 250 Spike Coins. Click to view it!",
            "Postcard bought for 250 Spike Coins. Click to view it!",
            "Postcard bought for 250 Spike Coins. Click to view it!",
            "Postcard bought for 250 Spike Coins. Click to view it!",
            "Postcard bought for 250 Spike Coins. Click to view it!",
            "Postcard bought for 250 Spike Coins. Click to view it!",
            "Postcard bought for 250 Spike Coins. Click to view it!",
            "Postcard unlocked. Click to view it!",
            "Postcard unlocked. Click to view it!",
            "Postcard unlocked. Click to view it!",
            "Postcard unlocked. Click to view it!",
            "Postcard unlocked. Click to view it!"
        };
        reqs = new string[] {
            "",
            "Obtain 150 lifetime points",
            "Obtain 2,500 lifetime points",
            "Obtain 500 lifetime points in Classic",
            "Obtain 500 lifetime points in the 2nd map",
            "Obtain 500 lifetime points in the 3rd map",
            "Obtain 500 lifetime points in the 4th map",
            "Check out the credits!",
            "???",
            "Buy this character for $500!",
            "Buy this character for $500!",
            "Buy this character for $500!",
            "Buy this character for $500!",
            "Buy this character for $1000!",
            "Buy this character for $1000!",
            "Buy this character for $1000!"
        };
        pReqs = new string[] {
            "Buy this postcard for $250!",
            "Buy this postcard for $250!",
            "Buy this postcard for $250!",
            "Buy this postcard for $250!",
            "Buy this postcard for $250!",
            "Buy this postcard for $250!",
            "Buy this postcard for $250!",
            "Obtain 1,000 lifetime points",
            "Obtain 2,500 lifetime points",
            "Obtain 3,500 lifetime points",
            "Obtain 5,000 lifetime points",
            "Obtain 10,000 lifetime points"
        };
        progress = new string[16,2]
        {
            {"", ""},
            {"lifetime", "150"},
            {"lifetime", "2500"},
            {"lifetimeClassic", "500"},
            {"lifetimeStorm", "500"},
            {"lifetimeOcean", "500"},
            {"lifetimeSpace", "500"},
            {"", ""},
            {"", ""},
            {"", ""},
            {"", ""},
            {"", ""},
            {"", ""},
            {"", ""},
            {"", ""},
            {"", ""}
        };
        pProgress = new string[12,2]
        {
            {"", ""},
            {"", ""},
            {"", ""},
            {"", ""},
            {"", ""},
            {"", ""},
            {"", ""},
            {"lifetime", "1000"},
            {"lifetime", "2500"},
            {"lifetime", "3500"},
            {"lifetime", "5000"},
            {"lifetime", "10000"}
        };

        /*

        REQUIREMENTS:

        Skippit: default
        Boubola: 150 lifetime points
        Doot: 1000 lifetime points
        Fren: score of 150 in Classic without powerups
        Comm: score of 150 in Storm without missing a crate
        A literal bubble: score of 150 in Ocean without touching crates
        Baller: highscore of 150 in Space while staying in the lower half
        Qoob: check out the credits
        Asterisk: knock it off the title card
        Rose: purchase
        Earth: purchase
        Cory: purchase
        Sunny: purchase

        */

        if (!File.Exists(Application.persistentDataPath + "/SpikeData.json")) {
            Data.SpikeData data = new Data.SpikeData();
            selectedSpike = data.equipped;
            skinsUnlocked = data.spikes;
            postcardsUnlocked = data.postcards;
            coinText.text = "$0";
	        Data.SaveToFile(data);
            stormUnlocked = false;
            oceanUnlocked = false;
            spaceUnlocked = false;
            TitleCard.GetComponent<Bounce>().canDo = true;
            SelectChoice(0);
        } else {
            Data.SpikeData data = Data.GetFromFile();
            if (data.lifetime >= 150 && !data.spikes[1]) data.spikes[1] = true;
            if (data.lifetime >= 1000 && !data.postcards[7]) data.postcards[7] = true;
            if (data.lifetime >= 2500 && !data.spikes[2]) data.spikes[2] = true;
            if (data.lifetime >= 2500 && !data.postcards[8]) data.postcards[8] = true;
            if (data.lifetime >= 3500 && !data.postcards[9]) data.postcards[9] = true;
            if (data.lifetime >= 5000 && !data.postcards[10]) data.postcards[10] = true;
            if (data.lifetime >= 5000 && !data.achievements[6]) data.achievements[6] = true;
            if (data.lifetime >= 10000 && !data.postcards[11]) data.postcards[11] = true;

            if (data.lifetimeClassic >= 500 && !data.spikes[3]) data.spikes[3] = true;
            if (data.lifetimeStorm >= 500 && !data.spikes[4]) data.spikes[4] = true;
            if (data.lifetimeOcean >= 500 && !data.spikes[5]) data.spikes[5] = true;
            if (data.lifetimeSpace >= 500 && !data.spikes[6]) data.spikes[6] = true;

            if (data.highClassic[0] >= 100) {
                data.maps[0] = true;
                data.achievements[0] = true;
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQAA", 100.0f, (bool success) => {});
            }
            if (data.highClassic[0] >= 150) {
                data.achievements[1] = true;
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQAQ", 100.0f, (bool success) => {});
            }
            if (data.highClassic[0] >= 200) {
                data.achievements[2] = true;
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQAg", 100.0f, (bool success) => {});
            }

            if (data.highStorm[0] >= 50) {
                if (!data.maps[1]) data.maps[1] = true;
            }
            if (data.highStorm[0] >= 100) {
                data.achievements[3] = true;
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQBA", 100.0f, (bool success) => {});
            }
            if (data.highOcean[0] >= 100) {
                if (!data.maps[2]) data.maps[2] = true;
            }
            if (data.highOcean[0] >= 150) {
                data.achievements[4] = true;
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQBQ", 100.0f, (bool success) => {});
            }
            if (data.highSpace[0] >= 80) {
                data.achievements[5] = true;
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQBg", 100.0f, (bool success) => {});
            }

            if (data.lifetimePowerups >= 100)
            {
                data.achievements[7] = true;
            }

            if (data.checkedCredits) {
                data.spikes[7] = true;
                data.achievements[12] = true;
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQCw", 100.0f, (bool success) => {});
            }
            if (data.titleCard) {
                data.spikes[8] = true;
                data.achievements[13] = true;
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQDA", 100.0f, (bool success) => {});
            }

            if (PlayerPrefs.GetInt("GooglePlay") == 1 && (data.checkedGooglePlay == false || data.checkedGooglePlay == null))
            {
                data.checkedGooglePlay = true;
                Social.LoadAchievements(achievements =>
                {
                    if (achievements.Length > 0)
                    {
                        string[] achList = new string[14]
                        {
                            "CgkIqPj-8swdEAIQAA", "CgkIqPj-8swdEAIQAQ", "CgkIqPj-8swdEAIQAg", "CgkIqPj-8swdEAIQBA", "CgkIqPj-8swdEAIQBQ", "CgkIqPj-8swdEAIQBg",
                            "CgkIqPj-8swdEAIQAw", "CgkIqPj-8swdEAIQEA",
                            "CgkIqPj-8swdEAIQBw", "CgkIqPj-8swdEAIQCA", "CgkIqPj-8swdEAIQCQ", "CgkIqPj-8swdEAIQCg",
                            "CgkIqPj-8swdEAIQCw", "CgkIqPj-8swdEAIQDA"
                        };
                        foreach (IAchievement achievement in achievements)
                        {
                            data.achievements[Array.IndexOf(achList, achievement.id)] = true;
                        }
                    }
                });
            }

            Data.SaveToFile(data);
            selectedSpike = data.equipped;
            equippedSpike = data.equipped;
            skinsUnlocked = data.spikes;
            postcardsUnlocked = data.postcards;
            coinText.text = $"${data.coins}";
            if (data.titleCard) {
                TitleCard.GetComponent<Bounce>().canDo = false;
                TitleCard.GetComponent<Image>().sprite = AlternateSprite;
            } else {
                TitleCard.GetComponent<Bounce>().canDo = true;
            }
            stormUnlocked = data.maps[0];
            oceanUnlocked = data.maps[1];
            spaceUnlocked = data.maps[2];
            SelectChoice(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedSpike == 6 || selectedChoice == 1) transform.rotation = Quaternion.identity;
        else transform.Rotate(0f, 0f, -180f * Time.deltaTime);
    }

    public void EquipSpike() {
        equippedSpike = selectedSpike;
        Data.SpikeData data = Data.GetFromFile();
        data.equipped = equippedSpike;
	    Data.SaveToFile(data);
    }

    public void SelectSpike(bool forward) {
        if (selectedChoice == 0)
        {
            if (forward)
            { // asterisk = 8
                if (selectedSpike == 7 && skinsUnlocked[8] == false) selectedSpike = 9;
                else selectedSpike += 1;
            }
            else
            {
                if (selectedSpike == 9 && skinsUnlocked[8] == false) selectedSpike = 7;
                else selectedSpike -= 1;
            }
            if (selectedSpike == 16) selectedSpike = 0;
            if (selectedSpike == -1) selectedSpike = 15;
            transform.parent.gameObject.GetComponent<Animation>().Rewind();
            transform.parent.gameObject.GetComponent<Animation>().Play("Boingy");
            ReloadInfo();
        }
        else
        {
            if (forward) selectedPostcard += 1;
            else selectedPostcard -= 1;
            if (selectedPostcard == 12) selectedPostcard = 0;
            if (selectedPostcard == -1) selectedPostcard = 11;
            transform.parent.gameObject.GetComponent<Animation>().Rewind();
            transform.parent.gameObject.GetComponent<Animation>().Play("Boingy");
            ReloadInfo();
        }
    }

    void ReloadInfo() {
        if (selectedChoice == 0)
        {
            selectSprite.sprite = sprites[selectedSpike];
            if (skinsUnlocked[selectedSpike])
            {
                selectTitle.text = titles[selectedSpike];
                selectDesc.text = descs[selectedSpike];
                selectButton.SetActive(true);
                GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                if (equippedSpike == selectedSpike)
                {
                    selectButton.GetComponent<Image>().color = new Color(0.622f, 0.622f, 0.622f, 1f);
                    selectText.text = "Equipped";
                }
                else
                {
                    selectButton.GetComponent<Image>().color = new Color(0.988f, 0.990f, 0.555f, 1f); //FCFD8E (original)
                    selectText.text = "Equip";
                }
            }
            else if (selectedSpike > 8)
            {
                selectTitle.text = "???";
                selectDesc.text = reqs[selectedSpike];
                selectButton.SetActive(true);
                selectButton.GetComponent<Image>().color = new Color(0.992f, 0.556f, 0.680f, 1f);
                GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
                int cost = 500;
                if (selectedSpike > 12) cost = 1000;
                selectText.text = $"${cost}";
            }
            else
            {
                selectTitle.text = "???";
                selectDesc.text = reqs[selectedSpike];
                if (selectedSpike == 4 && stormUnlocked) selectDesc.text = "Obtain 500 lifetime points in Storm";
                if (selectedSpike == 5 && oceanUnlocked) selectDesc.text = "Obtain 500 lifetime points in Ocean";
                if (selectedSpike == 6 && spaceUnlocked) selectDesc.text = "Obtain 500 lifetime points in Space";
                if (progress[selectedSpike, 0].Length > 0)
                {
                    Data.SpikeData data = Data.GetFromFile();
                    int count = 0;
                    if (progress[selectedSpike, 0] == "lifetime") count = data.lifetime;
                    if (progress[selectedSpike, 0] == "lifetimePowerups") count = data.lifetimePowerups;
                    if (progress[selectedSpike, 0] == "lifetimeClassic") count = data.lifetimeClassic;
                    if (progress[selectedSpike, 0] == "lifetimeStorm") count = data.lifetimeStorm;
                    if (progress[selectedSpike, 0] == "lifetimeOcean") count = data.lifetimeOcean;
                    if (progress[selectedSpike, 0] == "lifetimeSpace") count = data.lifetimeSpace;
                    selectDesc.text = selectDesc.text + Environment.NewLine + $"{count}/{progress[selectedSpike, 1]} obtained";
                }
                GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
                selectButton.SetActive(false);
            }
        } else
        {
            selectSprite.sprite = postcards[selectedPostcard];
            if (postcardsUnlocked[selectedPostcard])
            {
                selectTitle.text = pTitles[selectedPostcard];
                selectDesc.text = pDescs[selectedPostcard];
                selectButton.SetActive(true);
                GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                selectButton.GetComponent<Image>().color = new Color(0.988f, 0.990f, 0.555f, 1f);
                selectText.text = "View";
            }
            else if (selectedPostcard < 7)
            {
                selectTitle.text = pTitles[selectedPostcard];
                selectDesc.text = pReqs[selectedSpike];
                selectButton.SetActive(true);
                selectButton.GetComponent<Image>().color = new Color(0.992f, 0.556f, 0.680f, 1f);
                GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
                selectText.text = "$250";
            }
            else
            {
                selectTitle.text = pTitles[selectedPostcard];
                selectDesc.text = pReqs[selectedPostcard];
                //if (selectedSpike == 4 && stormUnlocked) selectDesc.text = "Obtain 500 lifetime points in the Storm map";
                //if (selectedSpike == 5 && oceanUnlocked) selectDesc.text = "Obtain 500 lifetime points in the Ocean map";
                //if (selectedSpike == 6 && spaceUnlocked) selectDesc.text = "Obtain 500 lifetime points in the Space map";
                if (pProgress[selectedPostcard, 0].Length > 0)
                {
                    Data.SpikeData data = Data.GetFromFile();
                    int count = 0;
                    if (pProgress[selectedPostcard, 0] == "lifetime") count = data.lifetime;
                    if (pProgress[selectedPostcard, 0] == "lifetimePowerups") count = data.lifetimePowerups;
                    if (pProgress[selectedPostcard, 0] == "lifetimeClassic") count = data.lifetimeClassic;
                    if (pProgress[selectedPostcard, 0] == "lifetimeStorm") count = data.lifetimeStorm;
                    if (pProgress[selectedPostcard, 0] == "lifetimeOcean") count = data.lifetimeOcean;
                    if (pProgress[selectedPostcard, 0] == "lifetimeSpace") count = data.lifetimeSpace;
                    selectDesc.text = selectDesc.text + Environment.NewLine + $"{count}/{pProgress[selectedPostcard, 1]} obtained";
                }
                GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
                selectButton.SetActive(false);
            }
        }
    }

    public void Select()
    {
        if (selectedChoice == 0)
        {
            if (selectedSpike > 8 && !skinsUnlocked[selectedSpike])
            {
                Data.SpikeData data = Data.GetFromFile();
                int cost;
                if (selectedSpike > 12) cost = 1000;
                else cost = 500;
                if (data.coins >= cost)
                {
                    data.coins -= cost;
                    skinsUnlocked[selectedSpike] = true;
                    equippedSpike = selectedSpike;
                    data.spikes[selectedSpike] = true;
                    data.equipped = equippedSpike;
                    coinText.text = $"${data.coins}";
                    if (Control.playSound) PurchaseSource.Play();
                    Data.SaveToFile(data);
                    ReloadInfo();
                }
                else
                {
                    if (Control.playSound) NupSource.Play();
                }
            }
            else if (equippedSpike != selectedSpike)
            {
                Data.SpikeData data = Data.GetFromFile();
                skinsUnlocked[selectedSpike] = true;
                equippedSpike = selectedSpike;
                data.equipped = equippedSpike;
                if (Control.playSound) YupSource.Play();
                Data.SaveToFile(data);
                ReloadInfo();
            }
        }
        else
        {
            if (selectedPostcard < 7 && !postcardsUnlocked[selectedPostcard])
            {
                Data.SpikeData data = Data.GetFromFile();
                if (data.coins >= 250)
                {
                    data.coins -= 250;
                    postcardsUnlocked[selectedPostcard] = true;
                    data.postcards[selectedPostcard] = true;
                    coinText.text = $"${data.coins}";
                    if (Control.playSound) PurchaseSource.Play();
                    Data.SaveToFile(data);
                    ReloadInfo();
                }
                else
                {
                    if (Control.playSound) NupSource.Play();
                }
            }
            else
            {
                ViewImage.sprite = postcards[selectedPostcard];
                ViewTitle.text = pTitles[selectedPostcard];
                ViewImage.transform.parent.gameObject.SetActive(true);
            }
        }
    }
    
    public void AsteriskGot() {
        Data.SpikeData data = Data.GetFromFile();
        data.titleCard = true;
        data.spikes[8] = true;
        data.equipped = 8;
        skinsUnlocked = data.spikes;
        postcardsUnlocked = data.postcards;
        equippedSpike = 8;
        selectedSpike = 8;
        data.achievements[13] = true;
	    Data.SaveToFile(data);
        ReloadInfo();
        TitleCard.GetComponent<Image>().sprite = AlternateSprite;
        if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQDA", 100.0f, (bool success) => {});
    }

    public void SelectChoice(int choice)
    {
        if (choice == 0)
        {
            if (selectedChoice == 1) selectedSpike = 0;
            skinsButton.GetComponent<Image>().color = new Color(0.944f, 1f, 0.598f, 1f);
            postcardsButton.GetComponent<Image>().color = new Color(0.830f, 0.830f, 0.830f, 1f);
        } else
        {
            if (selectedChoice == 0) selectedPostcard = 0;
            postcardsButton.GetComponent<Image>().color = new Color(0.944f, 1f, 0.598f, 1f);
            skinsButton.GetComponent<Image>().color = new Color(0.830f, 0.830f, 0.830f, 1f);
        }
        selectedChoice = choice;
        ReloadInfo();
    }

}
