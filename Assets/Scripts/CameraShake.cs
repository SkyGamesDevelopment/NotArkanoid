using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public static CameraShake instance;

	private void Awake()
	{
		if (instance != null)
			Destroy(instance);

		instance = this;
	}

	public void Shake(float time, float amplitude)
	{
		StartCoroutine(DoShake(time, amplitude));
	}

	private IEnumerator DoShake(float time, float amplitude)
	{
		GetComponent<CinemachineVirtualCamera>()
			.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>()
			.m_AmplitudeGain = amplitude;

		yield return new WaitForSeconds(time);

		GetComponent<CinemachineVirtualCamera>()
			.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>()
			.m_AmplitudeGain = 0f;
	}
}
