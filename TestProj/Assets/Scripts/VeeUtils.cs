using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vee 
{
	public class Utils 
	{
		/// <summary>
		/// 在屏幕上显示文字
		/// GUI方法 : 只能在MonoBehaviour派生类的OnGUI方法中使用
		/// </summary>
		/// <param name="screenPos">Screen position.</param>
		/// <param name="content">Content.</param>
		/// <param name="fontSize">Font size.</param>
		public static void GUIText (Vector2 screenPos, string content, int fontSize = 50) 
		{
			GUI.skin.label.fontSize = fontSize;
			GUI.Label (new Rect(screenPos.x, screenPos.y, Screen.width, Screen.height), content);
		}

		#region LOG 
		/// <summary>
		/// 输出调试信息(仅在编辑器中，release无效)
		/// </summary>
		/// <param name="content">Content.</param>
		public static void LogInEditor (string content)
		{
			#if UNITY_EDITOR
			Debug.Log (content);
			#endif
		}

		public static void LogErrorInEditor (string content)
		{
			#if UNITY_EDITOR
			Debug.LogError (content);
			#endif
		}
		#endregion



		#region Time
		/// DateTime时间格式转换为13位的Unix时间戳（毫秒单位）
		/// </summary>
		/// <param name="time"> DateTime时间格式</param>
		/// <returns>Unix时间戳格式</returns>
		public static long ConvertDateTimeLong(System.DateTime time)
		{
			System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
			return (long)(time - startTime).TotalMilliseconds;
		}

		//=========================================
		/// <summary>
		/// DateTime时间格式转换为10位的Unix时间戳（秒单位）
		/// </summary>
		/// <param name="time"> DateTime时间格式</param>
		/// <returns>Unix时间戳格式</returns>
		public static int ConvertDateTimeInt(System.DateTime time)
		{
			System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
			return (int)(time - startTime).TotalSeconds;
		}

		/// <summary>
		/// 获取当前时间的时间戳(毫秒)
		/// </summary>
		/// <returns>The time now.</returns>
		public static long getTimeNow()
		{
			return vee.Utils.ConvertDateTimeLong (System.DateTime.Now);
		}

		/// <summary>
		/// 获取当前时间的时间戳(秒)
		/// </summary>
		/// <returns>The time now.</returns>
		public static long getTimeNowInSec()
		{
			return vee.Utils.ConvertDateTimeInt (System.DateTime.Now);
		}

		//========================================
		/// <summary>
		/// 时间戳转为C#格式时间
		/// </summary>
		/// <param name="timeStamp">Unix时间戳格式</param>
		/// <returns>C#格式时间</returns>
		public static System.DateTime ConvertTimeStampToDataTime(string timeStamp)
		{
			System.DateTime dtStart = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
			timeStamp += (timeStamp.Length == 13) ? "0000" : "0000000";
			long lTime = long.Parse(timeStamp);
			System.TimeSpan toNow = new System.TimeSpan(lTime);
			return dtStart.Add(toNow);
		}


		#endregion
	}
}
