using UnityEngine;

namespace Enemy {
	using System.Linq;
	using System.Runtime.CompilerServices;

	using Battle;

	using DG.Tweening;

	using Marker;

	using Player;

	using UnityEngine.SceneManagement;

	public abstract class Enemy : MonoBehaviour
	{
		public abstract EnemyType Type { get; }

		[HideInInspector]
		public EnemyHealthBarMarker HealthBarMarker;

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

		private int variHealth;
		
		[SerializeField]
		protected float critDamage = 0;
		public float CritDamage => this.critDamage;

		[SerializeField, Range(-1, 1)] 
		protected float critChance = 0;
		public float CritChance => this.critChance;

		public float MoveTime = 0.3f;
		public float AttackTime = 0.2f;
		
		[HideInInspector]
		public FieldTile PositionTile;
		
		public abstract void Spawn(Battlefield battlefield);
		
		protected virtual void Awake()
		{
			this.variHealth = this.Health;
			this.transform.parent.GetComponent<Battlefield>().RegisterEnemy(this);
			this.PositionTile = GameManager.Instance.Battlefield.Tiles.First(t => (t.transform.position.ClearZ() -
																				   this.transform.position.ClearZ())
																				  .magnitude <= 0.1f);
			this.PositionTile.HasEnemy = true;
		}

		public void TakeDamage(int damage)
		{
			this.variHealth -= damage;

			var di = Instantiate(GameManager.Instance.Battlefield.DamageIndicatorPrefab, GameManager.Instance
																									.Battlefield
																									.transform
								 );
			di.transform.localPosition = this.transform.localPosition;
			di.SetValue(damage.ToString());
			
			this.HealthBarMarker.Change((float)this.variHealth/ this.health);

			if (this.variHealth <= 0)
			{
				GameManager.Instance.Battlefield.Enemies.Remove(this);
				this.gameObject.SetActive(false);
				Destroy(this.gameObject);
			}
		}

		public virtual bool Move()
		{
			if (this.variHealth <= 0)
				return false;

			var playerPos = GameManager.Instance.BattlePlayer.transform.position;

			
			var distance = Mathf.Abs(playerPos.x - this.transform.position.x) + Mathf.Abs(playerPos.y - this
																										.transform
																										.position.y);
			
			if (distance < this.MaxDistance)
				return false;

			this.CreateMovement(GameManager.Instance.BattlePlayer.PositionTile);

			return true;
		}

		private void CreateMovement(FieldTile target)
		{
			var tempPos = this.PositionTile;
			tempPos.HasEnemy = false;
			
			var mv = DOTween.Sequence();
			mv.PrependInterval(0.1f);

			var targetPos = target.transform.position;
			var distance = Mathf.Abs(targetPos.x - this.transform.position.x) + Mathf.Abs(targetPos.y - this
																										.transform
																										.position.y);

			
			var distanceWalkable = this.movement;
			
			while (distanceWalkable > 0 && distance > this.MaxDistance + 1)
			{
				var toMove = target.transform.position.ClearZ() - tempPos.transform.position.ClearZ();

				//if (Mathf.Abs(toMove.x) >= Mathf.Abs(toMove.y))
				if (Random.Range(0f, 1f) <= 0.5f) 
				{
					if (Mathf.Abs(toMove.x) > float.Epsilon)
					{
						tempPos = this.CreateXMove(mv, toMove.x, tempPos);
					}
				}
				else
				{
					if (Mathf.Abs(toMove.y) > float.Epsilon)
					{
						tempPos = this.CreateYMove(mv, toMove.y, tempPos);
					}
				}
				
				distanceWalkable--;
			}

			mv.OnComplete(() => { GameManager.Instance.Battlefield.EnemyCanMove = true; });
			this.PositionTile = tempPos;
			tempPos.HasEnemy = true;
		}

		private FieldTile CreateYMove(Sequence sq, float yDiff, FieldTile tile)
		{
			var targetTile = yDiff < 0 ? tile.DownNeighbor : tile.UpNeighbor;

			if (targetTile.HasEnemy)
				return tile;

			sq.Append(this.transform.DOMove(targetTile.transform.position.ClearZ() + this.transform.position.OnlyZ(),
											this.MoveTime));
			return targetTile;
		}

		private FieldTile CreateXMove(Sequence sq, float xDiff, FieldTile tile)
		{
			var targetTile = xDiff < 0 ? tile.LeftNeighbor : tile.RightNeighbor;

			if (targetTile.HasEnemy)
				return tile;
			
			sq.Append(this.transform.DOMove(targetTile.transform.position.ClearZ() + this.transform.position.OnlyZ(),
											this.MoveTime));
			return targetTile;
		}

		public virtual bool Attack()
		{
			var playerPos = GameManager.Instance.BattlePlayer.transform.position;
			var pos = this.transform.position;

			var distance = Mathf.Abs(playerPos.x - pos.x) + Mathf.Abs(playerPos.y - pos.y);

			if (distance <= this.MinDistance || distance > this.MaxDistance)
			{
				return false;
			}

			var dir = playerPos - this.transform.position;
			dir.Normalize();

			var mv = DOTween.Sequence();

			mv.Append(this.transform.DOMove(pos + 0.5f * dir, this.AttackTime))
			  .Append(this.transform.DOMove(pos, this.AttackTime));

			
			var player = GameManager.Instance.BattlePlayer;
			var playerKB = DOTween.Sequence();
			playerKB.AppendInterval(this.AttackTime)
					.AppendCallback(() => this.DealDamage(player))
					.Append(player.transform.DOMove(playerPos + 0.25f * dir, this.AttackTime))
					.Append(player.transform.DOMove(playerPos, this.AttackTime))
					.OnComplete(() => { GameManager.Instance.Battlefield.EnemyCanAttack = true; });
			
			return true;
		}

		public void DealDamage(BattlePlayer player)
		{
			var damage = this.Damage;

			if (Random.Range(0, 1f) < this.CritChance) damage = (int) (this.Damage * this.CritDamage);

			player.TakeDamage(damage);
		}
	}
}
