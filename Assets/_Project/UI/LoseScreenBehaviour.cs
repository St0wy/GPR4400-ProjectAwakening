using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening
{
    public class LoseScreenBehaviour : MonoBehaviour
    {
        public void ReloadLevel()
		{
			GetComponent<UI.MainMenuBehaviour>().StopMusic();
			GameManager.Instance.ReloadCurrentScene();
		}
    }
}
