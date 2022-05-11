using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace ProjectAwakening.Player
{
	public class PlayerMovement : MonoBehaviour
	{
		public enum MovementState
		{
			Idle = 0,
			Moving = 1,
		}

		public enum ActionState
		{
			Inactive = 0
		}

		[FormerlySerializedAs("_speed")] [SerializeField]
		private float speed = 5.0f;

		private MovementState movementState = MovementState.Moving;
		private ActionState actionState;
		private Vector2 direction;
		
		private PlayerInput playerInput;
		private Rigidbody2D rb;

		private void Awake()
		{
			playerInput = GetComponent<PlayerInput>();
			rb = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			//Get the directional value
			direction = playerInput.actions.FindAction("Move").ReadValue<Vector2>();

			//Normalize the vector if it's above 1 in length
			if (direction.sqrMagnitude > direction.normalized.sqrMagnitude)
				direction.Normalize();
		}

		private void FixedUpdate()
		{
			SetMovement();
		}

		private void SetMovement()
		{
			movementState = movementState switch
			{
				MovementState.Moving when direction == Vector2.zero => MovementState.Idle,
				MovementState.Idle when direction != Vector2.zero => MovementState.Moving,
				_ => movementState,
			};

			//Check if we can move
			switch (movementState)
			{
				case MovementState.Idle:
				case MovementState.Moving:
					//Set our velocity
					rb.velocity = direction * speed;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}