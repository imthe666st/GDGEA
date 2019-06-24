namespace Enemy
{
	using Battle;

	public class CompositeEnemy : Enemy
	{
		public Enemy[] Enemies;
		
		public override EnemyType Type => EnemyType.Composite;
		public override void Spawn(Battlefield battlefield)
		{
			foreach (var enemy in this.Enemies)
			{
				enemy.Spawn(battlefield);
			}
		}
	}
}
