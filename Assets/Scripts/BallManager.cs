using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
	#region variables
	public static BallManager instance;

	private Rigidbody2D rb;
	[HideInInspector]
	public Vector3 startPos = new Vector3(0f, -4.15f, 0f);
	private float baseSpeed = 5f;
	private float currentSpeed;
	//private float speedIncrement = 1f;
	#endregion

	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);

		instance = this;

		rb = GetComponent<Rigidbody2D>();
	}

	private void Start() => currentSpeed = baseSpeed;

	public void ResetBall()
	{
		rb.velocity = Vector3.zero;
		currentSpeed = baseSpeed;
		transform.position = startPos;
	}

	public void LaunchBall(Vector3 direction) => rb.velocity = direction * currentSpeed;
}
