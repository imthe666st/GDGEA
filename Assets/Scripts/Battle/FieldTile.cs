using System;

using UnityEngine;

namespace Battle {
	public class FieldTile : MonoBehaviour
	{
		public FieldTile UpNeighbor;
		public FieldTile DownNeighbor;
		public FieldTile RightNeighbor;
		public FieldTile LeftNeighbor;

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
	}
}
