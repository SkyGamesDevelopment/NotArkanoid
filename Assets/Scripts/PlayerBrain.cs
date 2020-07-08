using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
	#region variables
	[SerializeField]
	private SpriteRenderer rend;
	private float edgeRadius = 0.1f;
	#endregion

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.collider.CompareTag("Ball"))
		{
			Vector3 ballPos = col.transform.position;

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
		//DEBUG
		if (Input.GetKeyDown(KeyCode.Space))
			BallManager.instance.LaunchBall(new Vector3(0f, 2f, 0f));
	}
}
