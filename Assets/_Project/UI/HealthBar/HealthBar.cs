using System;
using MyBox;
using ProjectAwakening.Player.Character;
using StowyTools.Logger;
using UnityEngine;

namespace ProjectAwakening.UI
{
	public class HealthBar : MonoBehaviour
	{
		private const int MaxLife = 10;

		[MustBeAssigned] [SerializeField] private HeartBehaviour[] hearts;
		[SerializeField] private PlayerLife life;

		private void Start()
		{
			if (life == null)
			{
				life = FindObjectOfType<PlayerLife>();
			}

			life.OnHurt += OnHurt;

			UpdateLife(life.Lives);
		}

		private void OnDestroy()
		{
			life.OnHurt -= OnHurt;
		}

		private void OnHurt()
		{
			UpdateLife(life.Lives);
		}

		private void UpdateLife(int lifeAmount)
		{
			this.Log(lifeAmount);
			int half = lifeAmount / 2;
			int rest = lifeAmount % 2;

			for (var i = 0; i < half; i++)
			{
				hearts[i].ShowFull();
			}

			if (rest == 1)
			{
				hearts[half].ShowHalf();
			}
			else if (lifeAmount != MaxLife)
			{
				hearts[half].ShowEmpty();
			}

			for (int i = MaxLife / 2 - 1; i > half; i--)
			{
				hearts[i].ShowEmpty();
			}
		}
	}
}