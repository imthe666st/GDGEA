using System;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
	public float FadeTime = 1f;
	
	private Image _image;

	[HideInInspector]
	public Action OnDone;
	
	private void Awake()
	{
		this._image = this.GetComponentInChildren<Image>();

		var color = this._image.color;
		color.a = 0;
		this._image.color = color;

		var colornew = color;
		colornew.a = 1;

		this._image.DOColor(colornew, this.FadeTime).OnComplete(() =>
																{
																	this.OnDone?.Invoke();
																});
	}
	
	
}
