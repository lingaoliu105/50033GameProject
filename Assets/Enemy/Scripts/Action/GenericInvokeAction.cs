using System.Reflection;
using UnityEngine;

namespace Game {
    [CreateAssetMenu(menuName = "Enemy/Generic Action")]
    public class GenericInvokeAction : Action
    {

        public string methodName;
        public override void Act(StateController controller) {
            EnemyController m = (EnemyController)controller;
            MethodInfo methodInfo = m.GetType().GetMethod(methodName);
            if (methodInfo != null)
            {
                // Invoke the method
                methodInfo.Invoke(m,null);
            }
        }
    }
}