using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectAwakening.WorldGeneration
{
    public class WorldMap : MonoBehaviour
    {
		[Header("Parameters")]
		[Tooltip("Retry to generate a world map if the biggest zone is under that value")]
		[SerializeField]
		int minBiggestAreaSize;

		[Tooltip("Maximum number of times we try to generate the map")]
		[SerializeField]
		int maxTries;

		[Tooltip("Tilemap to draw on")]
		[SerializeField]
		Tilemap tileMap;

		[SerializeField]
		WaveCollapseMapMaker mapMaker;

		[SerializeField]
		SpecialElementsGenerator elementsGenerator;

		//Map as a bool array
		bool[,] wallMap;
		//Largest open area of ground
		List<Vector2Int> largestArea;

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
	}
}
