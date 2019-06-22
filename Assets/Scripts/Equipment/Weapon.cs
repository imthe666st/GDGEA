namespace Equipment
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "Weapon")]
	public class Weapon : ScriptableObject
	{
		[SerializeField]
		protected int damage;
		public int Damage => this.damage;

		[SerializeField]
		protected int minDistance;
		public int MinDistance => this.minDistance;

		[SerializeField]
		protected int maxDistance;
		public int MaxDistance => this.maxDistance;

		[SerializeField]
		protected int movement;
		public int Movement => this.movement;
	}
}
