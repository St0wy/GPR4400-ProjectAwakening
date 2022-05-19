using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening.Enemies
{
	[CreateAssetMenu( fileName = "SpawnEvent", menuName = "Events/SpawnEvent", order = 0)]
    public class SpawnEventScriptableObject : ScriptableObject
    {
		public delegate void SpawnEnemiesEvent();

		public SpawnEnemiesEvent OnSpawnEnemies { get; set; }

		public void SpawnEnemies()
		{
			OnSpawnEnemies?.Invoke();
		}
	}
}
