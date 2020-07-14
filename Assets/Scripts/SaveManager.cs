using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
	public static void SaveGame()
	{
		GameData data = new GameData(
			GameManager.instance.round,
			GameManager.instance.playerLifes,
			GameManager.instance.points,
			MapGenerator.instance.map,
			GameManager.instance.highScore);

		BinaryFormatter bf = new BinaryFormatter();
		string path = Application.persistentDataPath + "/save.data";
		FileStream fs = new FileStream(path, FileMode.Create);

		bf.Serialize(fs, data);
		fs.Close();
	}

	public static void LoadGame()
	{
		string path = Application.persistentDataPath + "/save.data";

		if (CheckData(path))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = new FileStream(path, FileMode.Open);

			GameData data = bf.Deserialize(fs) as GameData;
			fs.Close();

			GameManager.instance.PrepareGame(data.round, data.map, data.lifes, data.points, data.highScore);
		}
		else
		{
			GameManager.instance.StartNewGame();
		}
	}

	public static void SaveVolume()
	{
		VolumeData data = new VolumeData(SoundManager.soundVolume, SoundManager.musicVolume);

		BinaryFormatter bf = new BinaryFormatter();
		string path = Application.persistentDataPath + "/save.volume";
		FileStream fs = new FileStream(path, FileMode.Create);

		bf.Serialize(fs, data);
		fs.Close();
	}

	public static VolumeData GetVolume()
	{
		string path = Application.persistentDataPath + "/save.volume";

		if (CheckData(path))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = new FileStream(path, FileMode.Open);

			VolumeData data = bf.Deserialize(fs) as VolumeData;
			fs.Close();

			return data;
		}
		else
		{
			return new VolumeData(0.5f, 0.5f);
		}
	}

	public static int GetHighscore()
	{
		string path = Application.persistentDataPath + "/save.data";

		if (CheckData(path))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = new FileStream(path, FileMode.Open);

			GameData data = bf.Deserialize(fs) as GameData;
			fs.Close();

			GameManager.instance.highScore = data.highScore;

			return data.highScore;
		}
		else
		{
			return -1;
		}
	}

	public static bool CheckData(string path)
	{
		if (File.Exists(path))
			return true;
		else
			return false;
	}
}
