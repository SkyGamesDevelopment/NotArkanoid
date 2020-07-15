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

	[SerializeField]
	private TMP_Dropdown resDropdown;
	private int currentResIndex;

	[SerializeField]
	private Toggle fullscreenCheckmark;
	private bool fullScreen;
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
		ContinueButton.GetComponent<Button>().interactable = show;
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

	public void OnFullscreenCheckmarkChanged()
	{
		bool val = fullscreenCheckmark.isOn;

		int x;
		if (val)
			x = 1;
		else
			x = 0;

		fullScreen = val;

		PlayerPrefs.SetInt("fullscreen", x);

		Screen.SetResolution(
			GameAssets.instance.resolutions[currentResIndex].width,
			GameAssets.instance.resolutions[currentResIndex].height,
			fullScreen);
	}

	public void OnResolutionsChanged()
	{
		int val = resDropdown.value;

		currentResIndex = val;
		PlayerPrefs.SetInt("resolution", currentResIndex);

		Screen.SetResolution(
			GameAssets.instance.resolutions[currentResIndex].width,
			GameAssets.instance.resolutions[currentResIndex].height,
			fullScreen);
	}

	private void UpdateResolutions()
	{
		currentResIndex = PlayerPrefs.GetInt("resolution");
		if (currentResIndex == 0)
			currentResIndex = GameAssets.instance.resolutions.Length - 2;

		resDropdown.ClearOptions();
		resDropdown.AddOptions(GameAssets.instance.ResolutionsToString());
		resDropdown.value = currentResIndex;
		resDropdown.RefreshShownValue();

		if (PlayerPrefs.GetInt("fullscreen") == 0)
			fullScreen = false;
		else
			fullScreen = true;

		fullscreenCheckmark.isOn = fullScreen;

		Screen.SetResolution(
			GameAssets.instance.resolutions[currentResIndex].width,
			GameAssets.instance.resolutions[currentResIndex].height,
			fullScreen);
	}
}
