using UnityEngine;

public class RitualArea : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
		// get ritual plate
		this.transform.parent.GetComponent<RitualPlate>().PerformRitual();
    }
}