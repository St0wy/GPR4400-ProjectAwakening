using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectAwakening.Player
{
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerActions : MonoBehaviour
	{
		public ActionState ActionState { get; private set; } = ActionState.None;

		[Header("Action Parameters")]
		[SerializeField]
		private float attackTime;

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
			if (value.isPressed && ActionState == ActionState.None)
			{
				//Change state
				ActionState = ActionState.Aim;
			}
			else if (!value.isPressed && ActionState == ActionState.Aim) //Release the shot on button release
			{
				//TODO Create arrow
				Debug.Log("TODO shoot arrow");

				//Change state
				ActionState = ActionState.None;
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