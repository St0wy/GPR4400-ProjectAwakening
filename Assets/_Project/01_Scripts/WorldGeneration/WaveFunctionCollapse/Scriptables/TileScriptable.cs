using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

namespace ProjectAwakening.WorldGeneration
{
    [CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/Tile", order = 0)]
    public class TileScriptable : ScriptableObject
    {
        [Header("Printing")]
        [Tooltip("Wether to use the rule tile or the 'normal' tile")]
        [SerializeField] private bool useRuleTile = false;
        [Tooltip("Tile to use")]
        [SerializeField] private Tile tile;
        [Tooltip("Rule tile to use")]
        [SerializeField] private RuleTile ruleTile;

        [Header("Rules")]
        [Header("Face's Code")]
        //id for each face (used for determining which tile goes next to which)
        [SerializeField] private int upCode = -1;
        [SerializeField] private int rightCode = -1;
        [SerializeField] private int downCode = -1;
        [SerializeField] private int leftCode = -1;

        [Header("Accepted codes for neighbours")]
        [SerializeField] private List<int> goalUpCodes = new List<int>() { 0 };
        [SerializeField] private List<int> goalRightCodes = new List<int>() { 0 };
        [SerializeField] private List<int> goalDownCodes = new List<int>() { 0 };
        [SerializeField] private List<int> goalLeftCodes = new List<int>() { 0 };

        [Header("Other Rules")]

        [Tooltip("Wether this tile can or should be rotated")]
        [SerializeField] private bool canRotate = true;

        [Tooltip("How much this tile should be prioritized relative to the others when there are multiple possibilities")]
        [SerializeField] private float weight = 1;

        //id of neighbours in each direction
        private List<TileWFC> upNeighbours = new List<TileWFC>();
        private List<TileWFC> downNeighbours = new List<TileWFC>();
        private List<TileWFC> leftNeighbours = new List<TileWFC>();
        private List<TileWFC> rightNeighbours = new List<TileWFC>();

        //Getters
        public bool UseRuleTile { get { return useRuleTile; } }
        public Tile Tile { get { return tile; } }
        public RuleTile RuleTile { get => ruleTile; }

        public int UpCode { get { return upCode; } }
        public int DownCode { get { return downCode; } }
        public int LeftCode { get { return leftCode; } }
        public int RightCode { get { return rightCode; } }

        public List<TileWFC> UpNeighbours { get => upNeighbours; set { upNeighbours = value; } }
        public List<TileWFC> DownNeighbours { get => downNeighbours; set { downNeighbours = value; } }
        public List<TileWFC> LeftNeighbours { get => leftNeighbours; set { leftNeighbours = value; } }
        public List<TileWFC> RightNeighbours { get => rightNeighbours; set { rightNeighbours = value; } }

        public bool CanRotate { get => canRotate; }

        public List<int> GoalUpCodes => goalUpCodes;
        public List<int> GoalDownCodes => goalDownCodes;
        public List<int> GoalLeftCodes => goalLeftCodes;
        public List<int> GoalRightCodes => goalRightCodes;

        public float Weight { get => weight; }

        /// <summary>
        /// Check the compatibility of this tile whith another
        /// </summary>
        /// <param name="tile">The second tile to check compatibility with</param>
        /// <param name="direction">Starting at 0 for up and turning clockwise, the direction in which the second tile is relative to the first</param>
        /// <param name="rotation1">our rotation</param>
        /// <param name="rotation2">the rotation of the second tile</param>
        /// <returns>Returns if the two tiles are compatible</returns>
        public bool CheckCompatibility(TileScriptable tile, int direction, int rotation1, int rotation2)
        {
            if (rotation1 != 0 && !canRotate || rotation2 != 0 && !tile.CanRotate)
                return false;

            int reverseDirection = (direction + 2) % 4;

            if (GetCode(direction, rotation1) < 0)
                return false;

            return GetGoalCodes(direction, rotation1).Contains(tile.GetCode(reverseDirection, rotation2)) &&
                tile.GetGoalCodes(reverseDirection, rotation2).Contains(GetCode(direction, rotation1));
        }

        public void ResetNeighbours()
        {
            upNeighbours = new List<TileWFC>();
            downNeighbours = new List<TileWFC>();
            leftNeighbours = new List<TileWFC>();
            rightNeighbours = new List<TileWFC>();
        }

        /// <summary>
        /// Set the list of neighbours to the provided one
        /// </summary>
        /// <param name="value">The list to set</param>
        /// <param name="direction">Starting at 0 for up and turning clockwise, the direction in which the neighbours are</param>
        public void SetNeighbours(List<TileWFC> value, int direction)
        {
            switch (direction)
            {
                case 0:
                    upNeighbours = value;
                    return;
                case 2:
                    downNeighbours = value;
                    return;
                case 3:
                    leftNeighbours = value;
                    return;
                case 1:
                    rightNeighbours = value;
                    return;
                default:
                    Debug.Log("incorrect direction : " + direction.ToString());
                    return;
            }
        }

        public static int VectorToNumDirection(Vector2Int directionV)
        {
            return Mathf.Abs((1 - directionV.y) * directionV.y + (2 - directionV.x) * directionV.x);
        }

        public static Vector2Int NumDirectionToVector(int directionN)
        {
            switch (directionN)
            {
                case 0: return Vector2Int.up;
                case 1: return Vector2Int.right;
                case 2: return Vector2Int.down;
                case 3: return Vector2Int.left;
                default: return Vector2Int.zero;
            }
        }

        public List<TileWFC> GetNeighbours(int direction, int rotation)
        {
            direction = (direction - rotation + 4) % 4;

            switch (direction)
            {
                case 0:
                    return upNeighbours;
                case 1:
                    return rightNeighbours;
                case 2:
                    return downNeighbours;
                case 3:
                    return leftNeighbours;
                default:
                    Debug.LogError("incorrect direction : " + direction);
                    return upNeighbours;
            }
        }

        /// <summary>
        /// Add an element to the list of neighbours
        /// </summary>
        /// <param name="value">The element to add</param>
        /// <param name="direction">Starting at 0 for up and turning clockwise, the direction in which the neighbours are</param>
        public void AddNeighbour(TileWFC value, int direction)
        {
            List<TileWFC> list;

            switch (direction)
            {
                case 0:
                    list = upNeighbours;
                    break;
                case 2:
                    list = downNeighbours;
                    break;
                case 3:
                    list = LeftNeighbours;
                    break;
                case 1:
                    list = RightNeighbours;
                    break;
                default:
                    Debug.Log("incorrect direction : " + direction.ToString());
                    return;
            }

            if (!list.Contains(value))
            {
                list.Add(value);
            }
        }

        /// <summary>
        /// Get the code of a face in a given direction
        /// </summary>
        /// <param name="direction">Starting at 0 for up and turning clockwise, the direction of the code</param>
        /// <returns>The code</returns>
        public int GetCode(int direction, int rotation)
        {
            direction = (direction - rotation + 4) % 4;

            switch (direction)
            {
                case 0:
                    return upCode;
                case 2:
                    return downCode;
                case 3:
                    return leftCode;
                case 1:
                    return rightCode;
                default:
                    Debug.Log("incorrect direction : " + direction.ToString());
                    return -1;
            }
        }

        public List<int> GetGoalCodes(int direction, int rotation)
        {
            direction = (direction - rotation + 4) % 4;

            switch (direction)
            {
                case 0:
                    return goalUpCodes;
                case 1:
                    return goalRightCodes;
                case 2:
                    return goalDownCodes;
                case 3:
                    return goalLeftCodes;
                default:
                    Debug.Log("incorrect direction : " + direction.ToString());
                    return goalUpCodes;
            }
        }
    }
}