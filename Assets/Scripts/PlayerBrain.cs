using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
		if (col.CompareTag("Ball"))
		{
			Vector3 ballPos = col.transform.position;

			//To make sure we don't hit ball twice or more
			if (col.gameObject.GetComponent<Rigidbody2D>().velocity.y > 0f)
				return;

			if (ballPos.x >= rend.bounds.center.x)
			{
				if (ballPos.x >= rend.bounds.max.x - edgeRadius)
					BallManager.instance.LaunchBall(new Vector3(1.5f, 0.5f, 0f));
				else
					BallManager.instance.LaunchBall(new Vector3(1f, 1f, 0f));
			}
			else
			{
				if (ballPos.x <= rend.bounds.min.x + edgeRadius)
					BallManager.instance.LaunchBall(new Vector3(-1.5f, 0.5f, 0f));
				else
					BallManager.instance.LaunchBall(new Vector3(-1f, 1f, 0f));
			}
		}
	}

	private void Update()
	{
		CheckInput();
	}

	private void CheckInput()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			KickBall();
	}

	private void KickBall()
	{
		//Kick to the right
		if (transform.position.x <= BallManager.instance.startPos.x)
			BallManager.instance.LaunchBall(new Vector3(1f, 1f, 0f));
		//Kick to the left
		else
			BallManager.instance.LaunchBall(new Vector3(-1f, 1f, 0f));
	}
}
