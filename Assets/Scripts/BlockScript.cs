using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
	#region variables
	private short hp;
	private bool undestructable;
	#endregion

	public BlockScript(short hp) => this.hp = hp;

	public BlockScript(bool undestructable) => this.undestructable = undestructable;

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
