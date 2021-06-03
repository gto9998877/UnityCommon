using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vee.Debugs;

namespace Vee
{
    /// <summary>
    /// 
    /// </summary>
    public static class Scheduler
    {
        /// <summary>
        /// 开启协程，注意如果当前scene被关闭，协程终止
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public static CoroutineHandle StartCoroutine(IEnumerator cor)
        {
            var newHandle = NewHandle();
            var newInfo = new CoroutineInfo();
            newInfo.Id = newHandle.Id;
            newInfo.isGlobal = false;
            newInfo.coroutine = SceneScheduler.Instance.StartCoroutine(SceneScheduler.Instance.StartNew(cor, newHandle));
            _running_coroutines.Add(newInfo.Id, newInfo);
            return newHandle;
        }
        
        /// <summary>
        /// 开启协程(全局)，协程不会因为场景切换而被终止
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public static CoroutineHandle StartGlobalCoroutine(IEnumerator cor)
        {
            var newHandle = NewHandle();
            var newInfo = new CoroutineInfo();
            newInfo.Id = newHandle.Id;
            newInfo.isGlobal = true;
            newInfo.coroutine = GlobalScheduler.instance.StartCoroutine(GlobalScheduler.instance.StartNew(cor, newHandle));
            _running_coroutines.Add(newInfo.Id, newInfo);
            return newHandle;
        }

        /// <summary>
        /// 停止协程，调用此方法算作协程正常完成
        /// </summary>
        /// <param name="handle"></param>
        public static void StopCoroutine(CoroutineHandle handle)
        {
            if (handle == null) return;
            var id = handle.Id;
            if (_running_coroutines.ContainsKey(id))
            {
                var info = _running_coroutines[id];
                if (info.isGlobal)
                {
                    if (info.coroutine != null)
                    {
                        GlobalScheduler.instance.StopCoroutine(info.coroutine);
                        info.coroutine = null;
                    }
                }
                else
                {
                    if (info.coroutine != null && SceneScheduler.HasInstance)
                    {
                        SceneScheduler.Instance.StopCoroutine(info.coroutine);
                        info.coroutine = null;
                    }
                }

                if (DebugMode) _completed_coroutines.Add(id, info);
                _running_coroutines.Remove(id);
            }
        }

        /// <summary>
        /// 延迟回调
        /// </summary>
        /// <param name="func"></param>
        /// <param name="delaySec"></param>
        /// <returns></returns>
        public static CoroutineHandle DelayCall(Callback func, float delaySec = -1f)
        {
            return StartCoroutine(DelayCoroutine(func, delaySec));
        }

        static IEnumerator DelayCoroutine(Callback func, float delaySec)
        {
            if (delaySec < 0f)
            {
                // 等一帧
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(delaySec);
            }
            
            func?.Invoke();
        }

        static bool DebugMode = false;
        static int ObjectId = 0;
        public class CoroutineMap : Dict<int, CoroutineInfo> {}
        static CoroutineMap _running_coroutines = new CoroutineMap();
        static CoroutineMap _completed_coroutines = new CoroutineMap();    // 正常完成的，Debug模式下有效
        static CoroutineMap _breaked_coroutines = new CoroutineMap();  // 中断的，Debug模式下有效
        
        public static CoroutineHandle NewHandle()
        {
            var newHandle = new CoroutineHandle();
            newHandle.Id = ObjectId;

            ObjectId += 1;
            return newHandle;
        }

        public static void BreakAllNonGlobalCoroutines()
        {
            var toBeRemoved = new List<int>();
            VeeUtils.ForEachInDictionary(_running_coroutines, (k, v) => {
                if (!v.isGlobal)
                {
                    toBeRemoved.Add(k);
                }
            });
            
            foreach (var id in toBeRemoved)
            {
                if (DebugMode) _breaked_coroutines.Add(id, _running_coroutines[id]);
                _running_coroutines.Remove(id);
            }
            
            if (SceneScheduler.HasInstance) SceneScheduler.Instance.StopAllCoroutines();
        }

        public static void BreakAllCoroutines()
        {
            if (DebugMode)
            {
                VeeUtils.ForEachInDictionary(_running_coroutines, (k, v) => { _breaked_coroutines.Add(k, v); });
            }
            
            _running_coroutines.Clear();
            if (SceneScheduler.HasInstance) SceneScheduler.Instance.StopAllCoroutines();
            GlobalScheduler.instance.StopAllCoroutines();
        }

#if UNITY_EDITOR
        public static void LogState()
        {
            var runnings = new List<CoroutineInfo>(_running_coroutines.Values);
            VeeDebug.LogObj(runnings, $"Now Coroutines : count {runnings.Count}  ");
            
            var completes = new List<CoroutineInfo>(_completed_coroutines.Values);
            VeeDebug.LogObj(completes, $"Completed Coroutines : count {completes.Count}  ");
            
            var breaks = new List<CoroutineInfo>(_breaked_coroutines.Values);
            VeeDebug.LogObj(breaks, $"Break Coroutines : count {breaks.Count}  ");
        }
#endif

        [Serializable]
        public class CoroutineInfo
        {
            public int Id;
            public bool isGlobal;
            public Coroutine coroutine;
        }
    }

    [Serializable]
    public class CoroutineHandle
    {
        public int Id;
    }
    
}