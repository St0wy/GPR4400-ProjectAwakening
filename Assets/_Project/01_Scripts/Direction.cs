using System;
using UnityEngine;

namespace ProjectAwakening
{
	public enum Direction
	{
		Up = 0,
		Right = 1,
		Down = 2,
		Left = 3,
	}

	public static class DirectionUtils
	{
		public static Direction VectorToEnumDirection(Vector2 vector)
		{
			Vector2 vectorToConvert = vector.normalized;

			if (Math.Abs(vectorToConvert.x) > Mathf.Abs(vectorToConvert.y))
			{
				if (vectorToConvert.x > 0)
					return Direction.Right;
				else
					return Direction.Left;
			}
			else
			{
				if (vectorToConvert.y > 0)
					return Direction.Up;
				else
					return Direction.Down;
			}
		}

		public static Vector2Int GetDirectionVector(Direction direction)
		{
			return direction switch
			{
				Direction.Up => new Vector2Int(0, -1),
				Direction.Down => new Vector2Int(0, 1),
				Direction.Left => Vector2Int.left,
				Direction.Right => Vector2Int.right,
				_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
			};
		}

		public static float GetAngle(Direction direction)
		{
			return direction switch
			{
				Direction.Up => 0,
				Direction.Down => 180,
				Direction.Left => 90,
				Direction.Right => 270,
				_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
			};
		}
	}
}