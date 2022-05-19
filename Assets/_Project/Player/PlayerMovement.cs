using System;
using JetBrains.Annotations;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAwakening.Player
{
	[RequireComponent(typeof(PlayerActions), typeof(Life))]
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
		private Life life;
	
		public MovementState MovementState { get; private set; } = MovementState.Idle;
		
		public Direction Direction { get; set; }

		public static Vector2 DirectionToVector(Direction direction)
		{
			return direction switch
			{
				Direction.Up => Vector2.up,
				Direction.Down => Vector2.down,
				Direction.Left => Vector2.left,
				Direction.Right => Vector2.right,
				_ => Vector2.zero,
			};
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
			life = GetComponent<Life>();
		}

		[UsedImplicitly]
		private void OnMove(InputValue value)
		{
			//Get the directional value
			Input = value.Get<Vector2>();
		}

		private void FixedUpdate()
		{
			if (life.IsDead || life.IsBeingKnockedBack)
				return;

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
				case ActionState.Melee:
				case ActionState.Aim: moveMult = 0.0f;
					break;
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