using Assets.Scripts.Items;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
public class Inventory:MonoBehaviour {
     public PlayerController player;
    public EquipableItem[] equipableItems;

    public void Update() {
        EquipableItem[] items = player.Equipments;
    }

}
