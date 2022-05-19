using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAwakening.Player
{
	[RequireComponent(typeof(PlayerInput), typeof(PlayerMovement))]
    public class PlayerActions : MonoBehaviour
    {
		public ActionState ActionState { get; private set; } = ActionState.None;

		[SerializeField]
		PlayerMovement playerMovement;

		[Header("Action Parameters")]
		[Tooltip("Time the melee attack lasts")]
		[SerializeField]
		private float attackTime;

		[Tooltip("Minimum time between arrow shots")]
		[SerializeField]
		private float timeBetweenShots = 0.5f;
		float timeToShoot = 0.0f;

		[Header("ItemsToSpawn")]
		[Tooltip("The arrow object to spawn when we fire with our bow")]
		[SerializeField]
		private GameObject arrow;

		private void Update()
		{
			if (timeToShoot >= 0.0f)
				timeToShoot -= Time.deltaTime;
		}

		private void OnMelee(InputValue value)
		{
			//Check that the action can be performed
			if (value.isPressed && ActionState == ActionState.None)
			{
				//Change state
				ActionState = ActionState.Melee;

				//Return to normal after a time
				StartCoroutine(DelayedReturnToDefaultState(attackTime));

				//Animation handles damagingEnemies
			}
		}

		private void OnBow(InputValue value)
		{
			//Check that the action can be performed
			if (value.isPressed && ActionState == ActionState.None && timeToShoot <= 0.0f)
			{
				//Change state
				ActionState = ActionState.Aim;
			}
			else if (!value.isPressed && ActionState == ActionState.Aim) //Release the shot on button release
			{
				Vector2 dir = PlayerMovement.DirectionToVector(playerMovement.Direction);

				//Create the arrow
				Instantiate(arrow, transform.position + (Vector3) dir, Quaternion.Euler(0, 0, -90 * (int) playerMovement.Direction), null);

				//Change state
				ActionState = ActionState.None;

				//Make sure we can't shoot immediately after
				timeToShoot = timeBetweenShots;
			}
		}

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

		IEnumerator DelayedReturnToDefaultState(float time)
		{
			yield return new WaitForSeconds(time);

			ActionState = ActionState.None;
		}
	}
}
