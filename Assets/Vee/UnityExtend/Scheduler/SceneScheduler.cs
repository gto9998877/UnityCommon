using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vee
{
    /// <summary>
    /// 仅在当前Scene中有效，切换Scene终止所有的Coroutine
    /// </summary>
    public class SceneScheduler : MonoBehaviour
    {
        public static SceneScheduler Instance
        {
            get
            {
                if (instance == null)
                {
                    var newGo = VeeUtils.CreateGameObject("SchedulerScene", null);
                    instance = newGo.AddComponent<SceneScheduler>();
                }
    
                return instance;
            }
        }
    
        public static bool HasInstance { get { return instance != null; } }
        static SceneScheduler instance = null;
        void OnDestroy()
        {
            Scheduler.BreakAllNonGlobalCoroutines();
        }
        
        
        public IEnumerator StartNew(IEnumerator func, CoroutineHandle handle)
        {
            yield return func;

            Scheduler.StopCoroutine(handle);
        }
    }
}