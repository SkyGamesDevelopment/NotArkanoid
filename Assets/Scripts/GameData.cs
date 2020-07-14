[System.Serializable]
public class GameData
{
	public int round;
	public int lifes;
	public int points;
	public int highScore;
	public byte[,] map;

	public GameData(int round, int lifes, int points, byte[,] map, int highScore)
	{
		this.round = round;
		this.lifes = lifes;
		this.points = points;
		this.map = map;
		this.highScore = highScore;
	}
}

[System.Serializable]
public class VolumeData
{
	public float soundVolume;
	public float musicVolume;

	public VolumeData(float soundVolume, float musicVolume)
	{
		this.soundVolume = soundVolume;
		this.musicVolume = musicVolume;
	}
}
