using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	#region variables
	private float moveSpeed = 10f;
	private short input;
	#endregion

	private void Update()
	{
		CheckInputs();
	}

	private void FixedUpdate()
	{
		transform.position += new Vector3(input * (moveSpeed / 100f), 0f, 0f);
	}

	private void CheckInputs()
	{
		if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
			input = 0;
		else if (Input.GetKey(KeyCode.A))
			input = -1;
		else if (Input.GetKey(KeyCode.D))
			input = 1;
		else
			input = 0;
	}
}
