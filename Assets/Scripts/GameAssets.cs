using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

		InitResolutions();
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
	public GameObject rocketPickUp, grenadePickUp;
	#endregion

	#region resolution
	public Resolution[] resolutions;

	public void InitResolutions() => resolutions = Screen.resolutions;

	public List<string> ResolutionsToString()
	{
		List<string> list = new List<string>();

		foreach (Resolution r in resolutions)
		{
			list.Add(r.width + " x " + r.height);
		}

		return list.Distinct().ToList();
	}
	#endregion
}
