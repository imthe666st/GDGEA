using System;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;

namespace Player {
	public class OverworldPlayer : PausableObject
	{
		public bool  CanMove  = true;
		public float MoveTime = 1;

		private Rigidbody2D _rigidbody2D;

		private List<RaycastHit2D> _castHits;
	
		private void Awake()
		{
			GameManager.Instance.OverWorldPlayer = this;
			
			this._rigidbody2D = this.GetComponent<Rigidbody2D>();
			this._castHits    = new List<RaycastHit2D>();
		}

		protected override void PausableUpdate()
		{
			if (this.CanMove)
			{
				var direction = this.ProcessMovementInput();
				this.CanMove = false;
			
				var willMove = this.TryMove(direction);

				if (!willMove) this.CanMove = true;
			}
		}

		private Direction ProcessMovementInput()
		{
			var horizontal = Input.GetAxis("Horizontal");
			var vertical   = Input.GetAxis("Vertical");

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
			var targetPos = this.transform.position;

			switch (direction)
			{
				case Direction.None:
					return false;
				case Direction.Up:
					targetPos += new Vector3(0, 1);
					break;
				case Direction.Down:
					targetPos += new Vector3(0, -1);
					break;
				case Direction.Left:
					targetPos += new Vector3(-1, 0);
					break;
				case Direction.Right:
					targetPos += new Vector3(1, 0);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}


			this._castHits.Clear();
			var hits = this._rigidbody2D.Cast(targetPos, this._castHits, targetPos.magnitude);

			if (hits > 0)
				return false;

			this.transform.DOMove(targetPos, this.MoveTime).OnComplete(() =>
																	   {
																		   this.CanMove = true;
																		   GameManager.Instance.OverworldPlayerMoved();
																	   });

			return true;
		}
	}
}
