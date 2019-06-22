using UnityEngine;

namespace Marker {
	public class EnemyHealthBarMarker : MonoBehaviour
	{
		public void Awake()
		{
			this.transform.parent.GetComponent<Enemy.Enemy>().HealthBarMarker = this;
		}

		public void Change(float val)
		{
			var scale = this.transform.localScale;
			scale.x              = val;
			this.transform.localScale = scale;
		}
	}
}
