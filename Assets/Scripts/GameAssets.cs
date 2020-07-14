using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
	#region init
	public static GameAssets instance;

	private void Awake()
	{
		if (instance != null)
			Destroy(this.gameObject);
		else
			instance = this;

		DontDestroyOnLoad(this);
	}
	#endregion

	#region sound
	public Sound[] sounds;
	[Space] [Header("In game music")]
	public AudioClip music;

	[System.Serializable]
	public class Sound
	{
		public SoundManager.Sound sound;
		public AudioClip clip;
	}

	#endregion

	#region prefabs
	public GameObject playerPrefab;
	public GameObject ballPrefab;
	public GameObject generatorPrefab;
	public GameObject explosionPrefab;
	//Blocks
	public GameObject block1, block2, block3, undestructable, powerUp;
	//Power ups
	public GameObject rocketPrefab, grenadesPrefab;
	public GameObject rocketIcon, grenadeIcon;
	#endregion
}
