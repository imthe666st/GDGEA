using System;
using System.Collections;
using System.Collections.Generic;
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
		sq.Append(images[0].DOColor(Color.white, this.FadeDuration));
		
		foreach (var image in images.Skip(1))
		{
			sq.Join(image.DOColor(Color.white, this.FadeDuration));
		}

		sq.OnComplete(() => this.WaitForKey = true);
	}

	private void Update()
	{

		if (!this.WaitForKey)
			return;

		var images = this.GetComponentsInChildren<Image>();
		
		var targetC = new Color(1, 1, 1, 0);

		var sq = DOTween.Sequence();
		sq.Append(images[0].DOColor(targetC, this.FadeDuration));

		foreach (var image in images.Skip(1))
		{
			sq.Join(image.DOColor(targetC, this.FadeDuration));
		}

		sq.OnComplete(() =>
					  {
						  GameManager.Instance.isPaused = false;
						  this.onClosed?.Invoke();
						  Destroy(this.gameObject);
					  });
	}

	public void OnClosed(Action onClosed)
	{
		this.onClosed = onClosed;
	}
}
