using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
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

    private string[] titles;
    private string[] descs;
    private string[] reqs;
    public Sprite[] sprites;

    public bool[] unlocked;

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
        titles = new string[] {"SKIPPIT", "BOUBOLA", "DOOT", "FREN", "COMM", "A LITERAL BUBBLE", "BALLER", "QOOB", "ASTERISK", "ROSE", "LIFA", "JACK", "SUNNY"};
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
            SpikeData data = new SpikeData();
            data.coins = 0;
            data.equipped = 0;
            data.spikes = new bool[] {true, false, false, false, false, false, false, false, false, false, false, false, false};
            data.titleCard = false;
            data.checkedCredits = false;
            selectedSpike = data.equipped;
            unlocked = data.spikes;
            coinText.text = "0";
	        SaveToFile(data);
            stormUnlocked = false;
            oceanUnlocked = false;
            spaceUnlocked = false;
            ReloadInfo();
            TitleCard.GetComponent<Bounce>().canDo = true;
        } else {
            SpikeData data = GetFromFile();
            if (data.lifetime >= 150 && !data.spikes[1]) data.spikes[1] = true;
            if (data.lifetime >= 2500 && !data.spikes[2]) data.spikes[2] = true;
            if (data.lifetimeClassic >= 500 && !data.spikes[3]) data.spikes[3] = true;
            if (data.lifetimeStorm >= 500 && !data.spikes[4]) data.spikes[4] = true;
            if (data.lifetimeOcean >= 500 && !data.spikes[5]) data.spikes[5] = true;
            if (data.lifetimeSpace >= 500 && !data.spikes[6]) data.spikes[6] = true;

            if (data.highClassic[0] >= 100) {
                if (!data.maps[0]) data.maps[0] = true;
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQAA", 100.0f, (bool success) => {});
            }
            if (data.highClassic[0] >= 150) {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQAQ", 100.0f, (bool success) => {});
            }
            if (data.highClassic[0] >= 200) {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQAg", 100.0f, (bool success) => {});
            }

            if (data.highStorm[0] >= 50) {
                if (!data.maps[1]) data.maps[1] = true;
            }
            if (data.highStorm[0] >= 100) {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQBA", 100.0f, (bool success) => {});
            }
            if (data.highOcean[0] >= 100) {
                if (!data.maps[2]) data.maps[2] = true;
            }
            if (data.highOcean[0] >= 150) {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQBQ", 100.0f, (bool success) => {});
            }
            if (data.highSpace[0] >= 80) {
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQBg", 100.0f, (bool success) => {});
            }

            if (data.checkedCredits) {
                data.spikes[7] = true;
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQCw", 100.0f, (bool success) => {});
            }
            if (data.titleCard) {
                data.spikes[8] = true;
                if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQDA", 100.0f, (bool success) => {});
            }

            selectedSpike = data.equipped;
            equippedSpike = data.equipped;
            unlocked = data.spikes;
            Debug.Log(unlocked.Length);
            coinText.text = $"{data.coins}";
            if (data.titleCard) {
                TitleCard.GetComponent<Bounce>().canDo = false;
                TitleCard.GetComponent<Image>().sprite = AlternateSprite;
            } else {
                TitleCard.GetComponent<Bounce>().canDo = true;
            }
            stormUnlocked = data.maps[0];
            oceanUnlocked = data.maps[1];
            spaceUnlocked = data.maps[2];
            ReloadInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedSpike == 6) transform.rotation = Quaternion.identity;
        else transform.Rotate(0f, 0f, -180f * Time.deltaTime);
    }

    public void EquipSpike() {
        equippedSpike = selectedSpike;
        SpikeData data = GetFromFile();
        data.equipped = equippedSpike;
	    SaveToFile(data);
    }

    public void SelectSpike(bool forward) {
        if (forward) { // asterisk = 8
            if (selectedSpike == 7 && unlocked[8] == false) selectedSpike = 9;
            else selectedSpike += 1;
        }
        else {
            if (selectedSpike == 9 && unlocked[8] == false) selectedSpike = 7;
            else selectedSpike -= 1;
        }
        if (selectedSpike == 13) selectedSpike = 0;
        if (selectedSpike == -1) selectedSpike = 12;
        transform.parent.gameObject.GetComponent<Animation>().Rewind();
        transform.parent.gameObject.GetComponent<Animation>().Play("Boingy");
        ReloadInfo();
    }

    void ReloadInfo() {
        selectSprite.sprite = sprites[selectedSpike];
        if (unlocked[selectedSpike]) {
            selectTitle.text = titles[selectedSpike];
            selectDesc.text = descs[selectedSpike];
            selectButton.SetActive(true);
            GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            if (equippedSpike == selectedSpike) {
                selectButton.GetComponent<Image>().color = new Color(0.622f, 0.622f, 0.622f, 1f);
                selectText.text = "Equipped";
            }
            else {
                selectButton.GetComponent<Image>().color = new Color(0.988f, 0.990f, 0.555f, 1f); //FCFD8E (original)
                selectText.text = "Equip";
            }
        } else if (selectedSpike > 8) {
            selectTitle.text = "???";
            selectDesc.text = reqs[selectedSpike];
            selectButton.SetActive(true);
            selectButton.GetComponent<Image>().color = new Color(0.992f, 0.556f, 0.680f, 1f);
            GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
            selectText.text = "Purchase";
        } else {
            selectTitle.text = "???";
            selectDesc.text = reqs[selectedSpike];
            if (selectedSpike == 4 && stormUnlocked) selectDesc.text = "Obtain 500 lifetime points in the Storm map";
            if (selectedSpike == 5 && oceanUnlocked) selectDesc.text = "Obtain 500 lifetime points in the Ocean map";
            if (selectedSpike == 6 && spaceUnlocked) selectDesc.text = "Obtain 500 lifetime points in the Space map";
            GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f);
            selectButton.SetActive(false);
        }
    }

    public void Select() {
        if (selectedSpike > 8 && !unlocked[selectedSpike]) {
            SpikeData data = GetFromFile();
            int cost;
            if (selectedSpike > 10) cost = 1000;
            else cost = 500;
		    if (data.coins >= cost) {
                data.coins -= cost;
                unlocked[selectedSpike] = true;
                equippedSpike = selectedSpike;
                data.spikes[selectedSpike] = true;
                data.equipped = equippedSpike;
                coinText.text = $"{data.coins}";
                if (Control.playSound) PurchaseSource.Play();
                SaveToFile(data);
                ReloadInfo();
            } else {
                if (Control.playSound) NupSource.Play();
            }
        }
        else if (equippedSpike != selectedSpike) {
		    SpikeData data = GetFromFile();
            unlocked[selectedSpike] = true;
            equippedSpike = selectedSpike;
            data.equipped = equippedSpike;
            if (Control.playSound) YupSource.Play();
            SaveToFile(data);
            ReloadInfo();
        }
    }
    
    public void AsteriskGot() {
        SpikeData data = GetFromFile();
        data.titleCard = true;
        data.spikes[8] = true;
        data.equipped = 8;
        unlocked = data.spikes;
        equippedSpike = 8;
        selectedSpike = 8;
	    SaveToFile(data);
        ReloadInfo();
        TitleCard.GetComponent<Image>().sprite = AlternateSprite;
        if (PlayerPrefs.GetInt("GooglePlay") == 1) Social.ReportProgress("CgkIqPj-8swdEAIQDA", 100.0f, (bool success) => {});
    }
}
