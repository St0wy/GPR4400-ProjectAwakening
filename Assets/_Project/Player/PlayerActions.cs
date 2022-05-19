using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAwakening.Player
{
	[RequireComponent(typeof(PlayerInput), typeof(PlayerMovement))]
	public class PlayerActions : MonoBehaviour
	{
		[SerializeField] private PlayerMovement playerMovement;

		[Header("Action Parameters")]
		[Tooltip("Time the melee attack lasts")]
		[SerializeField]
		private float attackTime;

		[Tooltip("Minimum time between arrow shots")]
		[SerializeField]
		private float timeBetweenShots = 0.5f;

		[SerializeField]
		private float arrowSpawnDistance = 0.5f;

		[Header("ItemsToSpawn")]
		[Tooltip("The arrow object to spawn when we fire with our bow")]
		[SerializeField]
		private GameObject arrow;

		private float timeToShoot = 0.0f;

		public ActionState ActionState { get; private set; } = ActionState.None;

		private void Update()
		{
			if (timeToShoot >= 0.0f)
				timeToShoot -= Time.deltaTime;
		}

		[UsedImplicitly]
		private void OnMelee(InputValue value)
		{
			//Check that the action can be performed
			if (value.isPressed && ActionState == ActionState.None)
			{
				//Change state
				ActionState = ActionState.Melee;

				//Return to normal after a time
				StartCoroutine(ReturnToDefaultStateCoroutine(attackTime));

				//Animation handles damagingEnemies
			}
		}

		[UsedImplicitly]
		private void OnBow(InputValue value)
		{
			//Check that the action can be performed
			if (value.isPressed && ActionState == ActionState.None)
			{
				//Change state
				ActionState = ActionState.Aim;
			}
			else if (!value.isPressed && ActionState == ActionState.Aim) //Release the shot on button release
			{
				//Change state
				ActionState = ActionState.None;

				//Check that we haven't shot recently
				if (timeToShoot > 0.0f)
					return;

				Vector2 dir = PlayerMovement.DirectionToVector(playerMovement.Direction);

				//Create the arrow
				Instantiate(arrow, transform.position + (Vector3) dir * arrowSpawnDistance,
					Quaternion.Euler(0, 0, -90 * (int) playerMovement.Direction), null);


				//Make sure we can't shoot immediately after
				timeToShoot = timeBetweenShots;
			}
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

		private IEnumerator ReturnToDefaultStateCoroutine(float timeToReturnToDefaultState)
		{
			yield return new WaitForSeconds(timeToReturnToDefaultState);

			ActionState = ActionState.None;
		}
	}
}