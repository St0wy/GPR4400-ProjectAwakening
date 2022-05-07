using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class WaveCollapseMapMaker : MonoBehaviour
{
    [SerializeField] RuleTile _rule;

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
        //Destroy all previous children
        for (int i = this.transform.childCount; i > 0; --i)
            DestroyImmediate(this.transform.GetChild(0).gameObject);

        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                if (_superpositionsMap.Map[x,y].Count != 1)
                {
                }
                else
                {
                    RuleTile tileToDraw = _tileSet.Tiles[_superpositionsMap.Map[x, y][0].Id].Tile;
                    //tileToDraw.sprite = _tileSet.Tiles[_superpositionsMap.Map[x, y][0].Id].TileImg;

                    Vector3Int pos = new Vector3Int(_drawOrigin.x + x, _drawOrigin.y + y, 0);

                    //var m = tileToDraw.transform;
                    //m.SetTRS(Vector3.zero, Quaternion.Euler(new Vector3(0, 0, 360 - (90 * _superpositionsMap.Map[x, y][0].Rotation))), Vector3.one * 0.5f);
                    // tileToDraw.transform = m;

                    _tilemap.SetTile(pos, tileToDraw);
                }
            }
        }
        _tilemap.RefreshAllTiles();
    }
}
