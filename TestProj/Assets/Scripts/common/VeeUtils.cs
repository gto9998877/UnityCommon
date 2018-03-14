using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
		#endregion //LOG



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


		#endregion //Time

		#region String
		/// <summary>
		/// 给string加上颜色信息
		/// </summary>
		/// <returns>string with color</returns>
		/// <param name="src">Source string</param>
		/// <param name="colorName">Color name, you can use color name in rich text , also can see vee.ColorName </param>
		public static string DecorateWithColor (string src, string colorName )
		{
			return "<color=" + colorName + ">" + src + "</color>";
		}
		/// <summary>
		/// 给string加上字体大小信息
		/// </summary>
		/// <returns>string with size.</returns>
		/// <param name="src">Source string</param>
		/// <param name="size">Size number</param>
		public static string DecorateWithSize (string src, int size )
		{
			return "<size=" + size + ">" + src + "</size>";
		}

		#endregion // String

		#region EventSystem
		public static bool IsClickUI () {
			if (Input.GetMouseButton (0)) {
				return (EventSystem.current.IsPointerOverGameObject () || GUIUtility.hotControl != 0);
			}

			return false;
		}

		#endregion //EventSystem


		#region 游戏场景物体
		/// <summary>
		/// 查找Hierarchy顶层物体
		/// </summary>
		/// <returns>The top object.</returns>
		/// <param name="name">Name.</param>
		public static GameObject GetTopObject(string name)  
		{  
			UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();  
			GameObject[] rootObj = scene.GetRootGameObjects();  
			foreach (GameObject obj in rootObj)  
			{  
				if (obj.name == name)  
				{  
					return obj;  
				}  
			}  

			return null;  
		}  
		#endregion

		#region 随机，几何，计算相关
		/// <summary>
		/// Randoms the int.
		/// </summary>
		/// <returns>The int.</returns>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		public static int RandomInt (int start, int end) 
		{
			int len = end - start;
			return (int)(Mathf.Floor(Random.value * (len + 1)) + start);
		}
			
		/// <summary>
		/// Gets the point with angle. (2D)
		/// </summary>
		/// <returns>The point with angle.</returns>
		/// <param name="center">Center.</param>
		/// <param name="distance">Distance.</param>
		/// <param name="angle">Angle. 角度值 非弧度</param>
		public static Vector2 GetPointWithAngle (Vector2 center, float distance, float angle) 
		{
			var radian = angle * Mathf.PI / 180;
			var deltaX = Mathf.Sin(radian) * distance;
			var deltaY = Mathf.Cos(radian) * distance;				
			return new Vector2(center.x + deltaX, center.y + deltaY);
		}

		/// <summary>
		/// 获取一个平面圆区域内的一个随机点
		/// </summary>
		/// <returns>The position circle.</returns>
		/// <param name="center">Center.</param>
		/// <param name="radius">Radius.</param>
		public static Vector2 RandomPosCircle (Vector2 center, float radius) 
		{
			return GetPointWithAngle(center, RandomInt(0,100)/100f * radius, RandomInt(0, 360));
		}
		#endregion //随机，几何，计算相关
	}
}
