namespace Enemy {
	using Camera;

	using DG.Tweening;

	using UnityEngine;
	using UnityEngine.SceneManagement;

	public class Boss1Encounter : PredefinedEncounter
	{
		public string[] WinText;
		private int accessWin = 0;

		public string AttackText;

		public float WalkDistance;
		public float WalkTime;

		public string SceneTo = "Level2";

		public GameObject ToDestroy;

		public FadeOut FadeOutPrefab;

		protected override void OnTriggerEnter2D(Collider2D other)
		{
			GameManager.Instance.CameraController.SpawnTextBox(this.AttackText).OnClosed(() =>
																						 {
																							 
																							 GameManager.Instance.StartPredefinedBattle(this.Enemy, this.OnDone, this.Count, this.NoLoot);
																						 });
		}

		protected override void OnDone()
		{
			Destroy(this.ToDestroy);
			
			if (GameManager.Instance.ShaderState <= ShaderState.Half)
			{
				GameManager.Instance.ShaderState = ShaderState.Half;
			}
			
			GameManager.Instance.isPaused = true;

			var camPos = GameManager.Instance.CameraController.transform.position.OnlyZ();
			GameManager.Instance.CameraController.transform.position = GameManager.Instance.OverWorldPlayer.transform
																				  .position.ClearZ() + camPos;

			GameManager.Instance.CameraController.cameraMode = CameraMode.Static;
			
			this.NextBox();
			
			base.OnDone();
		}

		protected void NextBox()
		{
			if (this.accessWin < this.WinText.Length)
			{
				GameManager.Instance.CameraController.SpawnTextBox(this.WinText[this.accessWin++]).OnClosed(this
				.NextBox);
			}
			else
			{
				var player = GameManager.Instance.OverWorldPlayer;
				player.transform.DOMoveY(player.transform.position.y + this.WalkDistance, this.WalkTime)
					  .OnComplete(this.ChangeScene);
			}
		}

		protected void ChangeScene()
		{
			var fo = Instantiate(this.FadeOutPrefab);
			fo.OnDone = () => { SceneManager.LoadScene(this.SceneTo, LoadSceneMode.Single); };
		}
	}
}
