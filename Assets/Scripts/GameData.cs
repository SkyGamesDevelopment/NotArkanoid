using System.Collections.Generic;

[System.Serializable]
public class GameData
{
	public int round;
	public int lifes;
	public int points;
	public byte[,] map;

	public GameData(int round, int lifes, int points, byte[,] map)
	{
		this.round = round;
		this.lifes = lifes;
		this.points = points;
		this.map = map;
	}
}