using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
namespace Game
{
    public partial class PlayerController
    {
        public PlayerInfo PlayerInfo;
        [Header("属性的原生数值")]
        public int HPVOrigin;
        public int MPVOrigin;
        public int STMOrigin;
        public int STROrigin;
        public int DEXOrigin;
        public int TECOrigin;
        public int LUCOrigin;
        [Header("属性的实时数值")]
		public int HP;
        public int MaxHPMod;
        public int MaxHP { get { return MaxHPMod + MaxHPCalculated; } }
        public int Elec;
        public int MaxElecMod;
        public int MaxElec { get { return MaxElecMod + MaxElecCalculated; } }
        public int MaxStaminaMod;
        public int MaxStamina { get { return MaxStaminaMod + MaxStaminaCalculated; } }
        public int Stamina;
        public int Soul;
        public int STRMod;
        public int STR { get { return STRMod + STROrigin; } }
        public int DEXMod;
        public int DEX { get { return DEXMod + DEXOrigin; } }
        public int TECMod;
        public int TEC { get { return TECMod + TECOrigin; } }
        public int LUCMod;
        public int LUC{ get { return LUCMod + LUCOrigin; } }
        public int Level;
        public int MaxHPCalculated { get { return CalcHPValue(HPVOrigin); } }
        
        
        public int MaxElecCalculated { get { return (int)(CalcHPValue(MPVOrigin)/4); } }
        public int MaxStaminaCalculated { get { return (int)(CalcHPValue(STMOrigin)/3); } }

        public float STRFix { get { return CalcFixValue(STR); } }
        public float DEXFix { get { return CalcFixValue(DEX); } }
        public float TECFix { get { return CalcFixValue(TEC); } }
        public float LUCFix { get { return CalcFixValue(LUC); } }

        private float staminaRecoverConuntdown = 0f;
        public bool LoadDataFromFile = true;

        public int DamageToTake = 0;
        public string DamageTag = "";

        public bool DebugInvinsible = true;

        public void RestoreAllToMax() {
            HP = MaxHP;
            Elec = MaxElec;
            Stamina = MaxStamina;
        }
        

        public void LockStamina() {
            staminaRecoverConuntdown = Constants.StaminaLockCountdown;
        }
        private void RecoverStamina(float deltatime) {
            if (Stamina >= MaxStamina) { 
                Stamina = MaxStamina;
                return;
            }
            if (staminaRecoverConuntdown <= 0) { 
                Stamina += 1;
            }
            else {
                staminaRecoverConuntdown -= deltatime;
            }
        }

        public void TakeDamage(int amount, string tag = "") {
            if (amount <= 0) return;
            
            if (DebugInvinsible) {
                Debug.Log($"==={tag} Damage: {amount}===invincible: {invinsibleTimer >= 0}");
                return;
            }
            if (invinsibleTimer <= 0) {
                DamageToTake = amount;
                DamageTag = tag;
                UpdateEquipsOnHurt();
                if (DamageToTake <= 0) return;
                HP -= DamageToTake;
                if (HP <= 0) {
                    HP = 0;
                    Die();
                }
                invinsibleTimer = Constants.InvinsibleOnHitTime;
                Flash();
            }
        }
        public void RestoreHP(int amount) {
            HP += amount;
            if (HP > MaxHP) HP = MaxHP;
        }
        public void RestoreElec(int amount) {
            Elec += amount;
            if (Elec > MaxElec) Elec = MaxElec;
        }

        private PlayerInfo LoadFromFile() {
            PlayerInfo info = null;
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            path = path + "\\PlayerInfo.xml";
            if (System.IO.File.Exists(path)) {
                // 创建 XML 序列化器
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(PlayerInfo));
                // 创建文件流，用于读取 XML 数据
                using (System.IO.TextReader reader = new System.IO.StreamReader(path)) {
                    // 使用序列化器将对象数据读取出来
                    info = serializer.Deserialize(reader) as PlayerInfo;
                }
                Debug.Log("Load from " + path);
            }
            // 删除原文件
            System.IO.File.Delete(path);
            return info;
        }
        private float CalcFixValue(int value) {
            float result = 0;
            if (value <= 9 && value >= 0) result = value / 100f;
            if (value <= 29 && value >= 10) result = (10 + (value - 10) * 2) / 100f;
            if (value <= 39 && value >= 30) result = (50 + (value - 30) * 3.2f) / 100f;
            if (value <= 98 && value >= 40) result = (80 + (value - 40) * 0.328f) / 100f;
            if (value >= 99) result = 1f;
            return result;
        }
        private int CalcHPValue(int value) {
            int result = 0;
            if (value <= 27 && value >= 0) result = (int)(value * 36.3f + 42);
            if (value <= 44 && value >= 28) result = (int)((value -27 )* 15.3f + 1020);
            if (value <= 72 && value >= 45) result = (int)((value - 44 ) * 2.5f + 1280);
            if (value <= 98 && value >= 73) result = (int)((value - 72 ) * 1.85f + 1350);
            if (value >= 99) result = 1400;
            return result;
        }
        public int NextLevelExp(int currentLevel) {
            int nextLevel = currentLevel;
            int result = 0;
            if (nextLevel + 81 < 92) {
                result = (int)(0.1f * Math.Pow(nextLevel + 81, 2) + 1);
            } else {
                result = (int)((0.1f + 0.02f * (nextLevel + 81 - 92)) * Math.Pow(nextLevel + 81, 2) + 1);
            }
            return result;
        }

        public void GainSoul(int amount) { 
            Soul += amount;
        }

        

        private void LoadPlayerInfo() {
            PlayerInfo = LoadFromFile();
            if (PlayerInfo == null) {
                PlayerInfo = ScriptableObject.CreateInstance<PlayerInfo>();
            }
            {
                STROrigin = PlayerInfo.STROrigin;
                DEXOrigin = PlayerInfo.DEXOrigin;
                TECOrigin = PlayerInfo.TECOrigin;
                LUCOrigin = PlayerInfo.LUCOrigin;
                HPVOrigin = PlayerInfo.HPVOrigin;
                MPVOrigin = PlayerInfo.MPVOrigin;
                STMOrigin = PlayerInfo.STMOrigin;
                Soul = PlayerInfo.Soul;
                Level = PlayerInfo.Level;
                PlayerInfo.SaveData();
            }
            
        }
        private void SavePlayerInfo() { 
            PlayerInfo.STROrigin = STROrigin;
            PlayerInfo.DEXOrigin = DEXOrigin;
            PlayerInfo.TECOrigin = TECOrigin;
            PlayerInfo.LUCOrigin = LUCOrigin;
            PlayerInfo.HPVOrigin = HPVOrigin;
            PlayerInfo.MPVOrigin = MPVOrigin;
            PlayerInfo.STMOrigin = STMOrigin;
            PlayerInfo.Soul = Soul;
            PlayerInfo.Level = Level;
            PlayerInfo.position = Position;
            PlayerInfo.SaveData();
        }
	}
}
