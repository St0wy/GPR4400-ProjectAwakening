using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperpositionsMap
{
    Vector2Int _size;
    TileSetScriptable _tileSet;
    bool _wrapAround = false;

    public SuperpositionsMap(Vector2Int size, TileSetScriptable tileSet)
    {
        _size = size;
        _tileSet = tileSet;
    }

    public SuperpositionsMap(Vector2Int size, TileSetScriptable tileSet, bool wrapAround)
    {
        _size = size;
        _tileSet = tileSet;
        _wrapAround = wrapAround;
    }

    //map where each tile is in a superposition of possibilities, we store the index and the rotation
    List<Tile>[,] _superpositionMap;
    public List<Tile> PossibilitiesAt(Vector2Int pos) { return _superpositionMap[pos.x, pos.y]; }
    public List<Tile>[,] Map { get => _superpositionMap; }

    //Populate possibilities assuming every tile is possible
    public void PopulateMap()
    {
        _superpositionMap = new List<Tile>[_size.x, _size.y];
        for (int x = 0; x < _superpositionMap.GetLength(0); x++)
        {
            for (int y = 0; y < _superpositionMap.GetLength(1); y++)
            {
                List<Tile> possibleTiles = new List<Tile>();

                //Put each tile
                for (int index = 0; index < _tileSet.Tiles.Count; index++)
                {
                    //in each possible rotation
                    if (_tileSet.Tiles[index].CanRotate)
                        for (int rotation = 0; rotation < 4; rotation++)
                        {
                            possibleTiles.Add(new Tile(index, rotation));
                        }
                    else
                        possibleTiles.Add(new Tile(index, 0));
                }

                _superpositionMap[x, y] = possibleTiles;
            }
        }
    }

    /// <summary>
    /// Find the lowest point of entropy (lesser amount of possibilities) within the map
    /// </summary>
    /// <returns>Returns the position of the lowest entropy point or (0, -1) if all points have no entropy</returns>
    public Vector2Int FindLowestEntropy()
    {
        int lowestEntropy = int.MaxValue;
        Vector2Int lowestEntropyPoint = Vector2Int.down;

        //Go over the whole map
        for (int x = 0; x < _superpositionMap.GetLength(0); x++)
        {
            for (int y = 0; y < _superpositionMap.GetLength(1); y++)
            {
                //Check amount of entropy
                //Check that the tile has entropy
                if (_superpositionMap[x, y].Count > 1)
                {
                    //Check if the tile has less entropy than current lowest
                    if (_superpositionMap[x, y].Count < lowestEntropy)
                    {
                        lowestEntropy = _superpositionMap[x, y].Count;
                        lowestEntropyPoint = new Vector2Int(x, y);
                    }
                }
            }
        }

        return lowestEntropyPoint;
    }

    /// <summary>
    /// Recalculate the possible tiles in neighbourings slots. Only call this on changed slots
    /// </summary>
    /// <param name="pos"></param>
    void RecalculatePossibilities(Vector2Int pos)
    {
        Vector2Int direction = Vector2Int.up;

        do
        {
            //Rotate the direction we look at
            direction = TileScriptable.NumDirectionToVector((TileScriptable.VectorToNumDirection(direction) + 1) % 4);
            Vector2Int pos2 = pos + direction;

            //Check if the second tile would be outside the map
            if (pos2.x < 0 || pos2.x >= _superpositionMap.GetLength(0) || pos2.y < 0 || pos2.y >= _superpositionMap.GetLength(1))
            {
                if (!_wrapAround)
                {
                    continue;
                }
                else
                {
                    pos2 = new Vector2Int((pos2.x + _superpositionMap.GetLength(0)) % _superpositionMap.GetLength(0),
                        (pos2.y + _superpositionMap.GetLength(1)) % _superpositionMap.GetLength(1));
                }
            }

            if (_superpositionMap[pos2.x, pos2.y].Count == 1)
                continue;

            List<Tile> allowedPossibilities = new List<Tile>();

            ////Recalculate what neighbours are possible based on the ones we have on our tile
            //foreach (Tile tile in _superpositionMap[pos.x, pos.y])
            //{
            //    //Find each neighbour of that tile
            //    foreach (Tile neighbour in _tileSet.Tiles[tile.Id].
            //        GetNeighbours(TileScriptable.VectorToNumDirection(direction), (TileScriptable.VectorToNumDirection(direction) + 2) % 4))
            //    {
            //        //Add the possible neighbour if it's not already a possibility
            //        if (!allowedPossibilities.Contains(neighbour))
            //        {
            //            allowedPossibilities.Add(neighbour);
            //        }
            //    }
            //}

            //Approach by recalculation of compatibility
            foreach (Tile tile in _superpositionMap[pos.x, pos.y])
            {
                foreach (Tile possibility in _superpositionMap[pos2.x, pos2.y])
                {
                    if (!allowedPossibilities.Contains(possibility))
                    {
                        if (_tileSet.Tiles[tile.Id].CheckCompatibility(
                            _tileSet.Tiles[possibility.Id], TileScriptable.VectorToNumDirection(direction), tile.Rotation, possibility.Rotation))
                        {
                            allowedPossibilities.Add(possibility);
                        }
                    }
                }
            }

            //    //Now that we calculated the allowed neighbours remove the unallowed ones from the superposition map
            //    bool changed = false;
            //for (int i = _superpositionMap[pos2.x, pos2.y].Count - 1; i >= 0; i--)
            //{
            //    //Check if the current tile is allowed
            //    if (!allowedPossibilities.Contains(_superpositionMap[pos2.x, pos2.y][i]))
            //    {
            //        //if not, we remove it
            //        _superpositionMap[pos2.x, pos2.y].RemoveAt(i);
            //        changed = true;
            //    }
            //}

            //if(changed)
            //{
            //    RecalculatePossibilities(pos2);
            //}

            if (allowedPossibilities.Count != _superpositionMap[pos2.x, pos2.y].Count)
            {
                _superpositionMap[pos2.x, pos2.y] = allowedPossibilities;
                RecalculatePossibilities(pos2);
            }
        } while (direction != Vector2Int.up);
    }

    public int ChooseRandomByWeight(Vector2Int pos)
    {
        float totalWeight = 0.0f;

        foreach(Tile tile in _superpositionMap[pos.x, pos.y])
        {
            totalWeight += _tileSet.Tiles[tile.Id].Weight;
        }

        float randNum = Random.Range(0.0f, totalWeight);

        int choice = 0;
        int i = 0;
        foreach (Tile tile in _superpositionMap[pos.x, pos.y])
        {
            randNum -= _tileSet.Tiles[tile.Id].Weight;

            if (randNum < 0)
            {
                choice = i;
                break;
            }

            i++;
        }

        return choice;
    }

    public void CollapsePossibilities(Vector2Int pos, int chosenPos)
    {
        List<Tile> tiles = new List<Tile>();
        tiles.Add(_superpositionMap[pos.x, pos.y][chosenPos]);

        _superpositionMap[pos.x, pos.y] = tiles;

        RecalculatePossibilities(pos);
    }
}
