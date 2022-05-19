using System;
using JetBrains.Annotations;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAwakening.Player
{
	[RequireComponent(typeof(PlayerActions))]
	public class PlayerMovement : MonoBehaviour
	{
		//Threshold value to consider the character as facing a direction
		private const float DirectionEpsilon = 0.001f;
		
		[Header("Parameters")]
		[SerializeField] 
		private float speed = 5.0f;

		[Tooltip("Fraction of our speed when we hold the shield")]
		[SerializeField]
		private float shieldMoveMult = 0.4f;

		[Tooltip("Fraction of our speed when we carry something")]
		[SerializeField]
		private float carryMoveMult = 0.2f;

		private PlayerActions playerActions;

		private Vector2 input;
		private Rigidbody2D rb;
	
		public MovementState MovementState { get; private set; } = MovementState.Idle;
		public Direction Direction { get; private set; }

		public static Vector2 DirectionToVector(Direction direction)
		{
			switch (direction)
			{
				case Direction.Up: return Vector2.up;
				case Direction.Down: return Vector2.down;
				case Direction.Left: return Vector2.left;
				case Direction.Right: return Vector2.right;
				default: return Vector2.zero;
			}	
		}
		
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
			playerActions = GetComponent<PlayerActions>();
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

			//Set the direction our character is facing
			if (input.x > DirectionEpsilon)
			{
				Direction = Direction.Right;
			} 
			else if (input.x < -DirectionEpsilon)
			{
				Direction = Direction.Left;
			}

			if (input.y > DirectionEpsilon)
			{
				Direction = Direction.Up;
			} 
			else if (input.y < -DirectionEpsilon)
			{
				Direction = Direction.Down;
			}

			//Check if we can move
			float moveMult = 1.0f;
			switch(playerActions.ActionState)
			{
				case ActionState.Shield: moveMult = shieldMoveMult;
					break;
				case ActionState.Carry: moveMult = carryMoveMult;
					break;
				case ActionState.Aim: moveMult = 0.0f;
					break;
				case ActionState.Melee:
				case ActionState.None:
				default:
					break;
			}

			//Apply movements based on our speed
			switch (MovementState)
			{
				case MovementState.Idle:
				case MovementState.Moving:
					rb.velocity = Input * (speed * moveMult);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}