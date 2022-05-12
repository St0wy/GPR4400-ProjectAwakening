using System;
using JetBrains.Annotations;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAwakening.Player
{
	public enum MovementState
	{
		Idle = 0,
		Moving = 1,
	}

	public enum ActionState
	{
		None = 0,
		Melee = 1,
		Aim = 2,
		Shield = 3,
		Carry = 4,
	}

	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private float speed = 5.0f;

		private MovementState movementState = MovementState.Idle;
		private ActionState actionState = ActionState.None;
		private Vector2 direction;

		private Rigidbody2D rb;

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		[UsedImplicitly]
		private void OnMove(InputValue value)
		{
			//Get the directional value
			direction = value.Get<Vector2>();

			//Normalize the vector if it's above 1 in length
			if (direction.sqrMagnitude > direction.normalized.sqrMagnitude)
				direction.Normalize();
		}

		private void FixedUpdate()
		{
			ApplyMovement();
		}

		private void ApplyMovement()
		{
			movementState = direction.Approximately(Vector2.zero) ? MovementState.Idle : MovementState.Moving;

			//Check if we can move
			switch (movementState)
			{
				case MovementState.Idle:
				case MovementState.Moving:
					rb.velocity = direction * speed;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}