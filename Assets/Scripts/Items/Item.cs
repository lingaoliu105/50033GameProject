using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Items {
    [Serializable]
    public class ItemBase {
        public string Name;
        public int ID;
        public string Description;

        public int Price;
        public int SellPrice;
        public int Rarity;
        
        public bool IsUpgradable = false;
        public bool IsStackable = false;
        public int Level = 0;
        public int MaxStack = -1;
        public int CurrentStack = -1;

        public Sprite Icon;

        public PlayerController player;
        public virtual void InitializeItemInfo(ItemInfo info) {
            Name = info.Name;
            ID = info.ID;
            Description = info.Description;
            Price = info.Price;
            SellPrice = info.SellPrice;
            Rarity = info.Rarity;
            IsUpgradable = info.IsUpgradable;
            IsStackable = info.IsStackable;
            Level = info.Level;
            MaxStack = info.MaxStack;
            CurrentStack = info.CurrentStack;
            Icon = info.Icon;
        }
        public virtual void Use() { return; }
    }
    [Serializable]
    public class EquipableItem: ItemBase{
        public int UpdateTime;
        public virtual void OnMeleeAttack() { }
        public virtual void OnAttackHit() {  }
        public virtual void OnRangedAttack() {  }
        public virtual void OnHurt() {  }
        public virtual void OnDeath() { }
        public virtual void Update() { }
        public virtual void OnEquip() { }
        public virtual void OnUnequip() { }
    }
    [Serializable]
    public struct ItemInfo {
        public string ClassName;

        public string Name;
        public int ID;
        public string Description;

        public int Price;
        public int SellPrice;
        public int Rarity;

        public bool IsUpgradable;
        public bool IsStackable;
        public int Level;
        public int MaxStack;
        public int CurrentStack;

        public Sprite Icon;
    }
    [Serializable]
    public struct ItemData {
        public int ID;
        public ItemInfo Info;
    }
    [CreateAssetMenu(fileName = "ItemDataObject", menuName = "Items/ItemList", order = 1)]
    public class ItemDataObject : ScriptableObject {
        [SerializeField]
        public List<KeyValuePair<int, ItemInfo>> list;
        public List<ItemData> rawList;
        public void Initialize() {
            list = new List<KeyValuePair<int, ItemInfo>>();
            for(int i = 0; i < rawList.Count; i++) {
                list.Add(new KeyValuePair<int, ItemInfo>(rawList[i].ID, rawList[i].Info));
            }
        }
        public String GetClassNameByID(int ID) {
            KeyValuePair<int, ItemInfo> resultPair = list.FirstOrDefault(pair => pair.Key == ID);
            return resultPair.Value.ClassName;
        }
        public ItemInfo GetItemInfoByID(int ID) {
            KeyValuePair<int, ItemInfo> resultPair = list.FirstOrDefault(pair => pair.Key == ID);
            return resultPair.Value;
        }

        
    }
}
