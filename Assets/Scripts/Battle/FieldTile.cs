using System;

using UnityEngine;

namespace Battle {
	using System.Collections.Generic;

	public class FieldTile : MonoBehaviour
	{
		public FieldTile UpNeighbor;
		public FieldTile DownNeighbor;
		public FieldTile RightNeighbor;
		public FieldTile LeftNeighbor;

		public Sprite Normal;
		public Sprite Walkable;
		public Sprite PlayerAttackable;
		public Sprite EnemyAttackable;
		public Sprite WalkableEnemyAttackable;
		public Sprite PlayerAttackableEnemyAttackable;

		public TileStatus  TileStatus
		{
			get => this._tileStatus;
			set
			{
				this._tileStatus = value; 
				this.TileStatusChanged();
			}
		}
		
		private TileStatus _tileStatus;
		protected SpriteRenderer SpriteRenderer;

		private void Awake()
		{
			this.SpriteRenderer = this.GetComponent<SpriteRenderer>();
		}

		public void SetNeighbor(Direction direction, FieldTile tile)
		{
			switch (direction)
			{
				case Direction.None:
					break;
			
				case Direction.Up:
					this.UpNeighbor = tile;

					if (tile.DownNeighbor == null)
						tile.SetNeighbor(Direction.Down, this);
					break;
			
				case Direction.Down:
					this.DownNeighbor = tile;
				
					if (tile.UpNeighbor == null)
						tile.SetNeighbor(Direction.Up, this);
					break;
			
				case Direction.Left:
					this.LeftNeighbor = tile;

					if (tile.RightNeighbor == null)
						tile.SetNeighbor(Direction.Right, this);
					break;
			
				case Direction.Right:
					this.RightNeighbor = tile;
				
					if (tile.LeftNeighbor == null)
						tile.SetNeighbor(Direction.Left, this);
					break;
			
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
		}

		public void Reset()
		{
			this.TileStatus = TileStatus.Normal;
		}

		public void TileStatusChanged()
		{
			switch (this.TileStatus)
			{
				case TileStatus.Normal:
					this.SpriteRenderer.sprite = this.Normal;
					break;
				
				case TileStatus.Walkable:
					this.SpriteRenderer.sprite = this.Walkable;
					break;
				
				case TileStatus.PlayerAttackable:
					this.SpriteRenderer.sprite = this.PlayerAttackable;
					break;
				
				case TileStatus.EnemyAttackable:
					this.SpriteRenderer.sprite = this.EnemyAttackable;
					break;
				
				case TileStatus.WalkableEnemyAttackable:
					this.SpriteRenderer.sprite = this.WalkableEnemyAttackable;
					break;
				
				case TileStatus.PlayerAttackableEnemyAttackAble:
					this.SpriteRenderer.sprite = this.PlayerAttackableEnemyAttackable;
					break;
				
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void PlayerWalk(int remaining, List<FieldTile> walkable)
		{
			//sanity check/security
			if (remaining == 0)
				return;

			walkable.Add(this);
			this._tileStatus |= TileStatus.Walkable;
			
			//preemptively stop
			if (remaining - 1 == 0)
				return;

			if (this.UpNeighbor != null)
				this.UpNeighbor.PlayerWalk(remaining - 1, walkable);
			if (this.DownNeighbor != null)
				this.DownNeighbor.PlayerWalk(remaining - 1, walkable);
			if (this.LeftNeighbor != null)
				this.LeftNeighbor.PlayerWalk(remaining - 1, walkable);
			if (this.RightNeighbor != null)
				this.RightNeighbor.PlayerWalk(remaining - 1, walkable);
		}
	}
}
