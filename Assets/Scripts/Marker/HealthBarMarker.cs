using UnityEngine;

namespace Marker {
	public class HealthBarMarker : MonoBehaviour
	{
		public void Awake()
		{
			GameManager.Instance.BattlePlayer.HealthBarMarker = this;
		}

		public void Change(float val)
		{
			var scale = transform.localScale;
			scale.x = val;
			transform.localScale = scale;
		}
	}
}
