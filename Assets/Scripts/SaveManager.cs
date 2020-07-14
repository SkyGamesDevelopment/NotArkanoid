using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public static class SaveManager
{
	#region game data
	public static void SaveGame()
	{
		GameData data = new GameData(
			GameManager.instance.round,
			GameManager.instance.playerLifes,
			GameManager.instance.points,
			MapGenerator.instance.map);

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

			GameManager.instance.PrepareGame(data.round, data.map, data.lifes, data.points);
		}
		else
		{
			GameManager.instance.StartNewGame();
		}
	}

	public static void WipeData()
	{
		if (File.Exists(Application.persistentDataPath + "/save.data"))
			File.Delete(Application.persistentDataPath + "/save.data");
	}

	public static bool CanContinue()
	{
		return CheckData(Application.persistentDataPath + "/save.data");
	}
	#endregion

	public static bool CheckData(string path)
	{
		if (File.Exists(path))
			return true;
		else
			return false;
	}
}
