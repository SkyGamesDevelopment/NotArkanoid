using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	#region variables
	public static MapGenerator instance;

	[HideInInspector]
	public byte[,] map;
	private int round;
	private int seed;

	private const float blockSizeX = 0.6666f;
	private const float blockSizeY = 0.3750f;
	private Vector2 startPos = new Vector2(-4f + 0.3333f, 4.75f - 0.1875f); //Minus/Plus block bounds

	private GameObject blockParent;

	//Custom editor
	[HideInInspector]
	public string roundCE, seedCE;
	#endregion

	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);

		instance = this;
	}

	private void Start() => blockParent = GameObject.Find("Blocks");

	public void MapGeneratorContructor(string seed, int round)
	{
		//Getting ours seed here
		this.seed = Mathf.Abs(seed.GetHashCode() * round);
		this.round = round;

		ClearMap();
		Generate();
	}

	public void MapGeneratorContructor(string seed, int round, byte[,] savedMap)
	{
		this.seed = Mathf.Abs(seed.GetHashCode() * round);
		this.round = round;
		this.map = savedMap;

		ClearMap();
		PlaceBlocks(savedMap);
	}

	private void Generate()
	{
		//If you have serious health problems, please DONT look below! :O

		map = new byte[14, 12];

		//How much patterns of normal blocks are we going to generate
		int minPatterns = 3 + Mathf.FloorToInt(round / 3);
		if (minPatterns > 5) minPatterns = 5;

		int maxPatterns = 4 + Mathf.FloorToInt(round / 3);
		if (maxPatterns > 6) maxPatterns = 6;

		int patterns = (seed % maxPatterns == 0) ? maxPatterns : minPatterns;

		//To prevent same patterns making genearted map more empty and boring
		byte[] usedPatterns = new byte[patterns];
		int x = 0;
		int pickedBlock = 0;
		int pickedPattern = 0;
		float powerUpsToGenerate = 0f;

		//Loop for creating normal patterns
		for (int i = 0; i < patterns; i++)
		{
			//Pick block
			x = (seed * i) % 10;

			//10 % for 3, 20% for 2, 70% for 3
			if (x >= 9) pickedBlock = 3;
			else if (x >= 7) pickedBlock = 2;
			else pickedBlock = 1;

			//Balance
			if (pickedBlock < 3)
			{
				x = (seed * i) % 15;

				int modifier = Mathf.FloorToInt(round / 2);
				if (modifier > 6) modifier = 6;
				if (x - modifier <= 0) pickedBlock++;
			}

			//Harder map = more power ups :)
			powerUpsToGenerate += pickedBlock / 3f;

			//Picking pattern
			x = (seed * i) % normalPatternCount;

			//Making sure to do not pick this same pattern
			if (i == 0)
			{
				pickedPattern = x;
				usedPatterns[0] = (byte)pickedPattern;
			}
			else
			{
				bool isOkay = true;
				pickedPattern = x;

				do
				{
					//We dont want to increment picked pattern when its first loop
					if (!isOkay)
						pickedPattern++;

					if (pickedPattern >= normalPatternCount) pickedPattern = 0;

					if (usedPatterns.Contains((byte)pickedPattern))
					{
						isOkay = false;
					}
					else
					{
						usedPatterns[i] = (byte)pickedPattern;
						break;
					}
				} while (!isOkay);
			}
			//Picking right pattern
			switch (pickedPattern)
			{
				case 0:
					GeneratePattern(GetNormalPattern0(), (byte)pickedBlock);
					break;
				case 1:
					GeneratePattern(GetNormalPattern1(), (byte)pickedBlock);
					break;
				case 2:
					GeneratePattern(GetNormalPattern2(), (byte)pickedBlock);
					break;
				case 3:
					GeneratePattern(GetNormalPattern3(), (byte)pickedBlock);
					break;
				case 4:
					GeneratePattern(GetNormalPattern4(), (byte)pickedBlock);
					break;
				case 5:
					GeneratePattern(GetNormalPattern5(), (byte)pickedBlock);
					break;
				case 6:
					GeneratePattern(GetNormalPattern6(), (byte)pickedBlock);
					break;
				case 7:
					GeneratePattern(GetNormalPattern7(), (byte)pickedBlock);
					break;
				case 8:
					GeneratePattern(GetNormalPattern8(), (byte)pickedBlock);
					break;
				case 9:
					GeneratePattern(GetNormalPattern9(), (byte)pickedBlock);
					break;
			}
		}

		//Picking empty pattern
		int pattern = seed % emptyPatternCount;

		switch (pattern)
		{
			case 0:
				GeneratePattern(GetEmptyPattern0(), 4);
				break;
			case 1:
				GeneratePattern(GetEmptyPattern1(), 4);
				break;
			case 2:
				GeneratePattern(GetEmptyPattern2(), 4);
				break;
			case 3:
				GeneratePattern(GetEmptyPattern3(), 4);
				break;
			case 4:
				GeneratePattern(GetEmptyPattern4(), 4);
				break;
			case 5:
				GeneratePattern(GetEmptyPattern5(), 4);
				break;
		}

		//Picking undestructable pattern
		pattern = seed % undestructablePatternCount;

		switch (pattern)
		{
			case 0:
				GeneratePattern(GetUndestructablePattern0(), 5);
				break;
			case 1:
				GeneratePattern(GetUndestructablePattern1(), 5);
				break;
			case 2:
				GeneratePattern(GetUndestructablePattern2(), 5);
				break;
			case 3:
				GeneratePattern(GetUndestructablePattern3(), 5);
				break;
			case 4:
				GeneratePattern(GetUndestructablePattern4(), 5);
				break;
			case 5:
				GeneratePattern(GetUndestructablePattern5(), 5);
				break;
			case 6:
				GeneratePattern(GetUndestructablePattern6(), 5);
				break;
			case 7:
				GeneratePattern(GetUndestructablePattern7(), 5);
				break;
		}

		//Generate power ups
		powerUpsToGenerate = Mathf.FloorToInt(powerUpsToGenerate);

		//Balance
		if (powerUpsToGenerate > 3) powerUpsToGenerate = 3;

		int y = map.GetLength(0) - 3; //Level of power ups spawning (2 blocks from bottom)

		for (int i = seed % (3 + (round % 3)); i < map.GetLength(1); i++)
		{
			if ((seed + i) % 3 == 0 || ((seed * i) + round) % 5 == 0)
			{
				//We dont want blocks to be alone in air
				if (map[y - (seed + i) % 3 - 1, i] == 4 || map[y - (seed + i) % 3 - 2, i] == 4)
					map[y - (seed + i) % 3 - 3, i] = 6;
				else
					map[y - (seed + i) % 3, i] = 6;

				if (--powerUpsToGenerate < 1) break;
			}
		}

		PlaceBlocks(map);
	}

	private void PlaceBlocks(byte[,] grid)
	{
		int points = 0;

		for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				Vector3 pos = GetPos(i, j);

				switch (grid[i, j])
				{
					case 1:
						Instantiate(GameAssets.instance.block1, pos, Quaternion.identity, blockParent.transform);
						points++;
						break;
					case 2:
						Instantiate(GameAssets.instance.block2, pos, Quaternion.identity, blockParent.transform);
						points++;
						break;
					case 3:
						Instantiate(GameAssets.instance.block3, pos, Quaternion.identity, blockParent.transform);
						points++;
						break;
					case 5:
						Instantiate(GameAssets.instance.undestructable, pos, Quaternion.identity, blockParent.transform);
						break;
					case 6:
						Instantiate(
							GameAssets.instance.powerUp,
							pos,
							Quaternion.identity,
							blockParent.transform).GetComponent<PowerUp>().PowerUpContructor((j % 2) + 1);
						points++;
						break;
				}
			}
		}
		GameManager.instance.maxPoints = points;
	}

	private Vector3 GetPos(int y, int x)
	{
		return new Vector3(startPos.x + x * blockSizeX, startPos.y - y * blockSizeY, 0f);
	}

	private void GeneratePattern(byte[,] pattern, byte block)
	{
		// 1 = Block1, 2 = Block2, 3 = Block3, 4 = Empty, 5 = Undestructable, 6 = power up
		for (int i = 0; i < map.GetLength(0); i++)
		{
			for (int j = 0; j < map.GetLength(1); j++)
			{
				if (map[i, j] < block && pattern[i, j] == 1 || pattern[i, j] == 6)
					map[i, j] = block;
			}
		}
	}

	public void ClearMap()
	{
		if (!blockParent)
			blockParent = GameObject.Find("Blocks");

		foreach (Transform t in blockParent.transform)
		{
			Destroy(t.gameObject);
		}
	}

	public void DestroyBlock(Vector3 pos)
	{
		float x = Mathf.Abs((pos.x - startPos.x) / blockSizeX);
		float y = Mathf.Abs((pos.y - startPos.y) / blockSizeY);

		map[(int)y, (int)x] = 0;

		GameManager.instance.AddPoint();
	}

	//Patterns must be 12x 14y [14, 12]
	//0 means dont place block, 1 means place block
	//I must say that it looks like old games map data arrays :P
	//To prevent containig data in cache by variables. I used return type
	//of variables to create data only when needed

	#region normal patterns
	private const int normalPatternCount = 10;

	private byte[,] GetNormalPattern0()
	{
		return new byte[14, 12]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0},
			{ 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0},
			{ 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0},
			{ 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0},
			{ 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0},
			{ 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0},
			{ 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1}
		};
	}

	private byte[,] GetNormalPattern1()
	{
		return new byte[14, 12]
		{
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
		};
	}

	private byte[,] GetNormalPattern2()
	{
		return new byte[14, 12]
		{
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
	}

	private byte[,] GetNormalPattern3()
	{
		return new byte[14, 12]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1},
			{ 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1},
			{ 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1},
			{ 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1}
		};
	}

	private byte[,] GetNormalPattern4()
	{
		return new byte[14, 12]
		{
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1},
			{ 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1},
			{ 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1},
			{ 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1},
			{ 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1},
			{ 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1}
		};
	}

	private byte[,] GetNormalPattern5()
	{
		return new byte[14, 12]
		{
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
			{ 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
	}

	private byte[,] GetNormalPattern6()
	{
		return new byte[14, 12]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0},
			{ 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
	}

	private byte[,] GetNormalPattern7()
	{
		return new byte[14, 12]
		{
			{ 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 1}
		};
	}

	private byte[,] GetNormalPattern8()
	{
		return new byte[14, 12]
		{
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0}
		};
	}

	private byte[,] GetNormalPattern9()
	{
		return new byte[14, 12]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
	}
	#endregion

	#region air patterns
	private const int emptyPatternCount = 6;

	private byte[,] GetEmptyPattern0()
	{
		return new byte[14, 12]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{ 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0},
			{ 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0},
			{ 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0}
		};
	}

	private byte[,] GetEmptyPattern1()
	{
		return new byte[14, 12]
		{
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1}
		};
	}

	private byte[,] GetEmptyPattern2()
	{
		return new byte[14, 12]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{ 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
	}

	private byte[,] GetEmptyPattern3()
	{
		return new byte[14, 12]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
	}

	private byte[,] GetEmptyPattern4()
	{
		return new byte[14, 12]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{ 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0},
			{ 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0},
			{ 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
	}

	private byte[,] GetEmptyPattern5()
	{
		return new byte[14, 12]
		{
			{ 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0}
		};
	}
	#endregion

	#region undestructable patterns
	private const int undestructablePatternCount = 8;

	private byte[,] GetUndestructablePattern0()
	{
		return new byte[14, 12]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
	}

	private byte[,] GetUndestructablePattern1()
	{
		return new byte[14, 12]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 1, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
	}

	private byte[,] GetUndestructablePattern2()
	{
		return new byte[14, 12]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{ 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0},
			{ 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
	}

	private byte[,] GetUndestructablePattern3()
	{
		return new byte[14, 12]
		{
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1}
		};
	}

	private byte[,] GetUndestructablePattern4()
	{
		return new byte[14, 12]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0},
			{ 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
	}

	private byte[,] GetUndestructablePattern5()
	{
		return new byte[14, 12]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 1, 0, 1, 1, 1, 1, 0, 1, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
	}

	private byte[,] GetUndestructablePattern6()
	{
		return new byte[14, 12]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0},
			{ 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0},
			{ 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0},
			{ 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0},
			{ 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
	}

	private byte[,] GetUndestructablePattern7()
	{
		return new byte[14, 12]
		{
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0}
		};
	}
	#endregion
}