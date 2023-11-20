using Assets.Scripts.Items;
using Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.UI {
    public class ItemSlot : MonoBehaviour {
        public bool display;
        public Image ItemIconDisplay;
        public Sprite ItemIcon;
        public int ItemID;
        public PlayerController player;

        void Update() {
            if (display) {
                this.gameObject.SetActive(true);
                ItemIconDisplay.sprite = ItemIcon;
            }
            
            if (!display) {
                this.gameObject.SetActive(false);
            } 
        }

        void EquipItem() {
            ItemFactory itemFactory = new ItemFactory();
            player.EquipItem((EquipableItem)itemFactory.CreateItem(1));
        }

        void UnequipItem() {
        
        }
    }
}

