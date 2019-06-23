namespace Enemy {
	using Battle;

	using UnityEngine;

	public class ColorSlime : SingleEnemy
	{
		public Color[] RandomColors;
		
		public override void Spawn(Battlefield battlefield)
		{
			if (!battlefield.CanSpawnEnemy())
				return;
			
			var pos = battlefield.GetNewEnemyLocation();

			var me = Instantiate(this, pos, Quaternion.identity, battlefield.transform);
			me.GetComponent<SpriteRenderer>().color = this.RandomColors[Random.Range(0, this.RandomColors.Length)];
		}
	}
}
