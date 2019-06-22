using UnityEngine;

namespace Player {
    public class BattlePlayer : MonoBehaviour
    {
        public int BaseMovement = 2;
        
        private void Awake()
        {
            GameManager.Instance.BattlePlayer = this;
        }

        public int Damage
        {
            get
            {
                var inventory = GameManager.Instance.playerInventory;

                var damage = 0;
                damage += inventory.CurrentWeapon?.Damage ?? 0;
                damage += inventory.CurrentModifier1?.Damage ?? 0;
                damage += inventory.CurrentModifier2?.Damage ?? 0;
                
                return damage;
            }
        }

        public int Movement
        {
            get
            {
                var inventory = GameManager.Instance.playerInventory;

                var movement = this.BaseMovement;
                movement += inventory.CurrentWeapon?.Movement ?? 0;
                movement += inventory.CurrentModifier1?.Movement ?? 0;
                movement += inventory.CurrentModifier2?.Movement ?? 0;
                
                return movement;
            }
        }
    }
}
