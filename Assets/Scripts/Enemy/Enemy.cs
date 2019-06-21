using UnityEngine;

namespace Enemy {
	using System;

	using Battle;

	public abstract class Enemy : MonoBehaviour
	{
		public abstract EnemyType Type { get; }

		public abstract void Spawn(Battlefield battlefield);
		
		public virtual void DoTurn()
		{
			
		}

		protected virtual void Awake()
		{
			this.transform.parent.GetComponent<Battlefield>().RegisterEnemy(this);
		}
	}
}
