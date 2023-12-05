using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game{
    public partial class PlayerController {
        public bool CanConsume {
            get {
                return GameInput.ConsumeButton.Pressed();
            }
        }
        public int MaxTubes = 5;
        public int MaxRedTubes = 2;
        public int MaxBlueTubes = 1;
        public int MaxGreenTubes = 1;
        public int MaxYellowTubes = 1;
        public int RedTubes = 0;
        public int BlueTubes = 0;
        public int GreenTubes = 0;
        public int YellowTubes = 0;
        public int TubeLevel = 0;
        public TubeType CurrentTube = TubeType.HP;
        [SerializeField]
        public EquipableItem[] Equipments;
        [SerializeField]
        public List<EquipableItem> EquipmentBackpack;
        public ItemFactory ItemFactory;

        private float tubeSwitchColdDown = 0f;

        public void InitInventory() { 
            Equipments = new EquipableItem[4];
            for (int i = 0; i < 4; i++) {
                Equipments[i] = null;
            }
            RechargeAllTubes();
            //Test Only
            EquipItem((EquipableItem)ItemFactory.CreateItem(101));
            EquipItem((EquipableItem)ItemFactory.CreateItem(102));
        }

        public void EquipItem(EquipableItem item) {
            if (item == null) {
                return;
            }
            for(int i = 0; i < 4; i++) {
                if (Equipments[i] == null) {
                    Equipments[i] = item;
                    item.player = this;
                    Equipments[i].OnEquip();
                    return;
                }
            }
        }

        public void UnequipItem(int id) {
            for (int i = 0; i < 4; i++) {
                if (Equipments[i] != null && Equipments[i].ID == id) {
                    Equipments[i].OnUnequip();
                    Equipments[i] = null;
                    return;
                }
            }
        }

        public void EquipItem(int id) {
            EquipableItem item = (EquipableItem)ItemFactory.CreateItem(id);
            if (item == null) {
                return;
            }
            for (int i = 0; i < 4; i++) {
                if (Equipments[i] == null) {
                    Equipments[i] = item;
                    item.player = this;
                    Equipments[i].OnEquip();
                    return;
                }
            }
        }

        public void UpdateEquipsOnUpdate() {
           
            for (int i = 0; i < 4; i++) {
                if (Equipments[i] != null) {
                    Equipments[i].Update();
                }
            }
        }
        public void UpdateEquipsOnMeleeAttackHit() {
            for (int i = 0; i < 4; i++) {
                if (Equipments[i] != null) {
                    Equipments[i].OnMeleeAttackHit();
                }
            }
        }
        public void UpdateEquipsOnMeleeAttack() {
            for (int i = 0; i < 4; i++) {
                if (Equipments[i] != null) {
                    Equipments[i].OnMeleeAttack();
                }
            }
        }
        public void UpdateEquipsOnRangedAttackHit() {
            for (int i = 0; i < 4; i++) {
                if (Equipments[i] != null) {
                    Equipments[i].OnRangedAttackHit();
                }
            }
        }
        public void UpdateEquipsOnRangedAttack() {
            for (int i = 0; i < 4; i++) {
                if (Equipments[i] != null) {
                    Equipments[i].OnRangedAttack();
                }
            }
        }
        public void UpdateEquipsOnHurt() {
            for (int i = 0; i < 4; i++) {
                if (Equipments[i] != null) {
                    Equipments[i].OnHurt();
                }
            }
        }
        public void UpdateEquipsOnDeath() {
            for (int i = 0; i < 4; i++) {
                if (Equipments[i] != null) {
                    Equipments[i].OnDeath();
                }
            }
        }

        public void SwitchTubeType() {
            if (RedTubes == 0 && BlueTubes == 0 && GreenTubes == 0 && YellowTubes == 0) {
                CurrentTube = TubeType.None;
                return;
            }
            int next = (int)CurrentTube % 4 + 1;
            while (next != (int)CurrentTube) {
                if (next == 1 && RedTubes > 0) {
                    CurrentTube = TubeType.HP;
                    return;
                }
                if (next == 2 && BlueTubes > 0) {
                    CurrentTube = TubeType.MP;
                    return;
                }
                if (next == 3 && GreenTubes > 0) {
                    CurrentTube = TubeType.InfiMp;
                    return;
                }
                if (next == 4 && YellowTubes > 0) {
                    CurrentTube = TubeType.InfiStamina;
                    return;
                }
                next = next % 4 + 1;
            }
        }

        public void UseTube() {
            GameInput.ConsumeButton.ConsumeBuffer();
            if (CurrentTube == TubeType.HP) {
                if (RedTubes > 0) {
                    RedTubes--;
                    VaccTube tube = new VaccTube(TubeLevel, TubeType.HP);
                    tube.player = this;
                    tube.Use();
                    if (RedTubes == 0) {
                        SwitchTubeType();
                    }
                }
            } else if (CurrentTube == TubeType.MP) {
                if (BlueTubes > 0) {
                    BlueTubes--;
                    VaccTube tube = new VaccTube(TubeLevel, TubeType.MP);
                    tube.player = this;
                    tube.Use();
                    if (BlueTubes == 0) {
                        SwitchTubeType();
                    }
                }
            } else if (CurrentTube == TubeType.InfiMp) {
                if (GreenTubes > 0) {
                    GreenTubes--;
                    VaccTube tube = new VaccTube(TubeLevel, TubeType.InfiMp);
                    tube.player = this;
                    tube.Use();
                    if (GreenTubes == 0) {
                        SwitchTubeType();
                    }
                }
            } else if (CurrentTube == TubeType.InfiStamina) {
                if (YellowTubes > 0) {
                    YellowTubes--;
                    VaccTube tube = new VaccTube(TubeLevel, TubeType.InfiStamina);
                    tube.player = this;
                    tube.Use();
                    if (YellowTubes == 0) {
                        SwitchTubeType();
                    }
                }
            }
        }

        public void RechargeAllTubes() {
            RedTubes = MaxRedTubes;
            BlueTubes = MaxBlueTubes;
            GreenTubes = MaxGreenTubes;
            YellowTubes = MaxYellowTubes;
        }
    }
}
