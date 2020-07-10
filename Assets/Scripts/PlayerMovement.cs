using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	#region variables
	private float moveSpeed = 18f;
	private short input;

	private float maxDistanceToMove = 4f;
	private float fixedMaxDistanceToMove;
	#endregion

	private void Start()
	{
		//Fixing max distance to move (borders) because maybe in future player will be bigger or smaller
		fixedMaxDistanceToMove = maxDistanceToMove - PlayerBrain.instance.rend.bounds.extents.x;
	}

	private void Update()
	{
		CheckInputs();
		MovePlayer();
	}

	private void MovePlayer()
	{
		Vector3 pos = transform.position;

		if (input == -1 && pos.x <= -fixedMaxDistanceToMove)
			return;
		else if (input == 1 && pos.x >= fixedMaxDistanceToMove)
			return;

		pos += new Vector3(input * (moveSpeed / 100f) * Time.fixedDeltaTime, 0f, 0f);
		transform.position = pos;

		if (pos.x < -fixedMaxDistanceToMove)
			transform.position = new Vector3(-fixedMaxDistanceToMove, pos.y, pos.z);
		else if (pos.x > fixedMaxDistanceToMove)
			transform.position = new Vector3(fixedMaxDistanceToMove, pos.y, pos.z);
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
