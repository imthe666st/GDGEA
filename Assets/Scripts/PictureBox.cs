using System;
using System.Linq;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

public class PictureBox : MonoBehaviour
{
	private bool WaitForKey = false;

	public float FadeDuration = 0.5f;

	private Action onClosed;

	public void SetImage(Sprite image)
	{
		var images = this.GetComponentsInChildren<Image>();

		foreach (var image1 in images)
		{
			var color = image1.color;
			color.a = 0;
			image1.color = color;
		}

		images.First(i => i.gameObject.name == "Image").sprite = image;
	}

	private void Start()
	{
		GameManager.Instance.isPaused = true;
		
		var images = this.GetComponentsInChildren<Image>();

		var sq = DOTween.Sequence();
		
		foreach (var image in images)
		{
			var color = image.color;
			color.a = 1;
			
			sq.Join(image.DOColor(color, this.FadeDuration));
		}

		sq.OnComplete(() => this.WaitForKey = true);
	}

	private void Update()
	{

		if (!this.WaitForKey)
			return;

		if (Input.GetButtonDown("Select"))
		{
			this.WaitForKey = false;
			
			var images = this.GetComponentsInChildren<Image>();

			var sq = DOTween.Sequence();

			foreach (var image in images)
			{
				var color = image.color;
				color.a = 0;

				sq.Join(image.DOColor(color, this.FadeDuration));
			}

			sq.OnComplete(() =>
						  {
							  GameManager.Instance.isPaused = false;
							  this.onClosed?.Invoke();
							  Destroy(this.gameObject);
						  });
		}
	}

	public void OnClosed(Action onClosed)
	{
		this.onClosed = onClosed;
	}
}
