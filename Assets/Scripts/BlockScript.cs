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
			Destroy(this.gameObject);
	}

	private void OnDestroy()
	{
		//TODO call GameManager that block destroyed
	}
}
