using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Game;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Buff;
using Assets.Scripts.Items;

namespace Assets.Scripts.UI {
    public class UIOverPlayer:MonoBehaviour {
        public PlayerController Player;
        private int HP;
        public Image HPBar;
        private int Elec;
        public Image ElecBar;
        private int Stamina;
        public Image StaminaBar;
        private int MaxHP;
        public Image MaxHPBar;
        private int MaxElec;
        public Image MaxElecBar;
        private int MaxStamina;
        public Image MaxStaminaBar;
        private int Soul;
        public TextMeshProUGUI SoulText;
        public GameObject[] BuffPanels;
        public BuffIconList buffIconList;
        public GameObject VaccTubePrefab;
        public Transform VaccTubePanel;
        private GameObject[] VaccTubes;
        public GameObject[] MapButton;

        public Canvas PauseMenu;
        public bool isPaused = false;

        public Canvas DiePanel;
        private void Start() {
            Player = this.transform.parent.GetComponent<PlayerController>();
            InitializeTubePool();
        }
        private string Len3(int value) {
            string result = "";
            if (value < 10) result = "00" + value.ToString();
            if (value >= 10 && value < 100) result = "0" + value.ToString();
            if (value >= 100) result = value.ToString();
            return result;
        }

        private string Int2String(int value) {
            string result = "";
            if (value < 1000) result = value.ToString();
            if (value >= 1000 && value < 1000000) result = (value / 1000).ToString() + "," + Len3(value % 1000);
            if (value >= 1000000 && value < 1000000000) result = (value / 1000000).ToString() + "," + Len3((value % 1000000) / 1000) + "K";
            return result;
        }
        private void UpdateBarValues() { 
            HP = Player.HP;
            Elec = Player.Elec;
            Stamina = Player.Stamina;
            MaxHP = Player.MaxHP;
            MaxElec = Player.MaxElec;
            MaxStamina = Player.MaxStamina;
            Soul = Player.Soul;
            HPBar.rectTransform.sizeDelta = new Vector2(HP * 0.8f + 20f, 25f);
            ElecBar.rectTransform.sizeDelta = new Vector2(Elec * 2f+20f, 25f);
            StaminaBar.rectTransform.sizeDelta = new Vector2(Stamina * 3f + 20f, 25f);
            MaxHPBar.rectTransform.sizeDelta = new Vector2(MaxHP * 0.8f + 35f, 40f);
            MaxElecBar.rectTransform.sizeDelta = new Vector2(MaxElec * 2f+35f, 40f);
            MaxStaminaBar.rectTransform.sizeDelta = new Vector2(MaxStamina * 3f+35f, 40f);
            SoulText.text = Int2String(Soul);
        }
        private void UpdateBuff() {
            int displayCount = 0;
            for (int i = 0; i < Player.buffs.Count; i++) {
                if (Player.buffs[i].currentID != -1) {
                    BuffPanels[displayCount].GetComponent<BuffPanel>().display = true;
                    BuffPanels[displayCount].GetComponent<BuffPanel>().buffIcon = buffIconList.buffIcons[Player.buffs[i].icon].icon;
                    BuffPanels[displayCount].GetComponent<BuffPanel>().buffTime = Player.buffs[i].time;
                    BuffPanels[displayCount].SetActive(true);
                    displayCount++;
                }
            }
            while (displayCount < 6) {
                BuffPanels[displayCount].GetComponent<BuffPanel>().display = false;
                BuffPanels[displayCount].SetActive(false);
                displayCount++;
            }
        }

        public void InitializeTubePool() {
            VaccTubes = new GameObject[12];
            for (int i = 0; i < 12; i++) {
                GameObject tube = Instantiate(VaccTubePrefab, VaccTubePanel);
                tube.transform.localPosition = new Vector3(i * 40f + 4f, 0f, 0f);
                tube.GetComponent<Tube>().SetColor(TubeColor.Empty);
                VaccTubes[i] = tube;
            }
        }
        public void UpdateTubes() {
            switch (Player.CurrentTube) {
                case TubeType.HP:
                    for (int i = 0; i < 12; i++) {
                        if (i < Player.RedTubes) {
                            VaccTubes[i].SetActive(true);
                            VaccTubes[i].GetComponent<Tube>().SetColor(TubeColor.Red);
                        } else if (i < Player.MaxRedTubes) {
                            VaccTubes[i].SetActive(true);
                            VaccTubes[i].GetComponent<Tube>().SetColor(TubeColor.Empty);
                        } else {
                            VaccTubes[i].SetActive(false);
                        }
                    }
                    break;
                case TubeType.MP:
                    for (int i = 0; i < 12; i++) {
                        if (i < Player.BlueTubes) {
                            VaccTubes[i].SetActive(true);
                            VaccTubes[i].GetComponent<Tube>().SetColor(TubeColor.Blue);
                        } else if (i < Player.MaxBlueTubes) {
                            VaccTubes[i].SetActive(true);
                            VaccTubes[i].GetComponent<Tube>().SetColor(TubeColor.Empty);
                        } else {
                            VaccTubes[i].SetActive(false);
                        }
                    }
                    break;
                case TubeType.InfiMp:
                    for (int i = 0; i < 12; i++) {
                        if (i < Player.GreenTubes) {
                            VaccTubes[i].SetActive(true);
                            VaccTubes[i].GetComponent<Tube>().SetColor(TubeColor.Green);
                        } else if (i < Player.MaxGreenTubes) {
                            VaccTubes[i].SetActive(true);
                            VaccTubes[i].GetComponent<Tube>().SetColor(TubeColor.Empty);
                        } else {
                            VaccTubes[i].SetActive(false);
                        }
                    }
                    break;
                case TubeType.InfiStamina:
                    for (int i = 0; i < 12; i++) {
                        if (i < Player.YellowTubes) {
                            VaccTubes[i].SetActive(true);
                            VaccTubes[i].GetComponent<Tube>().SetColor(TubeColor.Yellow);
                        } else if (i < Player.MaxYellowTubes) {
                            VaccTubes[i].SetActive(true);
                            VaccTubes[i].GetComponent<Tube>().SetColor(TubeColor.Empty);
                        } else {
                            VaccTubes[i].SetActive(false);
                        }
                    }
                    break;
                case TubeType.None:
                    for (int i = 0; i < 12; i++) {
                        VaccTubes[i].SetActive(false);
                    }
                    break;
            }
        }
        private void Update() {
            if (Player == null) return;
            if (Player.isAlive) {
                DiePanel.gameObject.SetActive(false);
            } else { 
                DiePanel.gameObject.SetActive(true);
            }
            UpdateBarValues();
            UpdateBuff();
            UpdateTubes();
            //if pressed esc
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (isPaused) {
                    PauseMenu.gameObject.SetActive(false);
                    isPaused = false;
                } else {
                    PauseMenu.gameObject.SetActive(true);
                    isPaused = true;
                }
            }
            for(int i =0;i<5;i++) {
                if (GameManager.Instance.SaveData.isMapAchieved[i]) {
                    MapButton[i].SetActive(true);
                } else {
                    MapButton[i].SetActive(false);
                }
            }
        }
        public void Reset() {
            GameManager.Instance.ResetSaveData();
        }
        public void Respawn() { 
            
            GameManager.Instance.Respawn();
        }
        public void ChangeScene(int sceneID) {
            GameManager.Instance.TriggerSceneChange(sceneID);
        }
    }
}
