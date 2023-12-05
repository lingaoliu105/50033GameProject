using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Game;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.UI {
    public class UpgradingOld :MonoBehaviour {
        public PlayerController Player;

        public TextMeshProUGUI SoulText;
        public TextMeshProUGUI LevelText;
        public TextMeshProUGUI STRText;
        public TextMeshProUGUI DEXText;
        public TextMeshProUGUI TECText;
        public TextMeshProUGUI LUCText;

                 public GameObject STRButtonUP;
                public GameObject DEXButtonUp;
                public GameObject TECButtonUp;
                public GameObject LUCButtonUp;
                public GameObject LUCButtonDown;
                public GameObject TECButtonDown;
                public GameObject DEXButtonDown;
                public GameObject STRButtonDown;
         
        private int tempLV = 0;
        private int displayLV = 0;
        private int tempSTR = 0;
        private int displaySTR = 0;
        private int tempDEX = 0;
        private int displayDEX = 0;
        private int tempTEC = 0;
        private int displayTEC = 0;
        private int tempLUC = 0;
        private int displayLUC = 0;
        private int tempSoul = 0;
        private int displaySoul = 0;

        private int[] costs = new int[999];

        private int nextLevelSoul = 0;


        private void Start() {
            Player = this.transform.parent.GetComponent<PlayerController>();
            Close();
        }
        private string Len3(int value) {
            string result = "";
            if (value < 10) result = "00" + value.ToString();
            if (value >= 10 && value < 100) result = "0" + value.ToString();
            if (value >= 100) result = value.ToString();
            return result;
        }

        public void Close() { 
            this.gameObject.SetActive(false);
        }
        public void Open() {
            this.gameObject.SetActive(true);
        }

        private string Int2String(int value) {
            string result = "";
            if (value < 1000) result = value.ToString();
            if (value >= 1000 && value < 1000000) result = (value / 1000).ToString() + "," + Len3(value % 1000);
            if (value >= 1000000 && value < 1000000000) result = (value / 1000000).ToString() + "," + Len3((value % 1000000) / 1000) + "K";
            return result;
        }
        public void STRUp() {
            if (nextLevelSoul <= displaySoul) {
                tempSTR++;
                tempLV++;
                costs[tempLV] = nextLevelSoul;
                tempSoul += nextLevelSoul;
            }
        }
        public void STRDown() { 
            tempSoul -= costs[tempLV];
            tempLV--;
            tempSTR--;
        }
        public void DEXUp() {
            if (nextLevelSoul <= displaySoul) {
                tempDEX++;
                tempLV++;
                costs[tempLV] = nextLevelSoul;
                tempSoul += nextLevelSoul;
            }
        }
        public void DEXDown() {
            tempSoul -= costs[tempLV];
            tempLV--;
            tempDEX--;
        }
        public void TECUp() {
            if (nextLevelSoul <= displaySoul) {
                tempTEC++;
                tempLV++;
                costs[tempLV] = nextLevelSoul;
                tempSoul += nextLevelSoul;
            }
        }
        public void TECDown() {
            tempSoul -= costs[tempLV];
            tempLV--;
            tempTEC--;
        }
        public void LUCUp() {
            if (nextLevelSoul <= displaySoul) {
                tempLUC++;
                tempLV++;
                costs[tempLV] = nextLevelSoul;
                tempSoul += nextLevelSoul;
            }
        }
        public void LUCDown() {
            tempSoul -= costs[tempLV];
            tempLV--;
            tempLUC--;
        }
        public void Confirm() {
            Player.STROrigin += tempSTR;
            Player.DEXOrigin += tempDEX;
            Player.TECOrigin += tempTEC;
            Player.LUCOrigin += tempLUC;
            Player.Level += tempLV;
            Player.Soul -= tempSoul;
            tempSTR = 0;
            tempDEX = 0;
            tempTEC = 0;
            tempLUC = 0;
            tempLV = 0;
            tempSoul = 0;
            Close();
        }
        private void UpdateValues() {
            displayLV = Player.Level + tempLV;
            nextLevelSoul = Player.NextLevelExp(displayLV);
            LevelText.text = displayLV.ToString();

            displaySoul = Player.Soul - tempSoul;

            if (nextLevelSoul > displaySoul) {
                STRButtonUP.SetActive(false);
                DEXButtonUp.SetActive(false);
                TECButtonUp.SetActive(false);
                LUCButtonUp.SetActive(false);
            } else { 
                STRButtonUP.SetActive(true);
                DEXButtonUp.SetActive(true);
                TECButtonUp.SetActive(true);
                LUCButtonUp.SetActive(true);
            }

            if (tempSTR > 0) {
                STRButtonDown.SetActive(true);
            } else {
                STRButtonDown.SetActive(false);
            }
            if (tempDEX > 0) {
                DEXButtonDown.SetActive(true);
            } else {
                DEXButtonDown.SetActive(false);
            }
            if (tempTEC > 0) {
                TECButtonDown.SetActive(true);
            } else {
                TECButtonDown.SetActive(false);
            }
            if (tempLUC > 0) {
                LUCButtonDown.SetActive(true);
            } else {
                LUCButtonDown.SetActive(false);
            }

            displaySTR = Player.STR + tempSTR;
            displayDEX = Player.DEX + tempDEX;
            displayTEC = Player.TEC + tempTEC;
            displayLUC = Player.LUC + tempLUC;


            SoulText.text = Int2String(displaySoul);

            STRText.text = displaySTR.ToString();
            DEXText.text = displayDEX.ToString();
            TECText.text = displayTEC.ToString();
            LUCText.text = displayLUC.ToString();
        }
        private void Update() {
            UpdateValues();
        }
    }
}
