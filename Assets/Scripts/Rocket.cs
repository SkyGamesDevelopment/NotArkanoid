using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
	#region variables
	private float speed = 13f;
	private float radius = 1.75f;
	#endregion

	private void FixedUpdate()
	{
		transform.position += new Vector3(0f, speed / 100f, 0f);
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("Collider") || col.gameObject.CompareTag("Border"))
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
			CameraShake.instance.Shake(0.2f, 4f);

			Destroy(gameObject);
		}
	}
}
