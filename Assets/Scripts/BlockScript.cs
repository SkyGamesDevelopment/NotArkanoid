using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
	#region variables
	#endregion

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.collider.CompareTag("Ball"))
			Destroy(this.gameObject);
	}
}
