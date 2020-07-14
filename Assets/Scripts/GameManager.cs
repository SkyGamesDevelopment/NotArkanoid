using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	#region variables
	public static GameManager instance;

	[HideInInspector]
	public int round;
	private const string seed = "seed123";

	[HideInInspector]
	public int maxPoints, pointsInRound, points, highScore;

	[HideInInspector]
	public int playerLifes;

	private GameObject player, ball, generator;

	Vector3 playerStartPos = new Vector3(0f, -4.6f, 0f);

	public bool canMove = false;
	public bool alive = false;

	private bool load = false;

	private float brickDestroyedTimer;
	#endregion

	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);

		instance = this;

		DontDestroyOnLoad(this);
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && alive)
		{
			TogglePause();
		}
	}

	public void StartNewGame()
	{
		load = false;
		SceneManager.LoadScene(1); //Game scene
	}

	public void ContinueGame()
	{
		load = true;
		SceneManager.LoadScene(1); //Game scene
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "GameScene")
		{
			InGameUIManager.instance.ChangeCanvas(InGameUIManager.canvas.playing);
			if (load)
				SaveManager.LoadGame();
			else
				PrepareGame(1);
		}
	}

	private void PrepareGame(int round)
	{
		//If starting game, make sure player have all 3 lifes
		if (round == 1)
		{
			playerLifes = 3;
			points = 0;
			highScore = PlayerPrefs.GetInt("highscore");
		}

		pointsInRound = 0;
		this.round = round;

		InGameUIManager.instance.UpdateHighScore(highScore);
		InGameUIManager.instance.UpdateScore(points);
		InGameUIManager.instance.UpdateLifes(playerLifes);

		if (generator == null)
			generator = Instantiate(GameAssets.instance.generatorPrefab);

		MapGenerator.instance.MapGeneratorContructor(seed, round);

		StartRound();
	}

	public void PrepareGame(int round, byte[,] map, int lifes, int points)
	{
		this.round = round;
		this.playerLifes = lifes;
		this.points = points;
		pointsInRound = 0;
		highScore = PlayerPrefs.GetInt("highscore");

		InGameUIManager.instance.UpdateHighScore(highScore);
		InGameUIManager.instance.UpdateScore(points);
		InGameUIManager.instance.UpdateLifes(playerLifes);

		if (generator == null)
			generator = Instantiate(GameAssets.instance.generatorPrefab);

		MapGenerator.instance.MapGeneratorContructor(seed, round, map);

		StartRound();
	}

	private void StartRound()
	{
		//We dont want player to continue from the same moment multiple times when he dies
		//The point of save game is to continue not "checkpointning" in game
		SaveManager.WipeData();

		SoundManager.PlaySound(SoundManager.Sound.NextRound);

		alive = true;
		canMove = true;
		Time.timeScale = 1f;

		if (player == null)
			player = Instantiate(GameAssets.instance.playerPrefab, playerStartPos, Quaternion.identity);
		else
			player.transform.position = playerStartPos;

		if (ball == null)
			ball = Instantiate(GameAssets.instance.ballPrefab, player.transform.position, Quaternion.identity);
		else
			ball.GetComponent<BallManager>().ResetBall();
	}

	public void AddPoint()
	{
		pointsInRound++;
		points++;

		InGameUIManager.instance.UpdateScore(points);

		if (points > highScore)
		{
			highScore = points;
			InGameUIManager.instance.UpdateHighScore(highScore);
		}

		if (pointsInRound >= maxPoints)
		{
			PrepareGame(round + 1);
		}
		else
		{
			if (brickDestroyedTimer + 0.05f < Time.time)
			{
				brickDestroyedTimer = Time.time;
				SoundManager.PlaySound(SoundManager.Sound.BlockDestroyed);
			}
		}
	}

	public void GetDamage()
	{
		playerLifes--;
		InGameUIManager.instance.UpdateLifes(playerLifes);

		CameraShake.instance.Shake(0.15f, 4f);

		if (playerLifes < 1)
		{
			EndGame();
		}
		else
		{
			SoundManager.PlaySound(SoundManager.Sound.GetDamage);
			BallManager.instance.ResetBall();
		}
	}

	private void EndGame()
	{
		PlayerPrefs.SetInt("highscore", highScore);
		SoundManager.PlaySound(SoundManager.Sound.GameOver);
		Time.timeScale = 0f;
		alive = false;
		canMove = false;
		InGameUIManager.instance.ChangeCanvas(InGameUIManager.canvas.gameOver);
	}

	private void TogglePause()
	{
		if (canMove)
		{
			Time.timeScale = 0f;
			canMove = false;
			InGameUIManager.instance.ChangeCanvas(InGameUIManager.canvas.pause);
		}
		else
		{
			Time.timeScale = 1f;
			canMove = true;
			InGameUIManager.instance.ChangeCanvas(InGameUIManager.canvas.playing);
		}
	}

	public void DontDestroyOnLoadSth(GameObject go) => DontDestroyOnLoad(go);
}
