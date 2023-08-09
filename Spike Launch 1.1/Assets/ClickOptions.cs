using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickOptions : MonoBehaviour
{

    public int id;

    public Sprite full;
    public Sprite half;
    public Sprite none;

    public HomeControl HomeControl;
    public GameObject CrowdedButton;
    public GameObject NormalButton;
    public GameObject RelaxedButton;
    public TMPro.TextMeshProUGUI modeText;
    public bool crowdedShow;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("CrowdedMode") == 1 && id == 1) CrowdedOn();
        if (PlayerPrefs.GetInt("Relaxed") == 1 && id == 1) Relaxed();
        else if (id == 1) Normal();
        if (id == 1) {
            int musicVol = PlayerPrefs.GetInt("MusicVolume");
            if (musicVol == 0) {
                musicVol = 100;
                PlayerPrefs.SetInt("MusicVolume", 100);
            }
            if (musicVol == 100) GetComponent<Image>().sprite = full;
            else if (musicVol == 50) GetComponent<Image>().sprite = half;
            else if (musicVol == -1) GetComponent<Image>().sprite = none;
        } else {
            int soundVol = PlayerPrefs.GetInt("SoundVolume");
            if (soundVol == 0) {
                soundVol = 100;
                PlayerPrefs.SetInt("SoundVolume", 100);
            }
            if (soundVol == 100) GetComponent<Image>().sprite = full;
            else if (soundVol == -1) GetComponent<Image>().sprite = none;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click() {
        if (id == 1) {
            int vol = PlayerPrefs.GetInt("MusicVolume");
            if (vol == 100) {
                PlayerPrefs.SetInt("MusicVolume", 50);
                GetComponent<Image>().sprite = half;
                HomeControl.ReloadVolume();
            }
            else if (vol == 50) {
                PlayerPrefs.SetInt("MusicVolume", -1);
                GetComponent<Image>().sprite = none;
                HomeControl.ReloadVolume();
            }
            else {
                PlayerPrefs.SetInt("MusicVolume", 100);
                GetComponent<Image>().sprite = full;
                HomeControl.ReloadVolume();
            }
        } else {
            int vol = PlayerPrefs.GetInt("SoundVolume");
            if (vol == 100) {
                PlayerPrefs.SetInt("SoundVolume", -1);
                GetComponent<Image>().sprite = none;
                HomeControl.ReloadVolume();
            }
            else {
                PlayerPrefs.SetInt("SoundVolume", 100);
                GetComponent<Image>().sprite = full;
                HomeControl.ReloadVolume();
            }
        }
    }

    public void Toggle() {
        crowdedShow = !crowdedShow;
        if (crowdedShow) CrowdedOn();
        else CrowdedOff();
        HomeControl.ReloadVolume();
    }

    public void Normal()
    {
        PlayerPrefs.SetInt("Relaxed", 0);
        NormalButton.GetComponent<Image>().color = new Color(0.229f, 0.411f, 0.774f, 1f);
        RelaxedButton.GetComponent<Image>().color = new Color(0.5188679f, 0.5188679f, 0.5188679f, 1f);
        modeText.text = "Classic Spike Launch gameplay.";
    }

    public void Relaxed() {
        PlayerPrefs.SetInt("Relaxed", 1);
        RelaxedButton.GetComponent<Image>().color = new Color(0.2268423f, 0.6792453f, 0.5621927f, 1f);
        NormalButton.GetComponent<Image>().color = new Color(0.5188679f, 0.5188679f, 0.5188679f, 1f);
        modeText.text = "Games are slower and easier.";
    }

    void CrowdedOn() {
        crowdedShow = true;
        PlayerPrefs.SetInt("CrowdedMode", 1);
        CrowdedButton.GetComponent<Image>().color = new Color(0.2662869f, 0.4077168f, 0.754717f, 1f);
    }

    void CrowdedOff() {
        crowdedShow = false;
        PlayerPrefs.SetInt("CrowdedMode", 0);
        CrowdedButton.GetComponent<Image>().color = new Color(0.5188679f, 0.5188679f, 0.5188679f, 1f);
    }
}
