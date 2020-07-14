using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
	private static GameObject soundObject, musicObject;
	private static AudioSource srcSound, srcMusic;

	public static float soundVolume, musicVolume;

	public enum Sound
	{
		Explosion,
		BallBounce,
		BlockDestroyed,
		GetDamage,
		NextRound,
		ButtonClick,
		PowerUpCollected,
		WelcomeVoice,
		GameOver
	}

	public static void UpdateVolume(float soundVol, float musicVol)
	{
		soundVolume = soundVol;
		if (soundObject && srcSound)
			srcSound.volume = soundVol;

		musicVolume = musicVol;
		if (musicObject && srcMusic)
			srcMusic.volume = musicVol;
	}

	public static void PlaySound(Sound s)
	{
		if (!soundObject)
		{
			soundObject = new GameObject("SoundObject");
			GameManager.instance.DontDestroyOnLoadSth(soundObject);
		}
		if (!srcSound)
			srcSound = soundObject.AddComponent<AudioSource>();

		srcSound.volume = soundVolume;
		srcSound.PlayOneShot(GetSound(s));
	}

	public static void PlayMusic()
	{
		if (!musicObject)
		{
			musicObject = new GameObject("MusicObject");
			//We want to continue playing music while loading to other scene
			GameManager.instance.DontDestroyOnLoadSth(musicObject);
		}
		if (!srcMusic)
			srcMusic = musicObject.AddComponent<AudioSource>();

		srcMusic.loop = true;
		srcMusic.volume = musicVolume;
		srcMusic.clip = GameAssets.instance.music;
		srcMusic.Play();
	}

	private static AudioClip GetSound(Sound s)
	{
		foreach (GameAssets.Sound Sound in GameAssets.instance.sounds)
		{
			if (Sound.sound == s)
				return Sound.clip;
		}
		return null;
	}
}
