using System.Collections;
using JetBrains.Annotations;
using ProjectAwakening.Player.Sword;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAwakening.Player
{
	[RequireComponent(typeof(PlayerInput), typeof(PlayerMovement))]
	public class PlayerActions : MonoBehaviour
	{
		[SerializeField] private PlayerMovement playerMovement;
		[SerializeField] private SwordBehaviour sword;

		[Header("Action Parameters")]
		[Tooltip("Minimum time between arrow shots")]
		[SerializeField]
		private float timeBetweenShots = 0.5f;

		[SerializeField]
		private float chargeTime = 0.5f;
		private Coroutine chargingCoroutine = null;

		[SerializeField] private float putBackBowTime = 0.1f;

		[SerializeField]
		private float arrowSpawnDistance = 0.5f;

		[Header("ItemsToSpawn")]
		[Tooltip("The arrow object to spawn when we fire with our bow")]
		[SerializeField]
		private GameObject arrow;
		[SerializeField]
		private GameObject chargedArrow;

		private float timeToShoot;

		public ActionState ActionState { get; private set; } = ActionState.None;
		public float AttackDuration { get; private set; }

		public bool IsCharged { get; private set; } = false;

		private void Update()
		{
			if (timeToShoot >= 0.0f)
				timeToShoot -= Time.deltaTime;
		}

		[UsedImplicitly]
		private void OnMelee(InputValue value)
		{
			// Check that the action can be performed
			if (!value.isPressed || ActionState != ActionState.None) return;

			// Change state
			ActionState = ActionState.Melee;

			AttackDuration = sword.Attack(playerMovement.Direction);

			// Return to normal after a time
			StartCoroutine(SetStateDelayedCoroutine(AttackDuration));
		}

		[UsedImplicitly]
		private void OnBow(InputValue value)
		{
			//Check that the action can be performed
			if (value.isPressed && ActionState == ActionState.None && timeToShoot <= 0.0f)
			{
				// Change state
				ActionState = ActionState.Aim;

				if (chargingCoroutine != null)
					StopCoroutine(chargingCoroutine);

				chargingCoroutine = StartCoroutine(ChargeBow());
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

				// Create the arrow
				Quaternion rotation = Quaternion.Euler(0, 0, -90 * (int) playerMovement.Direction);
				Vector3 position = transform.position + (Vector3) dir * arrowSpawnDistance;
				Instantiate(IsCharged ? chargedArrow : arrow, position, rotation, null);

				// Make sure we can't shoot immediately after
				timeToShoot = timeBetweenShots;
			}
		}

		IEnumerator ChargeBow()
		{
			IsCharged = false;

			yield return new WaitForSeconds(chargeTime);

			IsCharged = true;
		}

		[UsedImplicitly]
		private void OnShield(InputValue value)
		{
			if (value.isPressed && ActionState == ActionState.None)
			{
				//Change State
				ActionState = ActionState.Shield;
			}
			else if (!value.isPressed && ActionState == ActionState.Shield) //Release the shield
			{
				//Change State
				ActionState = ActionState.None;
			}
		}

		private IEnumerator SetStateDelayedCoroutine(float timeToReturnToDefaultState,
			ActionState state = ActionState.None)
		{
			yield return new WaitForSeconds(timeToReturnToDefaultState);

			ActionState = state;
		}
	}
}