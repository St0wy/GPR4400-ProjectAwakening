using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectAwakening.WorldGeneration
{
    [ExecuteInEditMode]
    public class WaveCollapseMapMaker : MonoBehaviour
    {
        [Header("Map")]

        [Tooltip("TileSet used to make the map")]
        [SerializeField] TileSetScriptable tileSet;

        [Tooltip("Size of the map")]
        [SerializeField] Vector2Int size;

        [Tooltip("Wether a tile on the edge should be considered neighbouring the opposite edge")]
        [SerializeField] bool wrapAround = false;

        [Header("Security")]
        [Tooltip("Max number of times we can choose a random tile to collapse")]
        [SerializeField] int maxIterations;
        [Tooltip("Number of time we can retry to make a map if the generation failed")]
        [SerializeField] int maxDiscards;

        [Header("Drawing")]
        [Tooltip("The tilemap to draw the map on")]
        [SerializeField] Tilemap tilemap;
        [Tooltip("The left bottom corner of the map")]
        [SerializeField] Vector2Int drawOrigin;

        [SerializeField] bool fillBorderWithRuleTile = false;
        [SerializeField] RuleTile borderRuleTile;

        SuperpositionsMap superpositionsMap;

        public SuperpositionsMap SuperpositionsMap { get => superpositionsMap; private set => superpositionsMap = value; }
        public TileSetScriptable TileSet { get => tileSet; private set => tileSet = value; }

        public void CreateMap()
        {
            superpositionsMap = new SuperpositionsMap(size, tileSet, wrapAround);
            superpositionsMap.PopulateMap();

            //Make a choice, apply the consequences
            bool continueLooping = true;
            int iters = 0;
            int rejectedMaps = 0;
            do
            {
                if (rejectedMaps > 0)
                    Debug.Log("Map Rejected");

                do
                {
                    //Make a choice
                    //Find the lowest entropy part
                    Vector2Int lowestEntropyPoint = superpositionsMap.FindLowestEntropy();
                    //Stop if we don't have entropy
                    if (lowestEntropyPoint.y < 0)
                    {
                        continueLooping = false;
                        continue;
                    }

                    //Randomly choose one tile
                    //Equal chances for now
                    int choice = superpositionsMap.ChooseRandomByWeight(lowestEntropyPoint);
                    superpositionsMap.CollapsePossibilities(lowestEntropyPoint, choice);

                } while (continueLooping && iters++ < maxIterations);

                if (iters >= maxIterations)
                    Debug.Log("MAX ITERATIONS REACHED");
            } while (!CheckMapValid(superpositionsMap) && rejectedMaps++ < maxDiscards);
        }

        bool CheckMapValid(SuperpositionsMap map)
        {
            foreach (var list in map.SuperpositionMap)
            {
                //A valid map only contains one element per tile, not 0 nor more
                if (list.Count != 1)
                    return false;
            }

            return true;
        }

        public void CreateMapVisuals()
        {
            //Reset tilemap
            tilemap.ClearAllTiles();

            //Draw the tiles in superpositions map
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    if (superpositionsMap.SuperpositionMap[x, y].Count != 1)
                    {
                    }
                    else
                    {
                        //Grab the tile
                        TileScriptable tile = tileSet.Tiles[superpositionsMap.SuperpositionMap[x, y][0].Id];

                        //Figure out the position
                        Vector3Int pos = new Vector3Int(drawOrigin.x + x, drawOrigin.y + y, 0);

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
            }

            //Draw the borders
            if (fillBorderWithRuleTile)
            {
                //Fill horizontally outside the map
                for (int x = -1; x <= size.x; x++)
                {
                    //Set position above the map
                    int y = -1;
                    Vector3Int pos = new Vector3Int(drawOrigin.x + x, drawOrigin.y + y, 0);

                    tilemap.SetTile(pos, borderRuleTile);

                    //Set position below the map
                    y = size.y;
                    pos = new Vector3Int(drawOrigin.x + x, drawOrigin.y + y, 0);

                    tilemap.SetTile(pos, borderRuleTile);
                }

                //Same for vertical
                for (int y = 0; y < size.y; y++)
                {
                    //Set position to the left of the map
                    int x = -1;
                    Vector3Int pos = new Vector3Int(drawOrigin.x + x, drawOrigin.y + y, 0);

                    tilemap.SetTile(pos, borderRuleTile);

                    //Set position to the right of the map
                    x = size.x;
                    pos = new Vector3Int(drawOrigin.x + x, drawOrigin.y + y, 0);

                    tilemap.SetTile(pos, borderRuleTile);
                }
            }

            //Apply the changes
            tilemap.RefreshAllTiles();
        }
    }
}