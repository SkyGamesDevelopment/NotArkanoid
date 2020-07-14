using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	#region variables
	[SerializeField]
	private GameObject ContinueButton;
	[SerializeField]
	private TextMeshProUGUI highscore;
	[SerializeField]
	private Slider music, sound;
	#endregion

	private void Start()
	{
		if (SaveManager.CanContinue())
			ToggleContinueButton(true);
		else
			ToggleContinueButton(false);

		UpdateHighscore(PlayerPrefs.GetInt("highscore"));
		UpdateSliders(PlayerPrefs.GetFloat("music"), PlayerPrefs.GetFloat("sound"));
		UpdateResolutions();
	}

	public void OnNewGameButtonClicked()
	{
		PlayerPrefs.SetFloat("music", music.value);
		PlayerPrefs.SetFloat("sound", sound.value);
		SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
		GameManager.instance.StartNewGame();
	}

	public void OnContinueButtonClicked()
	{
		PlayerPrefs.SetFloat("music", music.value);
		PlayerPrefs.SetFloat("sound", sound.value);
		SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
		GameManager.instance.ContinueGame();
	}

	public void OnExitButtonClicked()
	{
		SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
		PlayerPrefs.SetFloat("music", music.value);
		PlayerPrefs.SetFloat("sound", sound.value);
		Application.Quit();
	}

	private void ToggleContinueButton(bool show)
	{
		ContinueButton.SetActive(show);
	}

	private void UpdateHighscore(int score)
	{
		highscore.text = "Highscore \n" + score;
	}

	public void UpdateSliders(float musicVol, float soundVol)
	{
		music.value = musicVol;
		sound.value = soundVol;
	}

	public void OnMusicValueChanged()
	{
		SoundManager.UpdateVolume(SoundManager.soundVolume, music.value);
	}

	public void OnSoundValueChanged()
	{
		SoundManager.UpdateVolume(sound.value, SoundManager.musicVolume);
	}

	private void UpdateResolutions()
	{
		//TODO
	}
}
