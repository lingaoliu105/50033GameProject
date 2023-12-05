using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "Player/Player Infomation")]
public class PlayerInfo: ScriptableObject {
    private string Name = "PlayerInfo";
    [Header("属性的原生数值")]

    public int HPVOrigin = 10;
    public int MPVOrigin = 10;
    public int STMOrigin = 10;
    public int STROrigin = 10;
    public int DEXOrigin = 10;
    public int TECOrigin = 10;
    public int LUCOrigin = 10;
    public int Level = 0;
    public int Soul = 1000;
    public int MaxRedTubes = 3;
    public int MaxBlueTubes = 1;
    [Header("属性的实时数值")]
    //public int MaxHP = 300;
    //public int MaxElec = 40;
    //public int MaxStamina = 80;
    //public int HP = 300;
    //public int Elec = 40;
    //public int Stamina = 80;
    
    //public int STR = 10;
    //public int DEX = 10;
    //public int TEC = 10;
    //public int LUC = 10;
    [Header("场景信息")]
    public Vector2 position;

    public void SaveData() {
        string SavePath = "";
        // 创建 XML 序列化器
        XmlSerializer serializer = new XmlSerializer(GetType());
        if (Name != null) {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            SavePath = path + "\\" + Name + ".xml";
        }
        // 创建文件流，用于写入 XML 数据
        using (TextWriter writer = new StreamWriter(SavePath)) {
            // 使用序列化器将对象数据写入文件
            serializer.Serialize(writer, this);
        }
        Debug.Log("Save to " + SavePath);
    }
    public void OnValidate() {
    }
}

