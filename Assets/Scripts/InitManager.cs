using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitManager : MonoBehaviour
{
	//Keep private. We only want to have one initManager, thats why we are using singletone
	//to make sure we have only one on scene
	private static InitManager instance;

	private void Awake()
	{
		if (instance != null)
			Destroy(gameObject);
		else
			instance = this;

		DontDestroyOnLoad(this);
	}

	private void Start() => Init();

	private void Init() //Init here everything
	{
		//Sound system init
		SoundManager.UpdateVolume(PlayerPrefs.GetFloat("sound"), PlayerPrefs.GetFloat("music"));
		SoundManager.PlayMusic();
		SoundManager.PlaySound(SoundManager.Sound.WelcomeVoice);

		//Resolution init
		GameAssets.instance.InitResolutions();
	}
}
