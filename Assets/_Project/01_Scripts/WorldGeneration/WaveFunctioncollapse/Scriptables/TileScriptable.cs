using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;


[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/Tile", order = 0)]
public class TileScriptable : ScriptableObject
{
    //Img to print
    [SerializeField] RuleTile _ruleTile;

    [Header("Rules")]
    [Header("Face's Code")]
    //id for each face (used for determining which tile goes next to which)
    [SerializeField] private int _upCode = -1;
    [SerializeField] private int _rightCode = -1;
    [SerializeField] private int _downCode = -1;
    [SerializeField] private int _leftCode = -1;

    [Header("Accepted codes for neighbours")]
    [SerializeField] private List<int> _goalUpCodes = new List<int>() { 0 };
    [SerializeField] private List<int> _goalRightCodes = new List<int>() { 0 };
    [SerializeField] private List<int> _goalDownCodes = new List<int>() { 0 };
    [SerializeField] private List<int> _goalLeftCodes = new List<int>() { 0 };

    [Header("Other Rules")]

    [Tooltip("Wether this tile can or should be rotated")]
    [SerializeField] bool _canRotate = true;

    [Tooltip("How much this tile should be prioritized relative to the others when there are multiple possibilities")]
    [SerializeField] float _weight = 1;

    //id of neighbours in each direction
    private List<TileWFC> _upNeighbours = new List<TileWFC>();
    private List<TileWFC> _downNeighbours = new List<TileWFC>();
    private List<TileWFC> _leftNeighbours = new List<TileWFC>();
    private List<TileWFC> _rightNeighbours = new List<TileWFC>();

    //Getters
    public RuleTile Tile { get { return _ruleTile; } }
    public int UpCode { get { return _upCode; } }
    public int DownCode { get { return _downCode; } }
    public int LeftCode { get { return _leftCode; } }
    public int RightCode { get { return _rightCode; } }

    public List<TileWFC> UpNeighbours { get => _upNeighbours; set { _upNeighbours = value; }}
    public List<TileWFC> DownNeighbours { get => _downNeighbours; set { _downNeighbours = value; }}
    public List<TileWFC> LeftNeighbours { get => _leftNeighbours; set { _leftNeighbours = value; }}
    public List<TileWFC> RightNeighbours { get => _rightNeighbours; set { _rightNeighbours = value; }}

    public bool CanRotate { get => _canRotate; }

    public List<int> GoalUpCodes => _goalUpCodes;
    public List<int> GoalDownCodes => _goalDownCodes;
    public List<int> GoalLeftCodes => _goalLeftCodes;
    public List<int> GoalRightCodes => _goalRightCodes;

    public float Weight { get => _weight; }

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
        if (rotation1 != 0 && !_canRotate || rotation2 != 0 && !tile.CanRotate)
            return false;

        int reverseDirection = (direction + 2) % 4;

        if (GetCode(direction, rotation1) < 0)
            return false;

        return GetGoalCodes(direction, rotation1).Contains(tile.GetCode(reverseDirection, rotation2)) &&
            tile.GetGoalCodes(reverseDirection, rotation2).Contains(GetCode(direction, rotation1));
    }

    public void ResetNeighbours()
    {
        _upNeighbours = new List<TileWFC>();
        _downNeighbours = new List<TileWFC>();
        _leftNeighbours = new List<TileWFC>();
        _rightNeighbours = new List<TileWFC>();
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
                _upNeighbours = value;
                return;
            case 2:
                _downNeighbours = value;
                return;
            case 3:
                _leftNeighbours = value;
                return;
            case 1:
                _rightNeighbours = value;
                return;
            default:
                Debug.Log("incorrect direction : " + direction.ToString());
                return;
        }
    }

    public static int VectorToNumDirection(Vector2Int directionV)
    {
        return Mathf.Abs(((1 - directionV.y) * directionV.y) + ((2 - directionV.x) * directionV.x));
    }

    public static Vector2Int NumDirectionToVector(int directionN)
    {
        switch(directionN)
        { case 0: return Vector2Int.up;
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
                return _upNeighbours;
            case 1:
                return _rightNeighbours;
            case 2:
                return _downNeighbours;
            case 3:
                return _leftNeighbours;
            default:
                Debug.LogError("incorrect direction : " + direction);
                return _upNeighbours;
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
                list = _upNeighbours;
                break;
            case 2:
                list = _downNeighbours;
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
                return _upCode;
            case 2:
                return _downCode;
            case 3:
                return _leftCode;
            case 1:
                return _rightCode;
            default:
                Debug.Log("incorrect direction : " + direction.ToString());
                return -1;
        }
    }

    public List<int> GetGoalCodes(int direction, int rotation)
    {
        direction = (direction - rotation + 4) % 4;

        switch(direction)
        {
            case 0:
                return _goalUpCodes;
            case 1:
                return _goalRightCodes;
            case 2:
                return _goalDownCodes;
            case 3:
                return _goalLeftCodes;
            default:
                Debug.Log("incorrect direction : " + direction.ToString());
                return _goalUpCodes;
        }
    }
}
