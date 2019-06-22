using UnityEngine;

namespace Marker {
    using System;

    using UnityEngine.UI;

    public class WeaponMarker : MonoBehaviour
    {
        [HideInInspector]
        public Image Image;
        
        private void Awake()
        {
            GameManager.Instance.WeaponMarker = this;
            this.Image = this.GetComponent<Image>();
            this.Image.sprite = GameManager.Instance.playerInventory.CurrentWeapon.Representation;
        }

        public void Set(Sprite sprite)
        {
            this.Image.sprite = sprite;
        }
    }
}
