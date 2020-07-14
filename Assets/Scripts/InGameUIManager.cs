using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
	#region variables

	public static InGameUIManager instance;

	[SerializeField]
	private GameObject playingCanvas, pauseCanvas, gameOverCanvas;
	[SerializeField]
	private Image life1, life2, life3;

	private Color showColor = new Color(1f, 1f, 1f, 0.75f);
	private Color hideColor = new Color(1f, 1f, 1f, 0.2f);

	[SerializeField]
	private TextMeshProUGUI score, highScore;
	#endregion

	private void Awake()
	{
		if (instance != null)
			Destroy(instance);

		instance = this;
	}

	public enum canvas
	{ 
		playing,
		pause,
		gameOver
	}

	public void ChangeCanvas(canvas x)
	{
		switch(x)
		{
			case canvas.playing:
				playingCanvas.SetActive(true);
				pauseCanvas.SetActive(false);
				gameOverCanvas.SetActive(false);
				break;
			case canvas.pause:
				playingCanvas.SetActive(true);
				pauseCanvas.SetActive(true);
				gameOverCanvas.SetActive(false);
				break;
			case canvas.gameOver:
				playingCanvas.SetActive(true);
				pauseCanvas.SetActive(false);
				gameOverCanvas.SetActive(true);
				break;
		}
	}

	public void SaveAndExitButtonClicked()
	{
		SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
		SaveManager.SaveGame();
		SaveManager.SaveHighscore();
		SceneManager.LoadScene(0); //Main menu
	}

	public void ToMenuButtonClicked()
	{
		SaveManager.SaveHighscore();
		SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
		SceneManager.LoadScene(0); //Main menu
	}

	public void UpdateHighScore(int score)
	{
		highScore.text = score.ToString();
	}

	public void UpdateScore(int score)
	{
		this.score.text = score.ToString();
	}

	public void UpdateLifes(int lifes)
	{
		switch(lifes)
		{
			case 0:
				life1.color = hideColor;
				life2.color = hideColor;
				life3.color = hideColor;
				break;
			case 1:
				life1.color = showColor;
				life2.color = hideColor;
				life3.color = hideColor;
				break;
			case 2:
				life1.color = showColor;
				life2.color = showColor;
				life3.color = hideColor;
				break;
			case 3:
				life1.color = showColor;
				life2.color = showColor;
				life3.color = showColor;
				break;
		}
	}
}
