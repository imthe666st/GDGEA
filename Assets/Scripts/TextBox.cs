using System;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
	private bool WaitForKey = false;

	public float FadeDuration = 1.5f;

	private Action onClosed;

	public void SetString(string text)
	{
		var image = this.GetComponentInChildren<Image>();
		var textC  = this.GetComponentInChildren<TextMeshProUGUI>();
		textC.text = text;
		
		var imageColor = image.color;
		imageColor.a = 0;

		image.color = imageColor;
		
		var textColor = textC.color;
		textColor.a = 0;

		textC.color = textColor;
	}

	private void Start()
	{
		GameManager.Instance.isPaused = true;
		
		var image = this.GetComponentInChildren<Image>();
		var text = this.GetComponentInChildren<TextMeshProUGUI>();

		var imageColor = image.color;
		imageColor.a = 1;

		var textColor = text.color;
		textColor.a = 1;

		var sq = DOTween.Sequence();
		sq.Append(image.DOColor(imageColor, this.FadeDuration)).Join(text.DOColor(textColor, this.FadeDuration))
		  .OnComplete(() => this.WaitForKey = true);
	}

	private void Update()
	{
		if (!this.WaitForKey)
			return;

		if (Input.GetButtonDown("Select"))
		{
			this.WaitForKey = false;
			
			var image = this.GetComponentInChildren<Image>();
			var text  = this.GetComponentInChildren<TextMeshProUGUI>();

			var imageColor = image.color;
			imageColor.a = 0;

			var textColor = text.color;
			textColor.a = 0;

			var sq = DOTween.Sequence();
			sq.Append(image.DOColor(imageColor, this.FadeDuration)).Join(text.DOColor(textColor, this.FadeDuration))
			  .OnComplete(() =>
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
