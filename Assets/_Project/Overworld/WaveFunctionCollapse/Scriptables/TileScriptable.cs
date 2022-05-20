using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
// ReSharper disable ConvertToAutoPropertyWhenPossible

namespace ProjectAwakening.Overworld.WaveFunctionCollapse
{
	[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/Tile", order = 0)]
	public class TileScriptable : ScriptableObject
	{
		[Header("Printing")]
		[Tooltip("Whether to use the rule tile or the 'normal' tile")]
		[SerializeField] private bool useRuleTile;
		[Tooltip("Tile to use")]
		[SerializeField] private Tile tile;
		[Tooltip("Rule tile to use")]
		[SerializeField] private RuleTile ruleTile;

		[Header("Rules")]
		[Header("Face's Code")]
		// id for each face (used for determining which tile goes next to which)
		[SerializeField] private int upCode = -1;
		[SerializeField] private int rightCode = -1;
		[SerializeField] private int downCode = -1;
		[SerializeField] private int leftCode = -1;

		[Header("Accepted codes for neighbours")]
		[SerializeField] private List<int> goalUpCodes = new() {0};
		[SerializeField] private List<int> goalRightCodes = new() {0};
		[SerializeField] private List<int> goalDownCodes = new() {0};
		[SerializeField] private List<int> goalLeftCodes = new() {0};

		[Header("Other Rules")]
		[Tooltip("Whether this tile can or should be rotated")]
		[SerializeField] private bool canRotate = true;

		[Tooltip(
			"How much this tile should be prioritized relative to the others when there are multiple possibilities")]
		[SerializeField] private float weight = 1;

		// id of neighbours in each direction
		private List<TileWfc> upNeighbours = new();
		private List<TileWfc> downNeighbours = new();
		private List<TileWfc> leftNeighbours = new();
		private List<TileWfc> rightNeighbours = new();

		public bool UseRuleTile => useRuleTile;
		public Tile Tile => tile;
		public RuleTile RuleTile => ruleTile;

		public int UpCode => upCode;
		public int DownCode => downCode;
		public int LeftCode => leftCode;
		public int RightCode => rightCode;

		public List<TileWfc> UpNeighbours { get => upNeighbours; set => upNeighbours = value; }
		public List<TileWfc> DownNeighbours { get => downNeighbours; set => downNeighbours = value; }
		public List<TileWfc> LeftNeighbours { get => leftNeighbours; set => leftNeighbours = value; }
		public List<TileWfc> RightNeighbours { get => rightNeighbours; set => rightNeighbours = value; }

		public bool CanRotate => canRotate;

		public List<int> GoalUpCodes => goalUpCodes;
		public List<int> GoalDownCodes => goalDownCodes;
		public List<int> GoalLeftCodes => goalLeftCodes;
		public List<int> GoalRightCodes => goalRightCodes;

		public float Weight => weight;

		/// <summary>
		/// Check the compatibility of this tile with another
		/// </summary>
		/// <param name="otherTile">The second tile to check compatibility with</param>
		/// <param name="direction">Starting at 0 for up and turning clockwise, the direction in which the second tile is relative to the first</param>
		/// <param name="rotation1">our rotation</param>
		/// <param name="rotation2">the rotation of the second tile</param>
		/// <returns>Returns if the two tiles are compatible</returns>
		public bool CheckCompatibility(TileScriptable otherTile, int direction, int rotation1, int rotation2)
		{
			if (rotation1 != 0 && !canRotate || rotation2 != 0 && !otherTile.CanRotate)
				return false;

			int reverseDirection = (direction + 2) % 4;

			if (GetCode(direction, rotation1) < 0)
				return false;

			return GetGoalCodes(direction, rotation1).Contains(otherTile.GetCode(reverseDirection, rotation2)) &&
				otherTile.GetGoalCodes(reverseDirection, rotation2).Contains(GetCode(direction, rotation1));
		}

		public void ResetNeighbours()
		{
			upNeighbours = new List<TileWfc>();
			downNeighbours = new List<TileWfc>();
			leftNeighbours = new List<TileWfc>();
			rightNeighbours = new List<TileWfc>();
		}

		/// <summary>
		/// Set the list of neighbours to the provided one
		/// </summary>
		/// <param name="value">The list to set</param>
		/// <param name="direction">Starting at 0 for up and turning clockwise, the direction in which the neighbours are</param>
		public void SetNeighbours(List<TileWfc> value, int direction)
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
			return directionN switch
			{
				0 => Vector2Int.up,
				1 => Vector2Int.right,
				2 => Vector2Int.down,
				3 => Vector2Int.left,
				_ => Vector2Int.zero
			};
		}

		public List<TileWfc> GetNeighbours(int direction, int rotation)
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
		public void AddNeighbour(TileWfc value, int direction)
		{
			List<TileWfc> list;

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
		/// Gets the code of a face in a given direction.
		/// </summary>
		/// <param name="direction">Direction of the code starting at 0 for up and turning clockwise.</param>
		/// <param name="rotation">Rotation of the code.</param>
		/// <returns>The code in the specified direction and rotation.</returns>
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