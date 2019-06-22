namespace Equipment
{
	using System;
	using System.Text;

	using UnityEngine;

	[CreateAssetMenu(fileName = "Modifier")]
	public class Modifier : ScriptableObject
	{
		[SerializeField]
		protected int minDistance = 0;
		public int MinDistance => this.minDistance;

		[SerializeField]
		protected int maxDistance = 0;
		public int MaxDistance => this.maxDistance;

		[SerializeField]
		protected int damage = 0;
		public int Damage => this.damage;

		[SerializeField]
		protected int movement = 0;
		public int Movement => this.movement;

		[SerializeField] 
		protected int health = 0;
		public int Health => this.health;

		[SerializeField]
		protected float critDamage = 0;
		public float CritDamage => this.critDamage;

		[SerializeField, Range(-1, 1)] 
		protected float critChance = 0;
		public float CritChance => this.critChance;

		public string GetDescription()
		{
			var sb = new StringBuilder();

			if (this.damage != 0)
				sb.AppendLine($"Damage: {(this.damage > 0 ? "+" : "")}{this.damage}");
			if (this.minDistance != 0) 
				sb.AppendLine($"Minimal attack distance: {(this.minDistance > 0 ? "+" : "")}{this.minDistance}");
			if (this.maxDistance != 0)
				sb.AppendLine($"Maximal attack distance: {(this.maxDistance > 0 ? "+" : "")}{this.maxDistance}");
			if (this.movement != 0)
				sb.AppendLine($"Movement: {(this.movement > 0 ? "+" : "")}{this.movement}");
			if (this.health != 0)
				sb.AppendLine($"Health: {(this.health > 0 ? "+" : "")}{this.health}");
			if (Math.Abs(this.critDamage) > float.Epsilon)
				sb.AppendLine($"Critical Damage: {(this.critDamage > 0 ? "+" : "")}{this.critDamage:F2}x");
			if (Math.Abs(this.critChance) > float.Epsilon)
				sb.AppendLine($"Critical Chance: {(this.critChance > 0 ? "+" : "")}{this.critChance:P2}");

			return sb.ToString();
		}
	}
}
