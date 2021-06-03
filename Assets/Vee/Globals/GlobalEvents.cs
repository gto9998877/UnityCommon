using Vee.Events;
using UnityEngine.Events;

namespace Vee.Globals
{    
    
    public class GlobalEvents : MonoSingleton<GlobalEvents>
    {
        protected override void OnDestroy() {
            // 清除所有的事件监听
            var t = typeof(GlobalEvents);
            var fields = t.GetFields();
            var baseType = typeof(UnityEventBase);
            foreach (var field in fields) {
                if (baseType.IsAssignableFrom(field.FieldType)) {
                    var f = field.GetValue(null) as UnityEventBase;
                    f?.RemoveAllListeners();
                }
            }
            
            base.OnDestroy();
        }

        // App
        public static UnityEvent ApplicationQuit = new UnityEvent();    // app 退出
        public static UnityEvent ApplicationFocusIn = new UnityEvent(); // app 得到焦点
        public static UnityEvent ApplicationFocusOut = new UnityEvent();// app 失去焦点
        public static UnityEvent ApplicationPause = new UnityEvent();   // app 暂停 
        public static UnityEvent ApplicationResume = new UnityEvent();  // app 恢复
        
        // System
        public static VeeResultEvent ErrorMessage = new VeeResultEvent();  // 错误事件

    }
}