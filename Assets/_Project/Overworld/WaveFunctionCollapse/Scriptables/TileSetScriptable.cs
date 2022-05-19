using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAwakening.WorldGeneration
{
    [CreateAssetMenu(fileName = "TileSet", menuName = "ScriptableObjects/TileSets", order = 1)]
    [ExecuteInEditMode]
    public class TileSetScriptable : ScriptableObject
    {
        [SerializeField] List<TileScriptable> tiles = new List<TileScriptable>();
        public List<TileScriptable> Tiles { get { return tiles; } }

        public void CalculateNeighbours()
        {
            //Reset all neighbours
            foreach (TileScriptable tile in tiles)
                tile.ResetNeighbours();

            //Check each tile
            for (int tileIndex = 0; tileIndex < tiles.Count; tileIndex++)
            {
                //Check each direction once (0 is up and turning clockwise)
                for (int direction = 0; direction < 4; direction++)
                {
                    //Check each possible neighbour
                    for (int neighbourIndex = 0; neighbourIndex < tiles.Count; neighbourIndex++)
                    {
                        for (int rotation1 = 0; rotation1 < 4; rotation1++)
                        {
                            for (int rotation2 = 0; rotation2 < 4; rotation2++)
                            {
                                //Check compatibility
                                if (tiles[tileIndex].CheckCompatibility(tiles[neighbourIndex], direction, 0, rotation2))
                                {
                                    //add the neighbour
                                    tiles[tileIndex].AddNeighbour(new TileWFC(neighbourIndex, rotation2), direction);
                                }

                                //Stop checking rotations if we can't rotate
                                if (!tiles[neighbourIndex].CanRotate)
                                    break;
                            }

                            if (!tiles[tileIndex].CanRotate)
                                break;
                        }
                    }
                }
            }
        }

        public void PrintTiles()
        {
            foreach (TileScriptable tile in tiles)
            {
                string up = "";
                string down = "";
                string left = "";
                string right = "";

                foreach (var n in tile.UpNeighbours)
                    up += n.Id + "," + n.Rotation + "|";
                foreach (var n in tile.DownNeighbours)
                    down += n.Id + "," + n.Rotation + "|";
                foreach (var n in tile.LeftNeighbours)
                    left += n.Id + "," + n.Rotation + "|";
                foreach (var n in tile.RightNeighbours)
                    right += n.Id + "," + n.Rotation + "|";

                Debug.Log(tile.name + " : \n" +
                    "Up : " + up + "\n" +
                    "Down : " + down + "\n" +
                    "Left : " + left + "\n" +
                    "Right : " + right);
            }
        }
    }
}