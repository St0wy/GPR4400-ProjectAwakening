using UnityEngine;

namespace ProjectAwakening.Enemies.AI
{
    public class AIBase : MonoBehaviour
    {
		[SerializeField]
		protected TransformReferenceScriptableObject playerTransform;

		[SerializeField]
		protected float updateAIRate = 5.0f;

		protected Vector2 goal;

		protected bool isActive;
		
		[SerializeField]
		protected float inactiveTime = 1.0f;

		private float timeSinceLastAIUpdate;

		private bool isVisible;

		private void Update()
		{
			if (!isVisible)
				return;

			//Handle inactivity period
			if (inactiveTime > 0.0f)
				inactiveTime -= Time.deltaTime;
			else
				isActive = true;

			timeSinceLastAIUpdate += Time.deltaTime;

			if (timeSinceLastAIUpdate < 1 / updateAIRate)
				return;

			timeSinceLastAIUpdate = 0.0f;

			AIUpdate();
		}

		protected virtual void FixedUpdate()
		{
			if (!isVisible || !isActive)
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
