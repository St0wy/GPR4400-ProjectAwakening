using MyBox;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectAwakening.UI
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Image))]
	public class HeartBehaviour : MonoBehaviour
	{
		[SerializeField] private Sprite heartFull;
		[SerializeField] private Sprite heartHalf;
		[SerializeField] private Sprite heartEmpty;

		private Image image;

		private void Awake()
		{
			image = GetComponent<Image>();
		}

		[ButtonMethod]
		public void ShowFull()
		{
			image.sprite = heartFull;
		}

		[ButtonMethod]
		public void ShowHalf()
		{
			image.sprite = heartHalf;
		}

		[ButtonMethod]
		public void ShowEmpty()
		{
			image.sprite = heartEmpty;
		}
	}
}