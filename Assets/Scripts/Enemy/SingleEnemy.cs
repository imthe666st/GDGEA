namespace Enemy
{
	using Battle;

	using UnityEngine;

	public class SingleEnemy : Enemy
	{
		public override EnemyType Type => EnemyType.Single;
		
		public override void Spawn(Battlefield battlefield)
		{
			if (!battlefield.CanSpawnEnemy())
				return;
			
			var pos = battlefield.GetNewEnemyLocation();

			Instantiate(this, pos, Quaternion.identity, battlefield.transform);
		}
	}
}
