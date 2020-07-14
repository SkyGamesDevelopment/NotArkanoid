using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
	#region variables
	public static PlayerBrain instance;

	public SpriteRenderer rend;
	private float edgeRadius = 0.1f;
	#endregion

	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);

		instance = this;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Ball") && BallManager.instance.kicked == true)
		{
			Vector3 ballPos = col.transform.position;

			//To make sure we don't hit ball twice or more
			if (col.gameObject.GetComponent<Rigidbody2D>().velocity.y > 0f)
				return;

			BallManager.instance.LaunchBall(new Vector3((ballPos.x - rend.bounds.center.x) * 2, 1f, 0f));
		}
	}

	private void Update()
	{
		CheckInput();
	}

	private void CheckInput()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !BallManager.instance.kicked && GameManager.instance.canMove)
			KickBall();

		if (Input.GetKeyDown(KeyCode.B))
		{
			PowerUpGain(1);
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			PowerUpGain(2);
		}
	}

	private void KickBall()
	{
		//Kick to the right
		if (transform.position.x <= 0f)
			BallManager.instance.LaunchBall(new Vector3(1.1f, 1f, 0f));
		//Kick to the left
		else
			BallManager.instance.LaunchBall(new Vector3(-1.1f, 1f, 0f));
	}

	public void PowerUpGain(int id)
	{
		SoundManager.PlaySound(SoundManager.Sound.PowerUpCollected);

		switch (id)
		{
			case 1:
				Instantiate(GameAssets.instance.rocketPrefab, transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity);
				break;
			case 2:
				Instantiate(GameAssets.instance.grenadesPrefab, transform.position + new Vector3(-0.2f, 0.5f, 0f), Quaternion.identity)
					.GetComponent<Rigidbody2D>().velocity = new Vector3(-3f, 3f, 0f);

				Instantiate(GameAssets.instance.grenadesPrefab, transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity)
					.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 6f, 0f);

				Instantiate(GameAssets.instance.grenadesPrefab, transform.position + new Vector3(0.2f, 0.5f, 0f), Quaternion.identity)
					.GetComponent<Rigidbody2D>().velocity = new Vector3(3f, 3f, 0f);
				break;
		}
	}
}
