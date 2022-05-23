using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening.Enemies
{
    public class AIBase : MonoBehaviour
    {
		[SerializeField]
		TransformReferenceScriptableObject playerTransform;

		[SerializeField]
		protected float updateAIRate = 5.0f;

		protected Vector2 goal;

		float timeSinceLastAIUpdate = 0.0f;

		private void Update()
		{
			timeSinceLastAIUpdate += Time.deltaTime;

			if (timeSinceLastAIUpdate < 1 / updateAIRate)
				return;

			timeSinceLastAIUpdate = 0.0f;

			AIUpdate();
		}

		protected virtual void AIUpdate()
		{
			FindWhereToGo();
			Move();
		}

        protected virtual void FindWhereToGo()
		{
			goal = playerTransform.Transform.position;
		}

		protected virtual void Move()
		{

		}
    }
}
