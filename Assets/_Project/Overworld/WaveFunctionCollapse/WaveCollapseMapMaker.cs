using System.Collections.Generic;
using System.Collections;
using System.Linq;
using StowyTools.Logger;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace ProjectAwakening.Overworld.WaveFunctionCollapse
{
	[ExecuteInEditMode]
	public class WaveCollapseMapMaker : MonoBehaviour
	{
		[Header("Map")]
		[Tooltip("TileSet used to make the map")]
		[SerializeField]
		private TileSetScriptable tileSet;

		[Tooltip("Size of the map")]
		[SerializeField]
		private Vector2Int size;

		[Tooltip("Whether a tile on the edge should be considered neighbouring the opposite edge")]
		[SerializeField]
		private bool wrapAround;

		[Header("Security")]
		[Tooltip("Max number of times we can choose a random tile to collapse")]
		[SerializeField]
		private int maxIterations;

		[Tooltip("Number of time we can retry to make a map if the generation failed")]
		[SerializeField]
		private int maxDiscards;

		[Header("Drawing")]
		[Tooltip("The tilemap to draw the map on")]
		[SerializeField]
		private Tilemap tilemap;

		[Tooltip("The left bottom corner of the map")]
		[SerializeField]
		private Vector2Int drawOrigin;

		[SerializeField] private bool fillBorderWithRuleTile;
		[SerializeField] private RuleTile borderRuleTile;
		[SerializeField] private int borderWidth;

		[FormerlySerializedAs("astar")] [SerializeField]
		private AstarPath aStar;

		public SuperpositionsMap SuperpositionsMap { get; private set; }

		public TileSetScriptable TileSet => tileSet;

		public void CreateMap()
		{
			SuperpositionsMap = new SuperpositionsMap(size, tileSet, wrapAround);
			SuperpositionsMap.PopulateMap();

			//Make a choice, apply the consequences
			var continueLooping = true;
			var iterations = 0;
			var rejectedMaps = 0;
			do
			{
				if (rejectedMaps > 0)
					this.Log("Map Rejected");

				do
				{
					// Make a choice
					// Find the lowest entropy part
					Vector2Int lowestEntropyPoint = SuperpositionsMap.FindLowestEntropy();

					// Stop if we don't have entropy
					if (lowestEntropyPoint.y < 0)
					{
						continueLooping = false;
						continue;
					}

					// Randomly choose one tile
					// Equal chances for now
					int choice = SuperpositionsMap.ChooseRandomByWeight(lowestEntropyPoint);
					SuperpositionsMap.CollapsePossibilities(lowestEntropyPoint, choice);
				} while (continueLooping && iterations++ < maxIterations);

				if (iterations >= maxIterations)
					this.Log("MAX ITERATIONS REACHED");
			} while (!CheckMapValid(SuperpositionsMap) && rejectedMaps++ < maxDiscards);
		}

		private static bool CheckMapValid(SuperpositionsMap map)
		{
			return map.SuperpositionMap.Cast<List<TileWfc>>().All(list => list.Count == 1);
		}

		public void CreateMapVisuals()
		{
			// Reset tilemap
			tilemap.ClearAllTiles();

			// Draw the tiles in superpositions map
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					if (SuperpositionsMap.SuperpositionMap[x, y].Count != 1) continue;

					//Grab the tile
					TileScriptable tile = tileSet.Tiles[SuperpositionsMap.SuperpositionMap[x, y][0].Id];

					//Figure out the position
					var pos = new Vector3Int(drawOrigin.x + x, drawOrigin.y + y, 0);

					//Print the tile to the tileset
					if (tile.UseRuleTile)
					{
						tilemap.SetTile(pos, tile.RuleTile);
					}
					else
					{
						tilemap.SetTile(pos, tile.Tile);
					}
				}
			}

			//Draw the borders
			if (fillBorderWithRuleTile)
			{
				//Fill horizontally outside the map
				for (int x = -borderWidth; x < size.x + borderWidth; x++)
				{
					//Set position below the map
					for (int y = -borderWidth; y < 0; y++)
					{
						var pos = new Vector3Int(drawOrigin.x + x, drawOrigin.y + y, 0);

						tilemap.SetTile(pos, borderRuleTile);
					}

					//Set position above the map
					for (int y = size.y + borderWidth - 1; y >= size.y; y--)
					{
						var pos = new Vector3Int(drawOrigin.x + x, drawOrigin.y + y, 0);

						tilemap.SetTile(pos, borderRuleTile);
					}
				}

				//Same for vertical
				for (int y = 0; y < size.y; y++)
				{
					//Set position to the left of the map
					for (int x = -borderWidth; x < 0; x++)
					{
						var pos = new Vector3Int(drawOrigin.x + x, drawOrigin.y + y, 0);

						tilemap.SetTile(pos, borderRuleTile);
					}

					//Set position to the right of the map
					for (int x = size.x + borderWidth - 1; x >= size.x; x--)
					{
						var pos = new Vector3Int(drawOrigin.x + x, drawOrigin.y + y, 0);

						tilemap.SetTile(pos, borderRuleTile);
					}
				}
			}

			//Apply the changes
			tilemap.RefreshAllTiles();

			StartCoroutine(RegenerateNavMesh());
		}

		IEnumerator RegenerateNavMesh()
		{
			//Wait two frames for tiles to exist and be ready for detection
			yield return null;
			yield return null;

			aStar.Scan();
		}
	}
}