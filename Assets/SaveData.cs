using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;
[CreateAssetMenu(fileName = "SaveData", menuName = "SaveData/SaveData", order = 1)]
public class SaveData: ScriptableObject{
    private string Name = "SaveInfo";
    // init all false bool array of size 15
    public bool[] isChestOpened = Enumerable.Repeat(false, 15).ToArray();
    public bool[] isMapAchieved = Enumerable.Repeat(false, 5).ToArray();
    public bool isBoss1Beaten = false;
    public bool isBoss2Beaten = false;
    public bool isMap2Reached = false;
    public bool isMap3Reached = false;
    public int currentMap = 0;

    public void SaveToFiles () {
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
}

