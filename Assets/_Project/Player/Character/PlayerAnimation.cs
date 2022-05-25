namespace ProjectAwakening.Player.Character
{
	/// <summary>
	/// Contains the name of the animations of the player.
	/// Using `static readonly` instead of `const` so that comparisons
	/// are reference based instead of value based.
	/// </summary>
	public static class PlayerAnimation
	{
		// ReSharper disable ConvertToConstant.Global
		public static readonly string IdleUp    = "PlayerIdleUp";
		public static readonly string IdleDown  = "PlayerIdleDown";
		public static readonly string IdleLeft  = "PlayerIdleLeft";
		public static readonly string IdleRight = "PlayerIdleRight";
		
		public static readonly string WalkUp    = "PlayerWalkUp";
		public static readonly string WalkDown  = "PlayerWalkDown";
		public static readonly string WalkLeft  = "PlayerWalkLeft";
		public static readonly string WalkRight = "PlayerWalkRight";
		
		public static readonly string AttackDown  = "AttackDown";
		public static readonly string AttackLeft  = "AttackLeft";
		public static readonly string AttackRight = "AttackRight";
		public static readonly string AttackUp    = "AttackUp";
		
		public static readonly string IdleShieldDown  = "IdleShieldDown";
		public static readonly string IdleShieldLeft  = "IdleShieldLeft";
		public static readonly string IdleShieldRight = "IdleShieldRight";
		public static readonly string IdleShieldUp    = "IdleShieldUp";
		
		public static readonly string WalkShieldDown  = "WalkShieldDown";
		public static readonly string WalkShieldLeft  = "WalkShieldLeft";
		public static readonly string WalkShieldRight = "WalkShieldRight";
		public static readonly string WalkShieldUp    = "WalkShieldUp";
		// ReSharper restore ConvertToConstant.Global
	}
}