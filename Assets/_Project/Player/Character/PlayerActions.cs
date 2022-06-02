using System.Collections;
using JetBrains.Annotations;
using ProjectAwakening.Player.Sword;
using StowyTools.Logger;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAwakening.Player.Character
{
	[RequireComponent(typeof(PlayerInput), typeof(PlayerMovement))]
	public class PlayerActions : MonoBehaviour
	{
		#region Serialized Fields

		[SerializeField] private PlayerMovement playerMovement;
		[SerializeField] private SwordBehaviour sword;

		[Header("Action Parameters")]
		[Tooltip("Minimum time between arrow shots")]
		[SerializeField]
		private float timeBetweenShots = 0.5f;

		[SerializeField] private float chargeTime = 0.5f;
		[SerializeField] private float putBackBowTime = 0.1f;
		[SerializeField] private float arrowSpawnDistance = 0.5f;

		[Header("ItemsToSpawn")]
		[Tooltip("The arrow object to spawn when we fire with our bow")]
		[SerializeField]
		private GameObject arrow;
		[SerializeField]
		private GameObject chargedArrow;

		[SerializeField] private SoundRequest soundRequest;
		[SerializeField] private AudioClip swordAttackSound;
		[SerializeField] private AudioClip bowShootSound;

		#endregion

		private Coroutine chargingCoroutine;
		private Coroutine meleeCoroutine;

		private float timeToShoot;
		private bool askedForShield;
		private bool askedForAttack;

		public ActionState ActionState { get; private set; } = ActionState.None;
		public float AttackDuration { get; private set; }
		public bool IsCharged { get; private set; }

		private void Update()
		{
			if (timeToShoot >= 0.0f)
				timeToShoot -= Time.deltaTime;

			HandleBuffer();
		}

		private void HandleBuffer()
		{
			if (ActionState != ActionState.None) return;

			if (askedForShield)
			{
				ActionState = ActionState.Shield;
				askedForShield = false;
			}

			if (askedForAttack)
			{
				PerformMeleeAttack();
				askedForAttack = false;
			}
		}

		[UsedImplicitly]
		private void OnMelee(InputValue value)
		{
			askedForAttack = false;

			// Check that the action can be performed
			if (!value.isPressed) return;

			if (ActionState == ActionState.None)
			{
				PerformMeleeAttack();
			}
			else
			{
				askedForAttack = true;
			}
		}

		private void PerformMeleeAttack()
		{
			ActionState = ActionState.Melee;

			AttackDuration = sword.Attack(playerMovement.Direction);

			soundRequest.Request(swordAttackSound);

			// Return to normal after a time
			meleeCoroutine = StartCoroutine(SetStateDelayedCoroutine(AttackDuration));
		}

		[UsedImplicitly]
		private void OnBow(InputValue value)
		{
			//Check that the action can be performed
			bool isInGoodState = ActionState is ActionState.None or ActionState.Melee;
			if (value.isPressed && isInGoodState && timeToShoot <= 0.0f)
			{
				if (ActionState == ActionState.Melee)
				{
					StopAttack();
				}

				// Change state
				ActionState = ActionState.Aim;

				if (chargingCoroutine != null)
					StopCoroutine(chargingCoroutine);

				chargingCoroutine = StartCoroutine(ChargeBowCoroutine());
			}
			else if (!value.isPressed && ActionState == ActionState.Aim) //Release the shot on button release
			{
				// Change state
				ActionState = ActionState.Shoot;
				StartCoroutine(SetStateDelayedCoroutine(putBackBowTime));

				// Check that we haven't shot recently
				if (timeToShoot > 0.0f)
					return;

				Vector2 dir = PlayerMovement.DirectionToVector(playerMovement.Direction);

				//Check that the path to the arrow spawn is not obstrued
				if (Physics2D.Raycast(transform.position, dir, dir.magnitude, LayerMask.GetMask("Default")))
					return;

				// Create the arrow
				Quaternion rotation = Quaternion.Euler(0, 0, -90 * (int) playerMovement.Direction);
				Vector3 position = transform.position + (Vector3) dir * arrowSpawnDistance;
				Instantiate(IsCharged ? chargedArrow : arrow, position, rotation, null);

				soundRequest.Request(bowShootSound);

				// Make sure we can't shoot immediately after
				timeToShoot = timeBetweenShots;
			}
		}

		private void StopAttack()
		{
			StopCoroutine(meleeCoroutine);
			sword.StopAttackNow();
		}

		private IEnumerator ChargeBowCoroutine()
		{
			IsCharged = false;

			yield return new WaitForSeconds(chargeTime);

			IsCharged = true;
		}

		[UsedImplicitly]
		private void OnShield(InputValue value)
		{
			askedForShield = false;

			switch (value.isPressed)
			{
				case true when ActionState is ActionState.None or ActionState.Melee:
					if (ActionState == ActionState.Melee)
						StopAttack();
					ActionState = ActionState.Shield;
					break;
				case true:
					askedForShield = true;
					break;
				case false when ActionState == ActionState.Shield:
					ActionState = ActionState.None;
					break;
			}
		}

		private IEnumerator SetStateDelayedCoroutine(
			float timeToReturnToDefaultState,
			ActionState state = ActionState.None)
		{
			yield return new WaitForSeconds(timeToReturnToDefaultState);
			ActionState = state;
		}
	}
}