namespace Battle
{
	using System;

	[Flags]
	public enum TileStatus
	{
		Normal = 0,
		Walkable = 1 << 1,
		PlayerAttackable = 1 << 2,
		EnemyAttackable = 1 << 3,
		WalkableEnemyAttackable = Walkable | EnemyAttackable,
		PlayerAttackableEnemyAttackAble = PlayerAttackable | EnemyAttackable,
	}
}
