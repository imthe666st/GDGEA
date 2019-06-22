using UnityEngine;

namespace Enemy {
	using Battle;

	public abstract class Enemy : MonoBehaviour
	{
		public abstract EnemyType Type { get; }

		[SerializeField]
		protected int damage = 1;
		public int Damage => this.damage;

		[SerializeField]
		protected int minDistance = 0;
		public int MinDistance => this.minDistance;

		[SerializeField]
		protected int maxDistance = 1;
		public int MaxDistance => this.maxDistance;

		[SerializeField]
		protected int movement = 2;
		public int Movement => this.movement;
		
		[SerializeField] 
		protected int health = 2;
		public int Health => this.health;

		[SerializeField]
		protected float critDamage = 0;
		public float CritDamage => this.critDamage;

		[SerializeField, Range(-1, 1)] 
		protected float critChance = 0;
		public float CritChance => this.critChance;

		
		public abstract void Spawn(Battlefield battlefield);
		
		public virtual void DoTurn()
		{
			
		}

		protected virtual void Awake()
		{
			this.transform.parent.GetComponent<Battlefield>().RegisterEnemy(this);
		}

		public void TakeDamage(int damage)
		{
			this.health -= damage;

			if (damage <= 0)
			{
				//DEAD
			}
		}
	}
}
