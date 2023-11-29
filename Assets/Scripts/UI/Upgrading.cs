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
            }
            if (DeltaLevel == 0) {
                LevelText.color = NormalColor;
                LevelText.text = Player.Level.ToString();
            }
        }

        private void Start() {
            //Player = this.transform.parent.GetComponent<PlayerController>();
            IsBonfire = true;
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

        public void Close() { 
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

            //Close();
        }
       
    }
}
