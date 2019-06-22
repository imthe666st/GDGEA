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
                if (inventory.CurrentWeapon != null) damage    += inventory.CurrentWeapon.Damage;
                if (inventory.CurrentModifier1 != null) damage += inventory.CurrentModifier1.Damage;
                if (inventory.CurrentModifier2 != null) damage += inventory.CurrentModifier2.Damage;

                return damage;
            }
        }

        public int Movement
        {
            get
            {
                var inventory = GameManager.Instance.playerInventory;

                var movement = this.BaseMovement;
                if (inventory.CurrentWeapon != null) movement    += inventory.CurrentWeapon.Movement;
                if (inventory.CurrentModifier1 != null) movement += inventory.CurrentModifier1.Movement;
                if (inventory.CurrentModifier2 != null) movement += inventory.CurrentModifier2.Movement;

                
                return movement;
            }
        }
    }
}
