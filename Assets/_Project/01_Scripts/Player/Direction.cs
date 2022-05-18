using System;
using UnityEngine;

namespace ProjectAwakening.Player
{
	public enum Direction
	{
		Up,
		Down,
		Left,
		Right,
	}

	public static class DirectionUtils
	{
		public static Vector2Int GetDirectionVector(Direction direction)
		{
			return direction switch
			{
				Direction.Up => new Vector2Int(0, -1),
				Direction.Down => new Vector2Int(0, 1),
				Direction.Left => Vector2Int.left,
				Direction.Right => Vector2Int.right,
				_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
			};
		}
	}
}