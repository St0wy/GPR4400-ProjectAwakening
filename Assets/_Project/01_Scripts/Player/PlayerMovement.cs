using System;
using JetBrains.Annotations;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAwakening.Player
{
	public class PlayerMovement : MonoBehaviour
	{
		private const float DirectionEpsilon = 0.001f;
		
		[SerializeField] private float speed = 5.0f;

		private Vector2 input;
		private Rigidbody2D rb;
		
		public ActionState ActionState { get; private set; } = ActionState.None;
		public MovementState MovementState { get; private set; } = MovementState.Idle;
		public Direction Direction { get; private set; }
		
		private Vector2 Input
		{
			get => input;
			set
			{
				input = value;
				
				//Normalize the vector if it's above 1 in length
				if (input.sqrMagnitude > input.normalized.sqrMagnitude)
					input.Normalize();
			}
		}

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		[UsedImplicitly]
		private void OnMove(InputValue value)
		{
			//Get the directional value
			Input = value.Get<Vector2>();
		}

		private void FixedUpdate()
		{
			ApplyMovement();
		}

		private void ApplyMovement()
		{
			MovementState = Input.Approximately(Vector2.zero) ? MovementState.Idle : MovementState.Moving;

			if (input.x > DirectionEpsilon)
			{
				Direction = Direction.Right;
			} else if (input.x < DirectionEpsilon)
			{
				Direction = Direction.Left;
			}

			if (input.y > DirectionEpsilon)
			{
				Direction = Direction.Up;
			} else if (input.y < DirectionEpsilon)
			{
				Direction = Direction.Down;
			}

			//Check if we can move
			switch (MovementState)
			{
				case MovementState.Idle:
				case MovementState.Moving:
					rb.velocity = Input * speed;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}