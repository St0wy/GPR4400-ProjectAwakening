using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectAwakening
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
			foreach (var dot in dots)
			{
				dot.GetComponent<Image>().sprite = offSprite;
			}
		}

		public void UpdateProgress(float progress)
		{
			for (int index = 0; index < dots.Count; index++)
			{
				//The dot should be lit if it's inferior to the progress
				if (((float) index) /((float) dots.Count) < progress)
				{
					dots[index].GetComponent<Image>().sprite = onSprite;
				}
				else
				{
					dots[index].GetComponent<Image>().sprite = offSprite;
				}
			}
		}
	}
}
