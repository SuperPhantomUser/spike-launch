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

    */

    // Start is called before the first frame update
    public void LoadPreviewStats()
    {
        titles = new string[] { "SKIPPIT", "BOUBOLA", "DOOT", "FREN", "COMM", "A LITERAL BUBBLE", "BALLER", "QOOB", "ASTERISK", "ROSE", "LIFA", "JACK", "SUNNY" };
        pTitles = new string[] { "PLACEHOLDER 1", "PLACEHOLDER 2", "PLACEHOLDER 3", "PLACEHOLDER 4", "PLACEHOLDER 5", "PLACEHOLDER 6", "PLACEHOLDER 7", "PLACEHOLDER 8", "PLACEHOLDER 9", "PLACEHOLDER 10", "PLACEHOLDER 11", "PLACEHOLDER 12", "PLACEHOLDER 13", "PLACEHOLDER 14", "PLACEHOLDER 15" };
        descs = new string[] {
            "Known for her generic looks, which can be a bother at times... She was hit by a Danger Spike one day, and since then swore to clear them out for good.",
            "An overall well-rounded guy. They once saw a sharper version of them fly face first into a brick wall above. Decided to help them with… that.",
            "Had fren. Accidentally hurt fren. Lost fren. Now trying to regain fren.",
            "are fren. tis nice",
            "No one knows their way around the planet quite like Comm does. Of course, it's a risk having magnets injected into your spikes. Hurt quite a lot.",
            "Quite literally, a bubble. He has a deadly fear of sharp objects. He strives to join the military one day.",
            "Sent on a failed expedition to the Moon, and were quickly forgotten about. They've been stuck on the moon for so long... and they want revenge.",
            "It's… a cube? Or is it just a square? Either way, it just randomly Snap!'d into existence.",
            "See what happens when you touch stuff? Now this fell, and you can't put it back!",
            "Comm's greatest ally, and possibly even more? She's an activist and knows how to keep plants alive. Bees and butterflies keep on landing on her face.",
            "Oh look, it's a leaflike blob! Seems this lil' fella wrong-warped to here, and is willing to stay to help.",
            "It's Jack, the adorable green jelly who will go to extreme lengths to annoy people! The others tried to leave him, he came along anyway.",
            "Likes playing Among Us. She wants to experience being eaten, even though she has no idea what eating is..."
            // He has a lot of crowns, but they're all actually just spikes! Natively speaks Mandarin for some reason… Also likes beer.
            // 
            // Old EArth: Sometimes, you have to defy the laws of physics for a few micro-transactions.
        };
        pDescs = new string[] {
            "Postcard bought for 250 Spike Coins.",
            "Postcard bought for 250 Spike Coins.",
            "Postcard bought for 250 Spike Coins.",
            "Postcard bought for 250 Spike Coins.",
            "Postcard bought for 250 Spike Coins.",
            "Postcard bought for 250 Spike Coins.",
            "Postcard bought for 250 Spike Coins.",
            "Postcard bought for 250 Spike Coins.",
            "Postcard bought for 250 Spike Coins.",
            "Postcard bought for 250 Spike Coins.",
            "Postcard unlocked.",
            "Postcard unlocked.",
            "Postcard unlocked.",
            "Postcard unlocked.",
            "Postcard unlocked."
        };
        reqs = new string[] {
            "",
            "Obtain 150 lifetime points",
            "Obtain 2500 lifetime points",
            "Obtain 500 lifetime points in the Classic map",
            "Obtain 500 lifetime points in the 2nd map",
            "Obtain 500 lifetime points in the 3rd map",
            "Obtain 500 lifetime points in the 4th map",
            "Check out the credits!",
            "???",
            "Buy for 500 Spike Coins!",
            "Buy for 500 Spike Coins!",
            "Buy for 1000 Spike Coins!",
            "Buy for 1000 Spike Coins!"
        };
        pReqs = new string[] {
            "Buy for 250 Spike Coins!",
            "Buy for 250 Spike Coins!",
            "Buy for 250 Spike Coins!",
            "Buy for 250 Spike Coins!",
            "Buy for 250 Spike Coins!",
            "Buy for 250 Spike Coins!",
            "Buy for 250 Spike Coins!",
            "Buy for 250 Spike Coins!",
            "Buy for 250 Spike Coins!",
            "Buy for 250 Spike Coins!",
            "---",
            "---",
            "---",
            "---",
            "---"
        };
        progress = new string[13,2]
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
            {"", ""}
        };
        pProgress = new string[15,2]
        {
            {"", ""},
            {"", ""},
            {"", ""},
            {"", ""},
            {"", ""},
            {"", ""},
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
            if (data.lifetime >= 2500 && !data.spikes[2]) data.spikes[2] = true;
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

            if (data.lifetime >= 5000)
            {
                data.achievements[6] = true;

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
            if (selectedSpike == 13) selectedSpike = 0;
            if (selectedSpike == -1) selectedSpike = 12;
            transform.parent.gameObject.GetComponent<Animation>().Rewind();
            transform.parent.gameObject.GetComponent<Animation>().Play("Boingy");
            ReloadInfo();
        }
        else
        {
            if (forward) selectedPostcard += 1;
            else selectedPostcard -= 1;
            if (selectedPostcard == 15) selectedPostcard = 0;
            if (selectedPostcard == -1) selectedPostcard = 14;
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
                selectText.text = "Purchase";
            }
            else
            {
                selectTitle.text = "???";
                selectDesc.text = reqs[selectedSpike];
                if (selectedSpike == 4 && stormUnlocked) selectDesc.text = "Obtain 500 lifetime points in the Storm map";
                if (selectedSpike == 5 && oceanUnlocked) selectDesc.text = "Obtain 500 lifetime points in the Ocean map";
                if (selectedSpike == 6 && spaceUnlocked) selectDesc.text = "Obtain 500 lifetime points in the Space map";
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
                    selectDesc.text = selectDesc.text + Environment.NewLine + $"{count}/{progress[selectedSpike, 1]}";
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
            else if (selectedPostcard < 10)
            {
                selectTitle.text = pTitles[selectedPostcard];
                selectDesc.text = pReqs[selectedSpike];
                selectButton.SetActive(true);
                selectButton.GetComponent<Image>().color = new Color(0.992f, 0.556f, 0.680f, 1f);
                GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
                selectText.text = "Purchase";
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
                    if (progress[selectedPostcard, 0] == "lifetime") count = data.lifetime;
                    if (progress[selectedPostcard, 0] == "lifetimePowerups") count = data.lifetimePowerups;
                    if (progress[selectedPostcard, 0] == "lifetimeClassic") count = data.lifetimeClassic;
                    if (progress[selectedPostcard, 0] == "lifetimeStorm") count = data.lifetimeStorm;
                    if (progress[selectedPostcard, 0] == "lifetimeOcean") count = data.lifetimeOcean;
                    if (progress[selectedPostcard, 0] == "lifetimeSpace") count = data.lifetimeSpace;
                    selectDesc.text = selectDesc.text + Environment.NewLine + $"{count}/{progress[selectedPostcard, 1]} obtained";
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
                if (selectedSpike > 10) cost = 1000;
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
            if (selectedPostcard < 10 && !postcardsUnlocked[selectedPostcard])
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
                // view postcard
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
