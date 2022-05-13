namespace ProjectAwakening.Player
{
	/// <summary>
	/// Contains the name of the animations of the player.
	/// Using `static readonly` instead of `const` so that comparisons
	/// are reference based instead of value based.
	/// </summary>
	public static class PlayerAnimation
	{
		public static readonly string IdleUp = "PlayerIdleUp";
		public static readonly string IdleDown = "PlayerIdleDown";
		public static readonly string IdleLeft = "PlayerIdleLeft";
		public static readonly string IdleRight = "PlayerIdleRight";
		public static readonly string WalkUp = "PlayerWalkUp";
		public static readonly string WalkDown = "PlayerWalkDown";
		public static readonly string WalkLeft = "PlayerWalkLeft";
		public static readonly string WalkRight = "PlayerWalkRight";
	}
}