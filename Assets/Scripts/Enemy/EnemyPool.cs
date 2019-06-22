namespace Enemy {
	using UnityEngine;

	[CreateAssetMenu(menuName = "EnemyPool")]
	public class EnemyPool : ScriptableObject
	{
		public EnemyData[] pool;

		public Enemy GetRandom()
		{
			Debug.Assert(this.pool != null && this.pool.Length != 0, this);
			
			var select = Random.Range(0, this.pool.Length);

			return this.pool[select].data;
		}
	}

	[System.Serializable]
	public class EnemyData
	{
		public global::Enemy.Enemy data;
	}
}