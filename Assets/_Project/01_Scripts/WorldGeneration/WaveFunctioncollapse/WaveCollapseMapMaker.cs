using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class WaveCollapseMapMaker : MonoBehaviour
{
    [Header("Map")]

    [Tooltip("TileSet used to make the map")]
    [SerializeField] TileSetScriptable _tileSet;

    [Tooltip("Size of the map")]
    [SerializeField] Vector2Int _size;

    [Tooltip("Wether a tile on the edge should be considered neighbouring the opposite edge")]
    [SerializeField] bool _wrapAround = false;

    [Header("Security")]
    [Tooltip("Max number of times we can choose a random tile to collapse")]
    [SerializeField] int _maxIterations;
    [Tooltip("Number of time we can retry to make a map if the generation failed")]
    [SerializeField] int _maxDiscards;

    [Header("Drawing")]
    [Tooltip("The tilemap to draw the map on")]
    [SerializeField] Tilemap _tilemap;
    [Tooltip("The left bottom corner of the map")]
    [SerializeField] Vector2Int _drawOrigin;

    [SerializeField] bool _fillBorderWithRuleTile = false;
    [SerializeField] RuleTile _borderRuleTile;

    SuperpositionsMap _superpositionsMap;
   

    public void CreateMap()
    {
        _superpositionsMap = new SuperpositionsMap(_size, _tileSet, _wrapAround);
        _superpositionsMap.PopulateMap();

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
                Vector2Int lowestEntropyPoint = _superpositionsMap.FindLowestEntropy();
                //Stop if we don't have entropy
                if (lowestEntropyPoint.y < 0)
                {
                    continueLooping = false;
                    continue;
                }

                //Randomly choose one tile
                //Equal chances for now
                int choice = _superpositionsMap.ChooseRandomByWeight(lowestEntropyPoint);
                _superpositionsMap.CollapsePossibilities(lowestEntropyPoint, choice);

            } while (continueLooping && iters++ < _maxIterations);

            if (iters >= _maxIterations)
                Debug.Log("MAX ITERATIONS REACHED");
        } while (!CheckMapValid(_superpositionsMap) && rejectedMaps++ < _maxDiscards);
    }

    bool CheckMapValid(SuperpositionsMap map)
    {
        foreach (var list in map.Map)
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
        _tilemap.ClearAllTiles();

        //Draw the tiles in superpositions map
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                if (_superpositionsMap.Map[x,y].Count != 1)
                {
                }
                else
                {
                    //Grab the tile
                    TileScriptable tile = _tileSet.Tiles[_superpositionsMap.Map[x, y][0].Id];

                    //Figure out the position
                    Vector3Int pos = new Vector3Int(_drawOrigin.x + x, _drawOrigin.y + y, 0);

                    //Print the tile to the tileset
                    if (tile.UseRuleTile)
                    {
                        _tilemap.SetTile(pos, tile.RuleTile);
                    }
                    else
                    {
                        _tilemap.SetTile(pos, tile.Tile);
                    }
                }
            }
        }

        //Draw the borders
        if (_fillBorderWithRuleTile)
        {
            //Fill horizontally outside the map
            for (int x = -1; x <= _size.x; x++)
            {
                //Set position above the map
                int y = -1;
                Vector3Int pos = new Vector3Int(_drawOrigin.x + x, _drawOrigin.y + y, 0);

                _tilemap.SetTile(pos, _borderRuleTile);

                //Set position below the map
                y = _size.y;
                pos = new Vector3Int(_drawOrigin.x + x, _drawOrigin.y + y, 0);

                _tilemap.SetTile(pos, _borderRuleTile);
            }

            //Same for vertical
            for (int y = 0; y < _size.y; y++)
            {
                //Set position to the left of the map
                int x = -1;
                Vector3Int pos = new Vector3Int(_drawOrigin.x + x, _drawOrigin.y + y, 0);

                _tilemap.SetTile(pos, _borderRuleTile);

                //Set position to the right of the map
                 x = _size.x;
                pos = new Vector3Int(_drawOrigin.x + x, _drawOrigin.y + y, 0);

                _tilemap.SetTile(pos, _borderRuleTile);
            }
        }

        //Apply the changes
        _tilemap.RefreshAllTiles();
    }
}
