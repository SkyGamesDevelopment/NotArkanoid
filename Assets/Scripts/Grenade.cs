using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
	#region variables
	private float radius = 0.65f;
	private float timeLimit = 3f;
	#endregion

	private void Start() => StartCoroutine(StartTimer(timeLimit));

	private IEnumerator StartTimer(float delay)
	{
		yield return new WaitForSeconds(delay);

		Explode();
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("Collider"))
			Explode();
	}

	private void Explode()
	{
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius);

		foreach (Collider2D c in cols)
		{
			if (c.gameObject.CompareTag("Collider"))
			{
				if (c.gameObject.GetComponent<BlockScript>())
				{
					c.gameObject.GetComponent<BlockScript>().DestroyMe();
				}
				else
				{
					c.gameObject.GetComponent<PowerUp>().DestroyMe();
				}
			}
		}
		SoundManager.PlaySound(SoundManager.Sound.Explosion);
		Instantiate(GameAssets.instance.explosionPrefab, transform.position, Quaternion.identity);
		CameraShake.instance.Shake(0.1f, 1.25f);

		Destroy(gameObject);
	}
}
