using System.Collections.Generic;

using UnityEngine;

namespace Battle {
	using System;
	using System.Linq;

	using DG.Tweening;

	public class CursorController : MonoBehaviour
	{
		public bool Moveable = false;
		public List<FieldTile> PlayerMovable;
		public List<FieldTile> PlayerAttackable;

		public Sprite Idle;
		public Sprite Selected;
		public float MoveTime = 0.2f;

		protected bool PlayerCanMove = false;

		protected FieldTile PosTile;

		protected FieldTile PlayerOldTile;

		private void Start()
		{
			this.PosTile = GameManager.Instance.Battlefield.Tiles.First(t => Mathf.Abs((t.transform.position.ClearZ()
																						-
																						this.transform.position.ClearZ()
																					   ).magnitude) <= 0.1f);
		}

		private void Update()
		{
			if (this.Moveable)
			{
				var direction = this.ProcessMovementInput();
				this.Moveable = false;
				var willMove                 = this.TryMove(direction);

				if (!willMove)
				{
					this.Moveable = true;
					this.ProcessSelectionInput();
				}
			}
		}

		private Direction ProcessMovementInput()
		{
			var horizontal = Input.GetAxis("Horizontal");
			var vertical = Input.GetAxis("Vertical");

			if (horizontal < 0)
				return Direction.Left;
			if (horizontal > 0)
				return Direction.Right;
			if (vertical < 0)
				return Direction.Down;
			if (vertical > 0)
				return Direction.Up;

			return Direction.None;
		}

		private bool TryMove(Direction direction)
		{
			FieldTile targetPos = null;
			
			switch (direction)
			{
				case Direction.None:
					return false;
				case Direction.Up:
					targetPos = this.PosTile.UpNeighbor;
					break;
				case Direction.Down:
					targetPos = this.PosTile.DownNeighbor;
					break;
				case Direction.Left:
					targetPos = this.PosTile.LeftNeighbor;
					break;
				case Direction.Right:
					targetPos = this.PosTile.RightNeighbor;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}

			if (targetPos == null)
				return false;
			
			/*
			switch (GameManager.Instance.BattleState)
			{
				case BattleState.PlayerToMove:
					break;
				case BattleState.PlayerMoving:
					if (!this.PlayerMovable.Contains(targetPos))
						return false;
					break;
			}*/


			var targetLocation = targetPos.transform.position.ClearZ() + this.transform.position.OnlyZ();

			this.PosTile = targetPos;

			this.transform.DOMove(targetLocation, this.MoveTime).OnComplete(() => { this.Moveable = true; });

			return true;
		}

		private void ProcessSelectionInput()
		{
			if (GameManager.Instance.BattleState == BattleState.PlayerToMove)
			{
				GameManager.Instance.BattleState           = BattleState.PlayerMoving;
				this.GetComponent<SpriteRenderer>().sprite = this.Selected;
				this.PlayerOldTile                         = this.PosTile;
				this.PlayerCanMove                         = true;
			}
			
			if (Input.GetButtonDown("Select"))
			{

				switch (GameManager.Instance.BattleState)
				{
					case BattleState.PlayerToMove:
						break;
						//Check if highlighting Player
						if (Mathf.Abs((this.transform.position - GameManager.Instance.BattlePlayer.transform.position).magnitude) <= 0.1f)
						{
							GameManager.Instance.BattleState           = BattleState.PlayerMoving;
							this.GetComponent<SpriteRenderer>().sprite = this.Selected;
							this.PlayerOldTile = this.PosTile;
							this.PlayerCanMove = true;
						}
						break;
					
					case BattleState.PlayerMoving:

						if (this.PlayerCanMove && this.PlayerMovable.Contains(this.PosTile))
						{
							GameManager.Instance.BattlePlayer.MoveToTile(this.PosTile);
							this.PlayerCanMove = false;
							GameManager.Instance.Battlefield.UpdateTilesPlayerAttack();
							GameManager.Instance.BattleState = BattleState.PlayerToAttack;
							this.PlayerMovable.Clear();
						}
						break;
					
					case BattleState.PlayerToAttack:

						if (this.PlayerAttackable.Contains(this.PosTile))
						{
							Debug.Log("Attack");
							GameManager.Instance.BattlePlayer.Attack(this.PosTile);
							foreach (var tile in GameManager.Instance.Battlefield.Tiles)
							{
								tile.ResetPlayerAttack();
							}
							Destroy(this.gameObject);
						}
						
						break;
						
				}
			}
			/*else if (Input.GetButtonDown("Deselect"))
			{
				
				switch (GameManager.Instance.BattleState)
				{
					case BattleState.PlayerMoving:

						GameManager.Instance.BattleState           = BattleState.PlayerToMove;
						this.GetComponent<SpriteRenderer>().sprite = this.Idle;

						this.transform.position = this.PlayerOldTile.transform.position.ClearZ() + this.transform
																									   .position
																									   .OnlyZ();
						this.PosTile       = this.PlayerOldTile;
						this.PlayerOldTile = null;
						break;
				}
			}*/
		}
	}
}
