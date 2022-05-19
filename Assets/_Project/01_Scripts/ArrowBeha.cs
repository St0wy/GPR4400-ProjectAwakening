using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening
{
    public class ArrowBeha : MonoBehaviour
    {
		[Header("Properties")]
		[SerializeField]
		private float speed = 35.0f;

		[Tooltip("Time during which the collider is disabled")]
		[SerializeField]
		private float inactiveTime = 0.3f;

		[Header("Components")]
		[SerializeField]
		private Rigidbody2D rb;

		[SerializeField]
		private Collider2D col;

        // Start is called before the first frame update
        void Start()
        {
			rb.velocity = transform.up * speed;

			//Collider starts disabled then gets enabled
			col.enabled = false;
			StartCoroutine(DelayedActivate());
        }

		IEnumerator DelayedActivate()
		{
			yield return new WaitForSeconds(inactiveTime);
			col.enabled = true;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			//Check for opponent life script
			//Deal Damage
			//Die
			Destroy(gameObject);
		}
	}
}
