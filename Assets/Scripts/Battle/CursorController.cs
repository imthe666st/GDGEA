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

		public Sprite Idle;
		public Sprite Selected;
		public float MoveTime = 0.2f;

		protected FieldTile PosTile;

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
			
			switch (GameManager.Instance.BattleState)
			{
				case BattleState.PlayerToMove:
					break;
				case BattleState.PlayerMoving:
					if (!this.PlayerMovable.Contains(targetPos))
						return false;
					break;
			}


			var targetLocation = targetPos.transform.position.ClearZ() + this.transform.position.OnlyZ();

			this.PosTile = targetPos;

			this.transform.DOMove(targetLocation, this.MoveTime).OnComplete(() => { this.Moveable = true; });

			return true;
		}

		private void ProcessSelectionInput()
		{
			if (Input.GetButtonDown("Select"))
			{

				switch (GameManager.Instance.BattleState)
				{
					case BattleState.PlayerToMove:
						//Check if highlighting Player
						if (Mathf.Abs((this.transform.position - GameManager.Instance.BattlePlayer.transform.position).magnitude) <= 0.1f)
						{
							GameManager.Instance.BattleState           = BattleState.PlayerMoving;
							this.GetComponent<SpriteRenderer>().sprite = this.Selected;
						}
						break;
					
					case BattleState.PlayerMoving:
						
						break;
				}
			}
			else if (Input.GetButtonDown("Deselect"))
			{
				switch (GameManager.Instance.BattleState)
				{
					case BattleState.PlayerMoving:

						//Check if highlighting Player
						if (Mathf.Abs((this.transform.position - GameManager.Instance.BattlePlayer.transform.position).magnitude) <= 0.1f)
						{
							GameManager.Instance.BattleState           = BattleState.PlayerToMove;
							this.GetComponent<SpriteRenderer>().sprite = this.Idle;
						}
						break;
				}
			}
		}
	}
}
