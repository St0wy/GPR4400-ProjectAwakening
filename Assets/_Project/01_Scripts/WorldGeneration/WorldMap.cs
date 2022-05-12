using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectAwakening.WorldGeneration
{
    public class WorldMap : MonoBehaviour
    {
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
			mapMaker.CreateMap();
			mapMaker.CreateMapVisuals();

			wallMap = WorldMapUtilities.superPositionMapToArray(mapMaker.SuperpositionsMap, mapMaker.TileSet);
			largestArea = WorldMapUtilities.GetLargestArea(wallMap, false);

			elementsGenerator.GenerateElementsInArea(WorldMapUtilities.ConvertTilemapToWorld(largestArea, tileMap));
		}
	}
}
