using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using Unity.Services.Core;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class Begin : MonoBehaviour
{

    public GameObject SignInScreen;
    public Toggle toggle;

    async void Start() {
        #if UNITY_ANDROID
        try
		{
			await UnityServices.InitializeAsync();
		}
		catch (System.Exception e)
		{
			Debug.LogException(e);
		}
        PlayGamesPlatform.Activate();
        Login();
        #else
        PlayerPrefs.SetInt("GooglePlay", 0);
        Load();
	#endif
    }

    public void Load() {
        SceneManager.LoadScene("Home");
    }

    #if UNITY_ANDROID
    void Login() {
        PlayGamesPlatform.Instance.Authenticate(StartGooglePlay);
    }

    internal void StartGooglePlay(SignInStatus status) {
        if (status == SignInStatus.Success) {
            PlayerPrefs.SetInt("GooglePlay", 1);
            Load();
        } else {
            if (PlayerPrefs.GetInt("NotAgain") == 1) Load();
            else SignInScreen.SetActive(true);
        }
    }

    public void SignIn() {
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ManualAuthentication);
    }

    internal void ManualAuthentication(SignInStatus status) {
        if (status == SignInStatus.Success) {
            PlayerPrefs.SetInt("GooglePlay", 1);
        } else {
            PlayerPrefs.SetInt("GooglePlay", 0);
        }
        Load();
    }

    public void Nope() {
        PlayerPrefs.SetInt("GooglePlay", 0);
        if (toggle.isOn) PlayerPrefs.SetInt("NotAgain", 1);
        Load();
    }

    public void Policy() {
        Application.OpenURL("https://brokenpiano.org/policy/");
    }
    #endif
}
