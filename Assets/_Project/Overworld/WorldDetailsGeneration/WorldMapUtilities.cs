using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectAwakening.WorldGeneration
{
    public static class WorldMapUtilities
    {
        /// <summary>
        /// Convert a tilemap to an array of dimension 2 where true corresponds to a wall
        /// </summary>
        /// <param name="tilemap">Tilemap to read from</param>
        /// <returns>array dim2 where true means there is a wall there</returns>
        public static bool[,] superPositionMapToArray(SuperpositionsMap oldMap, TileSetScriptable tileSet)
        {
            Vector2Int size = new Vector2Int(oldMap.SuperpositionMap.GetLength(0), oldMap.SuperpositionMap.GetLength(1));
            bool[,] newMap = new bool[size.x, size.y];

            //Populate the map
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    //Check that the tile has no collider
                    RuleTile rule = tileSet.Tiles[oldMap[x,y][0].Id].RuleTile;
                   if (rule.m_DefaultColliderType != Tile.ColliderType.None)
                   {
                        newMap[x, y] = true;
                   }
                   else
                   {
                        newMap[x, y] = false;
                   }
                }
            }

            return newMap;
        }

        /// <summary>
        /// Finds via flood fill the largest area in a map
        /// </summary>
        /// <param name="map">the map to find in</param>
        /// <param name="matchValue">wether we match against true or false</param>
        /// <returns>all positions in the map that belong to the largest area</returns>
        public static List<Vector2Int> GetLargestArea(bool[,] map, bool matchValue = true)
        {
            //Storage for the amount of tiles in each area
            Dictionary<int, int> amountsPerId = new Dictionary<int, int>();

            Vector2Int size = new Vector2Int(map.GetLength(0), map.GetLength(1));

            //Map of ids to know what tiles belong to what area
            int[,] ids = new int[size.x, size.y];

            int curId = 0;

            //Fill ids with 0s
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    ids[x, y] = 0;
                }
            }

            //Call flood fill on each valid tile
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    //Check tile is valid
                    if (map[x, y] == matchValue && ids[x, y] == 0)
                    {
                        curId++;
                        amountsPerId.Add(curId, RecursiveFloodFill(new Vector2Int(x,y), curId, ids, map));
                    }
                }
            }

            //Find largest of the found areas
            int largest = 0;
            int biggestId = 0;
            foreach (var keyPair in amountsPerId)
            {
                if (keyPair.Value > largest)
                {
                    largest = keyPair.Value;
                    biggestId = keyPair.Key;
                }
            }

            //Return the tiles belonging to that area
            List<Vector2Int> tilesPos = new List<Vector2Int>();
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    if (ids[x,y] == biggestId)
                    {
                        tilesPos.Add(new Vector2Int(x, y));
                    }
                }
            }

            return tilesPos;
        }

        /// <summary>
        /// Recursively flood fills a tile and its neighbours to find an area
        /// </summary>
        /// <param name="pos">The position of the current tile</param>
        /// <param name="id">The id of the area</param>
        /// <param name="idMap">Map of ids where we set the id</param>
        /// <param name="map">the map where we check if something can belong to our area</param>
        /// <returns>the number of tiles in the area</returns>
        private static int RecursiveFloodFill(Vector2Int pos, int id, int[,] idMap, bool[,] map)
        { 
            //Set our id
            idMap[pos.x, pos.y] = id;

            int amountOfTilesFilled = 1;

            Vector2Int direction = Vector2Int.down;

            for (int i = 0; i < 4; i++)
            {
                //Switch direction
                switch (i)
                {
                    case 0: direction = Vector2Int.down;
                        break;
                    case 1: direction = Vector2Int.left;
                        break;
                    case 2: direction = Vector2Int.right;
                        break;
                    case 3: direction = Vector2Int.up;
                        break;
                }

                Vector2Int neighbourPos = pos + direction;

                //Check that neighbourPos is valid
                if (neighbourPos.x < 0 || neighbourPos.x >= map.GetLength(0) || neighbourPos.y < 0 || neighbourPos.y >= map.GetLength(1))
                    continue;

                //Check that neighbour can be added
                if (idMap[neighbourPos.x, neighbourPos.y] != 0 || map[neighbourPos.x, neighbourPos.y] != map[pos.x, pos.y])
                    continue;

                //Call recursive floodFill on that tile and the result
                amountOfTilesFilled += RecursiveFloodFill(neighbourPos, id, idMap, map);
            }

            return amountOfTilesFilled;
        }

		public static List<Vector3> ConvertTilemapToWorld(List <Vector2Int> tilemapPositions, Tilemap conversionTilemap)
		{
			List<Vector3> newPositions = new List<Vector3>();

			foreach (Vector2Int oldPos in tilemapPositions)
			{
				newPositions.Add(conversionTilemap.CellToWorld((Vector3Int) oldPos));
			}

			return newPositions;
		}
    }
}
