using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
	#region variables
	public int id;
	private float fallSpeed = 5f;
	#endregion

	private void Start()
	{
		StartCoroutine(DeathTimer());
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			PlayerBrain.instance.PowerUpGain(id);
			Destroy(gameObject);
		}
	}

	private void FixedUpdate() => transform.position += new Vector3(0f, -fallSpeed / 100f, 0f);

	private IEnumerator DeathTimer()
	{
		yield return new WaitForSeconds(10f);

		Destroy(gameObject);
	}
}
