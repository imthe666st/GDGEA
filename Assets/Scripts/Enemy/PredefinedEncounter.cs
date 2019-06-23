using UnityEngine;

public class PredefinedEncounter : MonoBehaviour
{
	public Enemy.Enemy Enemy;
	public int Count = 1;
	public bool NoLoot = false;
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		GameManager.Instance.StartPredefinedBattle(this.Enemy, this.OnDone, this.Count, this.NoLoot);
	}

	protected virtual void OnDone()
	{
		Destroy(this.gameObject);
	}
}
