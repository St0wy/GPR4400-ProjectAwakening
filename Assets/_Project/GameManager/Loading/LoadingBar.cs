using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectAwakening.Loading
{
	public class LoadingBar : MonoBehaviour
	{
		[SerializeField]
		private List<GameObject> dots;

		[SerializeField]
		private Sprite onSprite;
		[SerializeField]
		private Sprite offSprite;

		private void OnEnable()
		{
			foreach (GameObject dot in dots)
			{
				dot.GetComponent<Image>().sprite = offSprite;
			}
		}

		public void UpdateProgress(float progress)
		{
			for (var index = 0; index < dots.Count; index++)
			{
				var image = dots[index].GetComponent<Image>();

				// The dot should be lit if it's inferior to the progress
				image.sprite = (float) index / dots.Count < progress ? onSprite : offSprite;
			}
		}
	}
}