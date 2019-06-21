namespace Camera {
	using System;

	using Player;

	using UnityEngine;

	public class CameraController : PausableObject
	{
		public CameraMode cameraMode
		{
			get => this.CameraMode; 
			set { 
				this.CameraMode = value; 
				this.CameraModeChanged();
			}
		}
		
		[SerializeField]
		protected CameraMode CameraMode = CameraMode.FollowPlayer;

		protected OverworldPlayer OverworldPlayer;
		
		private void Start()
		{
			this.OverworldPlayer = GameManager.Instance.OverWorldPlayer;
		}

		protected override void Update()
		{
			base.Update();

			switch (this.CameraMode)
			{
				case CameraMode.FollowPlayer:
					
					var pos = this.transform.position;
					var playerPos = this.OverworldPlayer.transform.position;
					pos.x = playerPos.x;
					pos.y = playerPos.y;
					this.transform.position = pos;
					
					break;
				case CameraMode.Static:
					//TODO
					//STATIC POS
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		protected void CameraModeChanged()
		{
			
		}
	}
}
