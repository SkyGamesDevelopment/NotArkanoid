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
		int score = SaveManager.GetHighscore();

		if (score == -1)
		{
			ToggleContinueButton(false);
			UpdateHighscore(0);
		}
		else
		{
			ToggleContinueButton(true);
			UpdateHighscore(score);
		}

		UpdateSliders(SaveManager.GetVolume());
	}

	public void OnNewGameButtonClicked()
	{
		SaveManager.SaveVolume();
		SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
		GameManager.instance.StartNewGame();
	}

	public void OnContinueButtonClicked()
	{
		SaveManager.SaveVolume();
		SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
		GameManager.instance.ContinueGame();
	}

	public void OnExitButtonClicked()
	{
		SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
		SaveManager.SaveVolume();
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

	public void UpdateSliders(VolumeData data)
	{
		music.value = data.musicVolume;
		sound.value = data.soundVolume;
	}

	public void OnMusicValueChanged()
	{
		SoundManager.UpdateVolume(new VolumeData(SoundManager.soundVolume, music.value));
	}

	public void OnSoundValueChanged()
	{
		SoundManager.UpdateVolume(new VolumeData(sound.value, SoundManager.musicVolume));
	}
}
