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

		[SerializeField]
		Rigidbody2D rb;

        // Start is called before the first frame update
        void Start()
        {
			rb.velocity = Vector2.up * speed;
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
