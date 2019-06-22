using TMPro;

using UnityEngine;

namespace Marker {
	public class HealthMarker : MonoBehaviour
	{

		[HideInInspector]
		public TextMeshProUGUI Test;
		private void Awake()
		{
			GameManager.Instance.HealthMarker = this;
			this.Test                         = this.GetComponent<TextMeshProUGUI>();
			this.Test.text = GameManager.Instance.stats.BaseHealth.ToString();
		}

		public void SetValue(string text)
		{
			this.Test.text = text;
		}
	}
}
