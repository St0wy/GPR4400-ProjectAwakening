using UnityEditor;
using UnityEngine;

namespace ProjectAwakening.Enemies.Spawning
{
	public class MonsterSpawnerContextMenuItem : MonoBehaviour
	{
		public const string PrefabName = "DungeonMonsterSpawner";

		[MenuItem("GameObject/Monster Spawner")]
		public static void CreateMonsterSpawner()
		{
			var prefab = Resources.Load(PrefabName, typeof(GameObject)) as GameObject;
			if (prefab == null)
			{
				Debug.LogError("Did not find prefab");
				return;
			}

			PrefabUtility.InstantiatePrefab(prefab);
		}
	}
}