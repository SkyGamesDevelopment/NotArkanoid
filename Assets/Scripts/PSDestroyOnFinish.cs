using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSDestroyOnFinish : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem ps;

	private void Update()
	{
		if (ps && !ps.IsAlive())
			Destroy(this.gameObject);
	}
}
