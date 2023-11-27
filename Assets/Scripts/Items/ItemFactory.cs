using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items {
    public class ItemFactory {
        private ItemDataObject itemList;

        public ItemFactory(ItemDataObject itemList) {
            this.itemList = itemList;
            // 在构造函数中注册子类和对应的 id
            // RegisterItemType(1, typeof(RedDew));
            // RegisterItemType(2, typeof(Cat));
            // 添加更多的注册语句来处理其他 id 对应的子类
        }

        private void RegisterItemType(int id, Type itemType) {
            // s
        }

        private ItemBase GetClassByName(string className) {
            // 使用 Type.GetType 获取类型
            Type myClassType = Type.GetType(className);
            ItemBase myClassInstance = null;
            if (myClassType != null) {
                // 创建类的实例
                myClassInstance = Activator.CreateInstance(myClassType) as ItemBase;
            } else {
                Console.WriteLine($"Type with name {className} not found.");
            }
            return myClassInstance;
        }

        public ItemBase CreateItem(int id) {
            string className = itemList.GetClassNameByID(id);
            ItemBase myClassInstance = GetClassByName(className);
            if (myClassInstance != null) {
                myClassInstance.InitializeItemInfo(itemList.GetItemInfoByID(id));
            }
            return myClassInstance;
        }

        /*
         public ItemBase CreateItem(string className) {
            ItemBase myClassInstance = GetClassByName(className);
            if (myClassInstance != null) {
                myClassInstance.InitializeItemInfo(itemList.GetItemInfoByID(className));
            }
            return myClassInstance;
        }
         */
    }

}
