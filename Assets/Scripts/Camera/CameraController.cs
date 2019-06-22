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

		public TextBox TextBoxPrefab;

		protected OverworldPlayer OverworldPlayer;

		private void Awake()
		{
			GameManager.Instance.CameraController = this;
		}

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
					//STATIC POS - do nothing
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		protected void CameraModeChanged()
		{
			switch (this.CameraMode)
			{
				case CameraMode.FollowPlayer:
					break;
				case CameraMode.Static:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public TextBox SpawnTextBox(string text)
		{
			var tb = Instantiate(this.TextBoxPrefab, this.transform);
			tb.SetString(text);
			return tb;
		}
	}
}
