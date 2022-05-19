using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using ProjectAwakening.Enemies;

namespace ProjectAwakening.WorldGeneration
{
    public class WorldMap : MonoBehaviour
    {
		[SerializeField]
		private bool generateOnAwake = true;

		[Header("Parameters")]
		[Tooltip("Retry to generate a world map if the biggest zone is under that value")]
		[SerializeField]
		private int minBiggestAreaSize;

		[Tooltip("Maximum number of times we try to generate the map")]
		[SerializeField]
		private int maxTries;

		[Tooltip("Tilemap to draw on")]
		[SerializeField]
		private Tilemap tileMap;

		[SerializeField]
		private WaveCollapseMapMaker mapMaker;

		[SerializeField]
		private SpecialElementsGenerator elementsGenerator;

		[SerializeField]
		private SpawnEventScriptableObject spawnEvent;

		//Map as a bool array
		private bool[,] wallMap;
		//Largest open area of ground
		private List<Vector2Int> largestArea;

		private void Awake()
		{
			if (generateOnAwake)
			{
				Generate();
			}

			Spawn();
		}

		public void Generate()
		{
			int tries = 0;
			do
			{
				mapMaker.CreateMap();
				mapMaker.CreateMapVisuals();

				wallMap = WorldMapUtilities.superPositionMapToArray(mapMaker.SuperpositionsMap, mapMaker.TileSet);
				largestArea = WorldMapUtilities.GetLargestArea(wallMap, false);
			} while (largestArea.Count < minBiggestAreaSize && ++tries < maxTries);

			elementsGenerator.GenerateElementsInArea(WorldMapUtilities.ConvertTilemapToWorld(largestArea, tileMap));
		}

		public void Spawn()
		{
			spawnEvent.SpawnEnemies();
		}
	}
}
