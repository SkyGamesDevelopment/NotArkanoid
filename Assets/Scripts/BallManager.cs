using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
	#region variables
	public static BallManager instance;

	private Rigidbody2D rb;

	private Vector3 fix = new Vector3(0f, 0.25f, 0f);
	private float speed = 4.5f;

	[HideInInspector]
	public bool kicked = false;
	#endregion

	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);

		instance = this;

		rb = GetComponent<Rigidbody2D>();
	}

	private void Start() => ResetBall();

	private void Update()
	{
		if (!kicked)
			transform.position = PlayerBrain.instance.gameObject.transform.position + fix;
	}

	public void ResetBall()
	{
		rb.velocity = Vector3.zero;
		transform.position = PlayerBrain.instance.transform.position + fix;
		kicked = false;
	}

	public void LaunchBall(Vector3 direction)
	{
		SoundManager.PlaySound(SoundManager.Sound.BallBounce);
		kicked = true;
		rb.velocity = direction * speed;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("DeadBorder"))
		{
			GameManager.instance.GetDamage();
		}
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		SoundManager.PlaySound(SoundManager.Sound.BallBounce);
	}
}
