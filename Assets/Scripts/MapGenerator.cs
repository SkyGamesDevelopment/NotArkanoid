using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	#region variables
	private byte[,] map = new byte[16, 10];
	private int round;
	private string seed;

	//Blocks
	[SerializeField]
	private GameObject block1, block2, block3;
	#endregion

	public void MapGeneratorContructor(int round, string seed)
	{
		this.round = round;
		this.seed = seed;
	}

	private void Start() => Generate();

	private void Generate()
	{
		//Modyfying seed by level number
		int[] seedArray = MakeSeedArray();

		//How much patterns of normal blocks are we going to generate
		//Modified by level number to make map more challenging (more blocks to destroy)
		int x = seedArray[0];

		int minPatterns = 3 + Mathf.FloorToInt(round / 3);
		if (minPatterns > 5) minPatterns = 5;

		int maxPatterns = 5 + Mathf.FloorToInt(round / 3);
		if (maxPatterns > 7) maxPatterns = 7;

		int patterns;

		if (x <= minPatterns) patterns = minPatterns;
		else if (x >= maxPatterns) patterns = maxPatterns;
		else patterns = x;

		byte[] usedPatterns = new byte[patterns];

		//Loop for creating patterns
		for (int i = 0; i < patterns; i++)
		{
			//Pick block (1 from 3)
			// 50% for block1, 40% for block2, 10% for block3
			x = seedArray[1];
			int pickedBlock;

			if (x <= 4) pickedBlock = 1;
			else if (x <= 8) pickedBlock = 2;
			else pickedBlock = 3;

			//Balance
			if (pickedBlock < 3)
			{
				x = seedArray[2];
				int modifier = Mathf.FloorToInt(round / 2);
				if (modifier > 6) modifier = 6;
				if (x - modifier <= 0) pickedBlock++;
			}

			//Picking pattern
			x = seedArray[3];
			int pickedPattern;
			//Making sure to do not pick this same pattern
			if (i == 0)
			{
				pickedPattern = (x + (i * (x + 2))) % normalPatternCount;
				usedPatterns[0] = (byte)pickedPattern;
			}
			else
			{
				bool isOkay = true;

				do
				{
					int fix = 0;
					pickedPattern = ((x + (i * (x + 2))) + fix) % normalPatternCount;

					if (usedPatterns.Contains((byte)pickedPattern))
					{
						fix++;
						isOkay = false;
					}
					else
						break;

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

		//TODO
		//Generate unbreakable
		//Generate empty
		//Generate boosts
	}

	private void GeneratePattern(byte[,] pattern, byte block)
	{
		//TODO
	}

	private int[] MakeSeedArray()
	{
		long x = long.Parse(seed);
		x *= round;
		string fSeed = x.ToString();

		int[] seedArray = new int[seed.Length];

		for (int i = 0; i < seedArray.Length; i++)
		{
			seedArray[i] = (int)fSeed[i];
		}

		return seedArray;
	}

	//Patterns must be 16x 10y
	//0 means dont place block, 1 means place block
	//I must say that it looks like old games map data arrays :P
	//To prevent containig data in cache by variables. I used return type
	//of variables to create data in cache only when needed
	#region normal patterns
	private const int normalPatternCount = 10;

	private byte[,] GetNormalPattern0()
	{
		return new byte[10, 16]
		{
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1}
		};
	}

	private byte[,] GetNormalPattern1()
	{
		return new byte[10, 16]
		{
			{ 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1}
		};
	}

	private byte[,] GetNormalPattern2()
	{
		return new byte[10, 16]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
	}

	private byte[,] GetNormalPattern3()
	{
		return new byte[10, 16]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
			{ 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
	}

	private byte[,] GetNormalPattern4()
	{
		return new byte[10, 16]
		{
			{ 0, 1, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0},
			{ 0, 1, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0},
			{ 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0}
		};
	}

	private byte[,] GetNormalPattern5()
	{
		return new byte[10, 16]
		{
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0},
			{ 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0},
			{ 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
	}

	private byte[,] GetNormalPattern6()
	{
		return new byte[10, 16]
		{
			{ 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0},
			{ 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0},
			{ 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0},
			{ 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0}
		};
	}

	private byte[,] GetNormalPattern7()
	{
		return new byte[10, 16]
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0},
			{ 0, 0, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 1, 0, 0},
			{ 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0},
			{ 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1}
		};
	}

	private byte[,] GetNormalPattern8()
	{
		return new byte[10, 16]
		{
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0},
			{ 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0},
			{ 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
			{ 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
			{ 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
			{ 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0}
		};
	}

	private byte[,] GetNormalPattern9()
	{
		return new byte[10, 16]
		{
			{ 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
			{ 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
			{ 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1}
		};
	}
	#endregion

	#region undestructable patterns
	private const int unbreakablePatternCount = 8;
	#endregion

	#region empty patterns
	private const int emptyPatternCount = 6;
	#endregion
}
