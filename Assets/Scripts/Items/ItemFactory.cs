using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items {
    public class ItemFactory {
        private Dictionary<int, Type> itemTypes = new Dictionary<int, Type>();

        public ItemFactory() {
            // 在构造函数中注册子类和对应的 id
            RegisterItemType(1, typeof(RedDew));
            RegisterItemType(2, typeof(Cat));
            // 添加更多的注册语句来处理其他 id 对应的子类
        }

        private void RegisterItemType(int id, Type itemType) {
            itemTypes[id] = itemType;
        }

        public ItemBase CreateItem(int id) {
            if (itemTypes.TryGetValue(id, out Type itemType)) {
                // 使用反射创建实例
                ItemBase newItem = Activator.CreateInstance(itemType) as ItemBase;
                return newItem;
            } else {
                // 如果没有匹配的 id，可以返回一个默认的实例或者抛出异常，根据需求
                return null;
            }
        }
    }

}
