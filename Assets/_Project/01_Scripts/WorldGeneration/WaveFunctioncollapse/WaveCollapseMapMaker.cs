using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] int maxIterations;
    [SerializeField] int maxDiscards;

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

            } while (continueLooping && iters++ < maxIterations);

            if (iters >= maxIterations)
                Debug.Log("MAX ITERATIONS REACHED");
        } while (!CheckMapValid(_superpositionsMap) && rejectedMaps++ < maxDiscards);
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
        for (int i = this.transform.childCount; i > 0; --i)
            DestroyImmediate(this.transform.GetChild(0).gameObject);

        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                if (_superpositionsMap.Map[x,y].Count != 1)
                {
                    Debug.LogError("What the fuck, there's a hole there : " + x.ToString() + "/" + y.ToString());
                }
                else
                {
                    GameObject tile = new GameObject();
                    tile.transform.parent = transform;
                    tile.transform.position = Vector3.zero + Vector3.right * x + Vector3.up * y;

                    tile.transform.Rotate(new Vector3(0, 0, -90 * _superpositionsMap.Map[x,y][0].Rotation));
                    tile.AddComponent<SpriteRenderer>().sprite = _tileSet.Tiles[_superpositionsMap.Map[x, y][0].Id].TileImg;
                }
            }
        }
    }
}
