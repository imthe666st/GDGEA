using UnityEngine;

public class BattlePlayer : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.BattlePlayer = this;
    }
}
