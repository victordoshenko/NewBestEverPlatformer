using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Controller : MonoBehaviour {

	//[Tooltip("_sceneToLoadOnPlay is the name of the scene that will be loaded when users click play")]
	//public string _sceneToLoadOnPlay = "scene1";
	[Tooltip("_webpageURL defines the URL that will be opened when users click on your branding icon")]
	public string _webpageURL = "https://victordoshenko.storyland.mobi/";
	[Tooltip("_soundButtons define the SoundOn[0] and SoundOff[1] Button objects.")]
	public Button[] _soundButtons;
	[Tooltip("_audioClip defines the audio to be played on button click.")]
	public AudioClip _audioClip;
	[Tooltip("_audioSource defines the Audio Source component in this scene.")]
	public AudioSource _audioSource;
	
	void Awake () {
        Settings_Manager.Language = Application.systemLanguage;
        if (Settings_Manager.Language != SystemLanguage.Russian)
            Settings_Manager.Language = SystemLanguage.English;

        if (!PlayerPrefs.HasKey("_Mute")){
			PlayerPrefs.SetInt("_Mute", 0);
		}

        //PlayerPrefs.SetString("_LastScene", "scene1");
        
        if (!PlayerPrefs.HasKey("_LastScene"))
        {
            PlayerPrefs.SetString("_LastScene", "scene1");
        }        

        if (PlayerPrefs.GetInt("_Mute") == 0)
            Unmute();
        else
            Mute();
	}
	
	public void OpenWebpage () {
		_audioSource.PlayOneShot(_audioClip);
		Application.OpenURL(_webpageURL);
	}
	
	public void PlayGame () {
		_audioSource.PlayOneShot(_audioClip);
        //UnityEngine.SceneManagement.SceneManager.LoadScene(PlayerPrefs.GetString("_LastScene"));
        UnityEngine.SceneManagement.SceneManager.LoadScene("SelectLevel");
    }

    public void PlayAbout()
    {
        _audioSource.PlayOneShot(_audioClip);
        UnityEngine.SceneManagement.SceneManager.LoadScene("AboutScene");
    }

    public void Mute () {
		_audioSource.PlayOneShot(_audioClip);
		_soundButtons[0].interactable = true;
		_soundButtons[1].interactable = false;
		PlayerPrefs.SetInt("_Mute", 1);
	}
	
	public void Unmute () {
		_audioSource.PlayOneShot(_audioClip);
		_soundButtons[0].interactable = false;
		_soundButtons[1].interactable = true;
		PlayerPrefs.SetInt("_Mute", 0);
	}
	
	public void QuitGame () {
		_audioSource.PlayOneShot(_audioClip);
		#if !UNITY_EDITOR
			Application.Quit();
		#endif
		
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}
