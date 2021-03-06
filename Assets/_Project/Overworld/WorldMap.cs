using System.Collections.Generic;
using ProjectAwakening.Enemies.Spawning;
using ProjectAwakening.Overworld.WaveFunctionCollapse;
using ProjectAwakening.Overworld.WorldDetailsGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectAwakening.Overworld
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
		private RuleTile wallRuleTile;

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
		}

		private void Start()
		{
			Spawn();
		}

		public void Generate()
		{
			var tries = 0;
			do
			{
				mapMaker.CreateMap();
				mapMaker.CreateMapVisuals();

				wallMap = WorldMapUtilities.SuperPositionMapToArray(mapMaker.SuperpositionsMap, mapMaker.TileSet);
				if (wallRuleTile != null)
					largestArea = WorldMapUtilities.FloodFillAndGetLargest(wallMap, false, tileMap, wallRuleTile);
				else
					largestArea = WorldMapUtilities.FloodFillAndGetLargest(wallMap, false);
			} while (largestArea.Count < minBiggestAreaSize && ++tries < maxTries);

			elementsGenerator.GenerateElementsInArea(WorldMapUtilities.ConvertTilemapToWorld(largestArea, tileMap));
		}

		public void Spawn()
		{
			spawnEvent.SpawnEnemies();
		}
	}
}
