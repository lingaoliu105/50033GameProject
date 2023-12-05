using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Game;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace Assets.Scripts.UI {
    [Serializable]
    public struct ValueBlock { 
    
    
    }

    public enum ValueBlockType {
        HPV = 0,
        MPV = 1,
        STM = 2,
        STR = 3,
        DEX = 4,
        TEC = 5,
        LUC = 6,
        Size = 7,
        LV = 8,
        Soul = 9,
    }

    public class Upgrading:MonoBehaviour {
        public PlayerController Player;

        public bool IsBonfire = false;
        public int DeltaLevel = 0;
        public int DeltaSoul = 0;
        public int NextLevelSoul = 0;

        public UpgradingValueBlock[] ValueBlocks;

        public TextMeshProUGUI SoulText;
        public string SoulString;
        public int SoulValue;
        public TextMeshProUGUI LevelText;
        public string LevelString;
        public int LevelValue;
        public TextMeshProUGUI HPText;
        public string HPString;
        public int HPValue;
        public TextMeshProUGUI MPText;
        public string MPString;
        public int MPValue;
        public TextMeshProUGUI STMText;
        public string STMString;
        public int STMValue;

        public GameObject ConfirmButton;

        public Color HigherValueColor = Color.green;
        public Color NormalColor = Color.black;
        public Color LowerValueColor = Color.red;

        public bool CanUpgrade {
            get {
                if (IsBonfire) {
                    return (Player.Soul - DeltaSoul) >= NextLevelSoul;
                }
                return false;
            }
        }


        public void UpgradeCalc() {
            
            DeltaLevel += 1;
            costs[DeltaLevel] = NextLevelSoul;
            DeltaSoul += NextLevelSoul;
            NextLevelSoul = Player.NextLevelExp(Player.Level + DeltaLevel);
        }

        public void DowngradeCalc() { 
            DeltaSoul -= costs[DeltaLevel];
            NextLevelSoul = costs[DeltaLevel];
            costs[DeltaLevel] = 0;
            DeltaLevel -= 1;
        }

        public int GetValue(ValueBlockType type) {
            if (Player == null) return 0; 
            switch (type) {
                case ValueBlockType.HPV:
                    return Player.HPVOrigin;
                case ValueBlockType.MPV:
                    return Player.MPVOrigin;
                case ValueBlockType.STM:
                    return Player.STMOrigin;
                case ValueBlockType.STR:
                    return Player.STROrigin;
                case ValueBlockType.DEX:
                    return Player.DEXOrigin;
                case ValueBlockType.TEC:
                    return Player.TECOrigin;
                case ValueBlockType.LUC:
                    return Player.LUCOrigin;
                default:
                    return 0;
            }
        }
        

        private int[] costs = new int[999];


        public void Update() {
            if (Player == null) return;
            SoulValue = Player.Soul - DeltaSoul;
            if (DeltaSoul > 0) {
                SoulText.color = LowerValueColor;
                SoulText.text = Player.Soul.ToString()+" -> " +SoulValue.ToString();
            } 
            if (DeltaSoul == 0) {
                SoulText.color = NormalColor;
                SoulText.text = Player.Soul.ToString();
             
            }
            if (DeltaLevel > 0) {
                LevelText.color = HigherValueColor;
                LevelText.text = Player.Level.ToString()+" -> " + (Player.Level + DeltaLevel).ToString();
                ConfirmButton.SetActive(true);
            }
            if (DeltaLevel == 0) {
                LevelText.color = NormalColor;
                LevelText.text = Player.Level.ToString();
                ConfirmButton.SetActive(false);
            }
            
            HPValue = Player.HPVOrigin + ValueBlocks[(int)ValueBlockType.HPV].DeltaValueInt;
            if (ValueBlocks[(int)ValueBlockType.HPV].DeltaValueInt > 0) {
                HPText.color = HigherValueColor;
                HPText.text = Player.CalcHPValue(Player.HPVOrigin).ToString()+" -> " + Player.CalcHPValue(HPValue).ToString();
            } else {
                  HPText.color = NormalColor;
                HPText.text = Player.CalcHPValue(HPValue).ToString();
            }
            MPValue = Player.MPVOrigin + ValueBlocks[(int)ValueBlockType.MPV].DeltaValueInt;
            if (ValueBlocks[(int)ValueBlockType.MPV].DeltaValueInt > 0) {
                MPText.color = HigherValueColor;
                MPText.text = (Player.CalcHPValue(Player.MPVOrigin)/4).ToString()+" -> " + (Player.CalcHPValue(MPValue)/4).ToString();
            } else {
                  MPText.color = NormalColor;
                MPText.text = (Player.CalcHPValue(MPValue)/4).ToString();
            }
            STMValue = Player.STMOrigin + ValueBlocks[(int)ValueBlockType.STM].DeltaValueInt;
            if (ValueBlocks[(int)ValueBlockType.STM].DeltaValueInt > 0) {
                STMText.color = HigherValueColor;
                STMText.text = (Player.CalcHPValue(Player.STMOrigin)/3).ToString()+" -> " + (Player.CalcHPValue(STMValue)/3).ToString();
            } else {
                  STMText.color = NormalColor;
                STMText.text = (Player.CalcHPValue(STMValue)/3).ToString();
            }
        }

        private void Start() {
            //Player = this.transform.parent.GetComponent<PlayerController>();
            IsBonfire = true;
            Rewind();
            NextLevelSoul = Player.NextLevelExp(Player.Level + DeltaLevel);
            //Close();
        }
        private string Len3(int value) {
            string result = "";
            if (value < 10) result = "00" + value.ToString();
            if (value >= 10 && value < 100) result = "0" + value.ToString();
            if (value >= 100) result = value.ToString();
            return result;
        }

        public void Rewind() { 
            //Clear all delta
            DeltaLevel = 0;
            DeltaSoul = 0;
            NextLevelSoul = Player.NextLevelExp(Player.Level + DeltaLevel);
            for (int i = 0; i < (int)ValueBlockType.Size; i++) {
                ValueBlocks[i].Reset();
            }
            //this.gameObject.SetActive(false);
        }
        public void Open(bool isBonfire = false) {
            //IsBonfire = isBonfire;
            //this.gameObject.SetActive(true);

        }

        public IEnumerator OpenAnimate() { yield return null; }
        public IEnumerator CloseAnimate() { yield return null; }

        private string Int2String(int value) {
            string result = "";
            if (value < 1000) result = value.ToString();
            if (value >= 1000 && value < 1000000) result = (value / 1000).ToString() + "," + Len3(value % 1000);
            if (value >= 1000000 && value < 1000000000) result = (value / 1000000).ToString() + "," + Len3((value % 1000000) / 1000) + "K";
            return result;
        }
        
        public void Confirm() {
            if (IsBonfire) {
                Player.Level += DeltaLevel;
                Player.Soul -= DeltaSoul;
                Player.HPVOrigin += ValueBlocks[(int)ValueBlockType.HPV].DeltaValueInt;
                Player.MPVOrigin += ValueBlocks[(int)ValueBlockType.MPV].DeltaValueInt;
                Player.STMOrigin += ValueBlocks[(int)ValueBlockType.STM].DeltaValueInt;
                Player.STROrigin += ValueBlocks[(int)ValueBlockType.STR].DeltaValueInt;
                Player.DEXOrigin += ValueBlocks[(int)ValueBlockType.DEX].DeltaValueInt;
                Player.TECOrigin += ValueBlocks[(int)ValueBlockType.TEC].DeltaValueInt;
                Player.LUCOrigin += ValueBlocks[(int)ValueBlockType.LUC].DeltaValueInt;
                Player.RestoreAllToMax();
                Rewind();
            }
        }
       
    }
}
