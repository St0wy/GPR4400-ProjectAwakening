using System;
using UnityEngine;

namespace ProjectAwakening.Player
{
	[RequireComponent(typeof(PlayerMovement))]
	[RequireComponent(typeof(PlayerActions))]
	[RequireComponent(typeof(Animator))]
	public class PlayerAnimationBehaviour : MonoBehaviour
	{
		[SerializeField] private Transform bow;
		[SerializeField] private GameObject bowVisual;
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
			if (playerActions.ActionState != ActionState.Aim)
			{
				bowVisual.SetActive(false);
			}

			switch (playerActions.ActionState)
			{
				case ActionState.Aim:
					HandleBow();
					HandleNoAction();
					break;
				case ActionState.None:
					HandleNoAction();
					break;
				case ActionState.Shoot:
				case ActionState.Melee:
					HandleMelee();
					break;
				case ActionState.Shield:
					HandleShield();
					break;
				case ActionState.Carry:
				default:
					break;
			}
		}

		private void HandleBow()
		{
			bowVisual.SetActive(true);
			float angle = DirectionUtils.GetAngle(playerMovement.Direction) + 90f;
			bow.rotation = Quaternion.Euler(0, 0, angle);
		}

		private void HandleMelee()
		{
			switch (playerMovement.Direction)
			{
				case Direction.Up:
					SetAnimationState(PlayerAnimation.AttackUp);
					break;
				case Direction.Down:
					SetAnimationState(PlayerAnimation.AttackDown);
					break;
				case Direction.Left:
					SetAnimationState(PlayerAnimation.AttackLeft);
					break;
				case Direction.Right:
					SetAnimationState(PlayerAnimation.AttackRight);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
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