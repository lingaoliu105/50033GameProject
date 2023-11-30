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


    public class UpgradingValueBlock:MonoBehaviour {
        public ValueBlockType Type;

        public Upgrading UpgradingUI;

        public GameObject UpButton;
        public GameObject DownButton;

        public TextMeshProUGUI Key;
        public string KeyString;
        public TextMeshProUGUI Value;
        public string ValueString;
        public int ValueInt;
        public int DeltaValueInt;

        public Color HigherValueColor = Color.green;
        public Color NormalColor = Color.black;
        public Color LowerValueColor = Color.red;


        public void TryValueUp() { 
            if (UpgradingUI.CanUpgrade) {
                UpgradingUI.UpgradeCalc();
                DeltaValueInt+=1;
            }
        }

        public void TryValueDown() {
            if (DeltaValueInt > 0) { 
                UpgradingUI.DowngradeCalc();
                DeltaValueInt-=1;
            }
        }

        public void Reset() {
            DeltaValueInt = 0;
            ValueInt = UpgradingUI.GetValue(Type);
        }

        public void Start() {
            Key.text = KeyString;
            Value.text = ValueString;
            ValueInt = UpgradingUI.GetValue(Type);
            DeltaValueInt = 0;
        }


        public void Update() {
            
            if (UpgradingUI.CanUpgrade) {
                UpButton.SetActive(true);
            } else {
                UpButton.SetActive(false);
            }
            if (DeltaValueInt > 0) {
                DownButton.SetActive(true);
            } else {
                DownButton.SetActive(false);
            }
            if (DeltaValueInt > 0) {
                Value.color = HigherValueColor;
            } 
            if (DeltaValueInt < 0) {
                Value.color = LowerValueColor;
            }
            if (DeltaValueInt == 0) {
                Value.color = NormalColor;
            }
            ValueString = (ValueInt + DeltaValueInt).ToString();
            Value.text = ValueString;

        }

        public void UpButtonPressed() { 
            TryValueUp();
        }
        
        public void DownButtonPressed() {
            TryValueDown();
        }
    }
}
