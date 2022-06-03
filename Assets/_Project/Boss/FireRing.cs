using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening
{
    public class FireRing : MonoBehaviour
    {
		[SerializeField]
		private float turnSpeed;

		[SerializeField]
		private float speedUpValue;

		[SerializeField]
		private Life life;

		[SerializeField]
		private int speedUpSteps;

		private float rotation = 0.0f;

		private int startLife;

		private void Start()
		{
			startLife = life.Lives;
		}

		void FixedUpdate()
        {
			if (startLife - life.Lives >= speedUpSteps)
			{
				turnSpeed += speedUpValue;
				startLife -= speedUpSteps;
			}

			transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotation += Time.fixedDeltaTime * turnSpeed ));
        }
    }
}
