using System;
using MyBox;
using ProjectAwakening.Player.Character;
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
			UpdateLife(life.Lives);
		}

		private void OnEnable()
		{
			life.OnHurt += OnHurt;
		}

		private void OnHurt(int _)
		{
			UpdateLife(life.Lives);
		}

		private void UpdateLife(int lifeAmount)
		{
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
			else
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