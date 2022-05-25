using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening.Enemies
{
    public class AIBase : MonoBehaviour
    {
		[SerializeField]
		protected TransformReferenceScriptableObject playerTransform;

		[SerializeField]
		protected float updateAIRate = 5.0f;

		protected Vector2 goal;

		float timeSinceLastAIUpdate = 0.0f;

		bool isVisible = false;

		private void Update()
		{
			if (!isVisible)
				return;

			timeSinceLastAIUpdate += Time.deltaTime;

			if (timeSinceLastAIUpdate < 1 / updateAIRate)
				return;

			timeSinceLastAIUpdate = 0.0f;

			AIUpdate();
		}

		protected virtual void FixedUpdate()
		{
			if (!isVisible)
				return;

			Move();
		}

		protected virtual void OnBecameVisible()
		{
			isVisible = true;

			FindWhereToGo();
		}

		protected virtual void OnBecameInvisible()
		{
			isVisible = false;
		}

		protected virtual void AIUpdate()
		{
			FindWhereToGo();
		}

        protected virtual void FindWhereToGo()
		{
			if (playerTransform.Transform == null)
				return;

			goal = playerTransform.Transform.position;
		}

		protected virtual void Move()
		{

		}
    }
}
