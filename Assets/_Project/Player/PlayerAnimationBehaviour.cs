using System;
using StowyTools.Logger;
using UnityEngine;

namespace ProjectAwakening.Player
{
	[RequireComponent(typeof(PlayerMovement))]
	[RequireComponent(typeof(PlayerActions))]
	[RequireComponent(typeof(Animator))]
	public class PlayerAnimationBehaviour : MonoBehaviour
	{
		private PlayerMovement playerMovement;
		private PlayerActions playerActions;
		private Animator animator;

		private string currentAnim;

		private void Awake()
		{
			playerMovement = GetComponent<PlayerMovement>();
			playerActions = GetComponent<PlayerActions>();
			animator = GetComponent<Animator>();
		}

		private void Update()
		{
			switch (playerActions.ActionState)
			{
				case ActionState.None:
					HandleNoAction();
					break;
				case ActionState.Melee:
					HandleMelee();
					break;
				case ActionState.Shield:
					HandleShield();
					break;
				case ActionState.Aim:
					HandleNoAction();
					break;
				case ActionState.Carry:
				default:
					break;
			}
		}

		private void HandleMelee()
		{
			HandleNoAction();
		}

		private void HandleShield()
		{
			switch (playerMovement.MovementState)
			{
				case MovementState.Idle:
					HandleIdleShield();
					break;
				case MovementState.Moving:
					HandleMovingShield();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void HandleMovingShield()
		{
			switch (playerMovement.Direction)
			{
				case Direction.Up:
					SetAnimationState(PlayerAnimation.WalkShieldUp);
					break;
				case Direction.Down:
					SetAnimationState(PlayerAnimation.WalkShieldDown);
					break;
				case Direction.Left:
					SetAnimationState(PlayerAnimation.WalkShieldLeft);
					break;
				case Direction.Right:
					SetAnimationState(PlayerAnimation.WalkShieldRight);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void HandleIdleShield()
		{
			switch (playerMovement.Direction)
			{
				case Direction.Up:
					SetAnimationState(PlayerAnimation.IdleShieldUp);
					break;
				case Direction.Down:
					SetAnimationState(PlayerAnimation.IdleShieldDown);
					break;
				case Direction.Left:
					SetAnimationState(PlayerAnimation.IdleShieldLeft);
					break;
				case Direction.Right:
					SetAnimationState(PlayerAnimation.IdleShieldRight);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void HandleNoAction()
		{
			switch (playerMovement.MovementState)
			{
				case MovementState.Idle:
					HandleIdleNoActions();
					break;
				case MovementState.Moving:
					HandleMovingNoAction();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void HandleMovingNoAction()
		{
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
		}

		private void HandleIdleNoActions()
		{
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
		}

		private void SetAnimationState(string newAnimation)
		{
			if (currentAnim == newAnimation) return;
			animator.Play(newAnimation);
			currentAnim = newAnimation;
		}
	}
}