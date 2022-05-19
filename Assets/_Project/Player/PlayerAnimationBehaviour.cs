using System;
using UnityEngine;

namespace ProjectAwakening.Player
{
	[RequireComponent(typeof(PlayerMovement))]
	[RequireComponent(typeof(Animator))]
	public class PlayerAnimationBehaviour : MonoBehaviour
	{
		private PlayerMovement playerMovement;
		private Animator animator;

		private string currentAnim;

		private void Awake()
		{
			playerMovement = GetComponent<PlayerMovement>();
			animator = GetComponent<Animator>();
		}

		private void Update()
		{
			switch (playerMovement.MovementState)
			{
				case MovementState.Idle:
					switch (playerMovement.Direction)
					{
						case Direction.Up:
							SetAnimationState(PlayerAnimation.IdleUp);
							break;
						case Direction.Down:
							SetAnimationState(PlayerAnimation.IdleDown);
							break;
						case Direction.Left:
							SetAnimationState(PlayerAnimation.IdleLeft);
							break;
						case Direction.Right:
							SetAnimationState(PlayerAnimation.IdleRight);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					break;
				case MovementState.Moving:
					switch (playerMovement.Direction)
					{
						case Direction.Up:
							SetAnimationState(PlayerAnimation.WalkUp);
							break;
						case Direction.Down:
							SetAnimationState(PlayerAnimation.WalkDown);
							break;
						case Direction.Left:
							SetAnimationState(PlayerAnimation.WalkLeft);
							break;
						case Direction.Right:
							SetAnimationState(PlayerAnimation.WalkRight);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void SetAnimationState(string newAnimation)
		{
			if (currentAnim == newAnimation) return;
			animator.Play(newAnimation);
			currentAnim = newAnimation;
		}
	}
}