using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region variables
	[HideInInspector]
	private int round;
	private const string seed = "6291594"; //7 numbers from 0 to 9

	[SerializeField]
	private GameObject playerPrefab;
	[SerializeField]
	private GameObject ballPrefab;

	Vector3 playerStartPos = new Vector3(0f, -4.6f, 0f);
	Vector3 ballStartPos = new Vector3(0f, -4.15f, 0f);
	#endregion

	public void StartRound(int round)
	{
		this.round = round;

		MapGenerator scr = gameObject.AddComponent<MapGenerator>();
		scr.MapGeneratorContructor(round, seed);

		Instantiate(playerPrefab, playerStartPos, Quaternion.identity);
		Instantiate(ballPrefab, ballStartPos, Quaternion.identity);
	}
}
