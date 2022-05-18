using System;
using UnityEngine;

namespace ProjectAwakening.DungeonGeneration
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
	}
}