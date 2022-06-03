using System;
using JetBrains.Annotations;
using ProjectAwakening.UI;
using UnityEngine;

namespace ProjectAwakening
{
	public class PauseCallerBehaviour : MonoBehaviour
	{
		private PauseMenuBehaviour pause;

		private void Awake()
		{
			pause = FindObjectOfType<PauseMenuBehaviour>();
		}

		[UsedImplicitly]
		private void OnPause()
		{
			if (pause == null) return;

			pause.TogglePause();
		}
	}
}