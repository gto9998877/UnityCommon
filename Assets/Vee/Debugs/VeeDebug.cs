using System;
using Vee.Services.Times;

namespace Vee.Debugs
{
    /// <summary>
    /// 包含Debug通用方法
    /// 包装Unity自带的Debug相关方法，可以在适当的时候全局关闭或者开启
    /// </summary>
    public static class VeeDebug
    {
        // 启用日志
        const bool EnableLog = true;
        // 启用警告
        const bool EnableWarning = true;
        // 启用错误
        const bool EnableError = true;
        // 启用错误
        const bool EnableException = true;
        // 启用Assert
        const bool EnableAssert = true;
        
        public static void Log(string msg, bool timePrefix = false)
        {
//#if UNITY_EDITOR
            if (EnableLog)
            {
                var log = timePrefix ? $"[{BuildTimeStamp()}]" + msg : msg;
                UnityEngine.Debug.Log(log);
            }
//#endif
        }
        
        public static void LogInEditor(string msg, bool timePrefix = false)
        {
#if UNITY_EDITOR
            if (EnableLog)
            {
                var log = timePrefix ? $"[{BuildTimeStamp()}]" + msg : msg;
                UnityEngine.Debug.Log(log);
            }
#endif
        }

        public static void LogObj<T>(T obj, string tag = "", bool timePrefix = false)
        {
            if (EnableLog)
            {
                var log = timePrefix ? $"[{BuildTimeStamp()}]" + tag : tag;
                VeeUtils.LogObj(obj, log);
            }
        }
        
        public static void LogWarning(string msg, bool timePrefix = false)
        {
//#if UNITY_EDITOR
            if (EnableWarning)
            {
                var log = timePrefix ? $"[{BuildTimeStamp()}]" + msg : msg;
                UnityEngine.Debug.LogWarning(log);
            }
//#endif
        }
        
        public static void LogError(string msg, bool timePrefix = false)
        {
            if (EnableError)
            {
                var log = timePrefix ? $"[{BuildTimeStamp()}]" + msg : msg;
                UnityEngine.Debug.LogWarning(log);
            }
        }
        
        public static void LogException(Exception e)
        {
            if (EnableException)
            {
                var log = $"[{BuildTimeStamp()}]" + e.Message;
                UnityEngine.Debug.LogWarning(log);
            }
        }

        /// <summary>
        /// 程序应该确保condition为true，否则触发Assert，并显示msg
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="msg"></param>
        /// <param name="timePrefix"></param>
        public static void Assert(bool condition, string msg = "")
        {
//#if UNITY_EDITOR
            if (EnableAssert)
            {
                msg = VeeUtils.CheckStringValid(msg) ? msg : "Assert Failed";
                msg = $"[{BuildTimeStamp()}]" + msg;
                
                UnityEngine.Debug.Assert(condition, msg);
            }      
//#endif
        }
        
        public static string BuildTimeStamp() {
            return TimeService.Instance.BuildTimeString(DateTime.Now, TimeStringFormat.ColonHMS_MS);
        }
    }
}