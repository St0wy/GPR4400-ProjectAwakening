using System;
using UnityEngine;

namespace ProjectAwakening.DungeonGeneration.Rooms
{
	[Serializable]
	public struct Neighborhood
	{
		[field: SerializeField] public bool Top { get; set; }
		[field: SerializeField] public bool Bottom { get; set; }
		[field: SerializeField] public bool Left { get; set; }
		[field: SerializeField] public bool Right { get; set; }

		public int Count => (Top ? 1 : 0) + (Bottom ? 1 : 0) +
			(Left ? 1 : 0) + (Right ? 1 : 0);

		public NeighborhoodType Type
		{
			get
			{
				return Count switch
				{
					// ReSharper disable ConvertIfStatementToSwitchStatement
					1 when Top => NeighborhoodType.Top,
					1 when Bottom => NeighborhoodType.Bottom,
					1 when Right => NeighborhoodType.Right,
					1 when Left => NeighborhoodType.Left,
					2 when Top && Left => NeighborhoodType.TopLeft,
					2 when Top && Bottom => NeighborhoodType.TopBottom,
					2 when Top && Right => NeighborhoodType.TopRight,
					2 when Left && Bottom => NeighborhoodType.BottomLeft,
					2 when Left && Right => NeighborhoodType.LeftRight,
					2 when Bottom && Right => NeighborhoodType.BottomRight,
					3 when Top && Left && Right => NeighborhoodType.TopLeftRight,
					3 when Top && Left && Bottom => NeighborhoodType.TopBottomLeft,
					3 when Top && Right && Bottom => NeighborhoodType.TopBottomRight,
					3 when Left && Bottom && Right => NeighborhoodType.BottomLeftRight,
					4 => NeighborhoodType.TopBottomLeftRight,
					0 => NeighborhoodType.None,
					_ => throw new ArgumentOutOfRangeException(),
				};
			}
		}
	}
}