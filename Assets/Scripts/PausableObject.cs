using UnityEngine;

public class PausableObject : MonoBehaviour
{
    private bool _wasPausedBefore = false;
    
    protected virtual void Update()
    {
        if (GameManager.Instance.isPaused)
        {
            if (this._wasPausedBefore) return;
            
            this._wasPausedBefore = true;
            this.OnPaused();
            return;
        }
        
        this.PausableUpdate();
    }
    
    protected virtual void PausableUpdate() {}

    protected virtual void OnPaused() {}
}
