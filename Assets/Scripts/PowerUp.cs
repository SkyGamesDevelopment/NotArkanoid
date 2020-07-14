using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
	#region variables
	private int id;
	#endregion

	public void PowerUpContructor(int powerUpId) => id = powerUpId;

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.collider.CompareTag("Ball"))
			DestroyMe();
	}

	public void DestroyMe()
	{
		MapGenerator.instance.DestroyBlock(transform.position);

		switch (id)
		{
			case 1: //Rocket
				Instantiate(GameAssets.instance.rocketPickUp, transform.position, Quaternion.identity);
				break;
			case 2: //Grenades
				Instantiate(GameAssets.instance.grenadePickUp, transform.position, Quaternion.identity);
				break;
		}

		Destroy(gameObject);
	}
}
