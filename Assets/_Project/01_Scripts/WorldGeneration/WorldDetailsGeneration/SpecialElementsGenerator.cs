using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectAwakening.WorldGeneration
{
    public class SpecialElementsGenerator : MonoBehaviour
    {
        [SerializeField]
        WaveCollapseMapMaker mapMaker;
        
        bool[,] map;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        public void TestFloodFill()
        {
            map = WorldMapUtilities.superPositionMapToArray(mapMaker.SuperpositionsMap, mapMaker.TileSet);
            WorldMapUtilities.GetLargestArea(map, true);
        }
    }
}
