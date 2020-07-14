using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
	#region variables
	[SerializeField]
	private short startHp;
	private short hp;

	[SerializeField]
	private bool undestructable;
	#endregion

	public void Start() => hp = startHp;

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (!col.collider.CompareTag("Ball") || undestructable)
			return;

		if (--hp > 0)
			return;
		else
			DestroyMe();
	}

	public void DestroyMe()
	{
		if (undestructable)
			return;

		MapGenerator.instance.DestroyBlock(transform.position);

		Destroy(gameObject);
	}
}
