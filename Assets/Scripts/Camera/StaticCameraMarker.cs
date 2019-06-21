using UnityEngine;

namespace Camera {
	using System;

	public class StaticCameraMarker : MonoBehaviour
	{
		public void Start()
		{
			GameManager.Instance.CameraController.transform.position = this.transform.position;
		}
	}
}
