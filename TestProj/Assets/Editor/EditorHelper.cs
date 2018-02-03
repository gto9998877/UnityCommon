using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
using UnityEngine.UI;

using vee;
public class EditorHelper : MonoBehaviour {
	#region Menu items
	[MenuItem("vee/Check Hierarchy")]
	static void CheckHierarchy () {
		Debug.Log ("Check Hierarchy Start");
		GameObject[] objs = FindObjectsOfType(typeof(GameObject)) as GameObject[]; //获取所有gameobject元素给数组obj
		List<string> audioListeners = new List<string>();
		foreach (GameObject obj in objs)    //遍历所有gameobject
		{
//			Debug.Log(obj.name);  //可以在unity控制台测试一下是否成功获取所有元素

			if (obj.GetComponent<AudioListener> () != null)
			{
				audioListeners.Add (obj.name);
			}

			if (obj.GetComponent<Canvas> () != null) {
				EditorHelper.CheckCanvas (obj);
			}
		}

		if (audioListeners.Count == 0) {
			Debug.LogError ("No Audio Listener Found In Current SCENE !!");
		} else if (audioListeners.Count > 1) {
			string ws = "TOO MANY Audio Listener Found In Current SCENE : ";
			foreach (string name in audioListeners) {
				ws += name + "   ";
			}
			Debug.LogError (ws);
		}
			
		Debug.Log ("Check Hierarchy Finish");
		AssetDatabase.Refresh ();
	}
		
	[MenuItem("vee/Test")]
	static void TestCode () {
//		vee.Utils.GUIText (new Vector2(10,10), "zwz");
		var timeNow = vee.Utils.ConvertDateTimeInt(System.DateTime.Now);
		System.DateTime dt = vee.Utils.ConvertTimeStampToDataTime (timeNow.ToString());
		Debug.Log(dt.Date);
	}
	#endregion //Menu items



	#region Editor Listeners
	[UnityEditor.Callbacks.DidReloadScripts]
	static void onEditorReloadScripts() {
		Debug.Log ("onEditorReloadScripts");

		bool needRefresh = EditorHelper.checkUITempFile ();

		if (needRefresh) {
			AssetDatabase.Refresh ();
		}
	}
	#endregion //Editor Listeners



	#region common functions
	public static void deleteFileFromProject (string fileNameWithPath, bool logMsg = true) {
		if (logMsg) {
			Debug.Log ("The File : " + fileNameWithPath + " is DELETED ");
		}
		File.Delete (fileNameWithPath + ".meta");
		File.Delete (fileNameWithPath);
	}
	#endregion // common functions



	#region UI 
	static string autoGenUiCodeStart = "\t//=====================  Auto Generate UI Code Start, DO NOT MODIFY !========================";
	static string autoGenUiCodeEnd = "\t//=====================  Auto Generate UI Code End  ========================";
	static void CheckCanvas (GameObject go) {
		// check if script file exist
		string fileDir = Application.dataPath + "/Scripts/UI";
		if (! Directory.Exists (fileDir)) {
			Directory.CreateDirectory (fileDir);
		}

		string className = go.name;
		string fileName = fileDir + "/" + className + ".cs";
		string tempInfoFileName = fileDir + "/" + "tempUiInfo.txt";

		StringWriter preContent = new StringWriter ();
		StringWriter postContent = new StringWriter ();
		FileInfo file_info = new FileInfo (fileName);  
		if (file_info.Exists) {
			// find autoCode start and end, and cache old code contents
			StreamReader sr = file_info.OpenText ();

			bool genCodeStartFound = false;
			bool genCodeEndFound = false;

			int lintIndex = 0;
			string line = sr.ReadLine ();
			// ignore first line
			if (line.Substring (0, 2) == "//") {
				line = sr.ReadLine ();
			}
			while (line != null) {
				if (line == EditorHelper.autoGenUiCodeEnd) {
					genCodeEndFound = true;
				}

				if (!genCodeStartFound) {
					preContent.WriteLine (line);
				} else if (genCodeEndFound){
					postContent.WriteLine (line);
				}

				if (line == EditorHelper.autoGenUiCodeStart) {
					genCodeStartFound = true;
				}

				line = sr.ReadLine ();
				lintIndex++;
			}

			sr.Close ();
			sr.Dispose ();

			if (!genCodeStartFound || !genCodeEndFound) {
				Debug.LogError ("Error Gen UI Code for : " + className + ".cs, can not find CODE start or end");
				preContent.Close ();
				preContent.Dispose ();
				postContent.Close ();
				postContent.Dispose ();
				return;
			}
		} else {
			// write new script file
			preContent.WriteLine ("using System.Collections;");
			preContent.WriteLine ("using System.Collections.Generic;");
			preContent.WriteLine ("using UnityEngine;");
			preContent.WriteLine ("using UnityEngine.UI;");
			preContent.WriteLine ("\n");
			preContent.WriteLine ("public class " + className + " : UIDialog {");
			preContent.WriteLine (EditorHelper.autoGenUiCodeStart);

			postContent.WriteLine (EditorHelper.autoGenUiCodeEnd);
			postContent.WriteLine ("}");
		}

		// write old nessesary code  and auto generate UI code
		StreamWriter sw = file_info.CreateText ();
		sw.WriteLine ("//================Generate Time : " + System.DateTime.Now.ToString() + "================");
		sw.Write (preContent.ToString());

		UiObject[] uiArray = go.GetComponentsInChildren<UiObject> ();

		foreach (UiObject uiObj in uiArray) {
			Image im = uiObj.gameObject.GetComponent<Image> ();
			if (im != null) {
				sw.WriteLine("\tpublic Image m_" + uiObj.gameObject.name + ";"); 
			}
		}

		sw.Write (postContent.ToString());

		// dont forget close all
		preContent.Close ();
		preContent.Dispose ();
		postContent.Close ();
		postContent.Dispose ();
		sw.Flush ();
		sw.Close ();  
		sw.Dispose ();
		Debug.Log (fileName + " generated success !");

		// save UIClass name and object instanceId to temp file.
		// because now the new generate script is not compile yet, we can not get class info
		// until Unity Editor finish compile new files(handle in onEditorReloadScripts)
		FileInfo temp_file = new FileInfo (tempInfoFileName);  
		StreamWriter tsw;
		if (temp_file.Exists) {
			tsw = temp_file.AppendText ();
		} else {
			tsw = temp_file.CreateText ();
		}
		tsw.WriteLine (className + " " + go.GetInstanceID());
		tsw.Flush ();
		tsw.Close ();
		tsw.Dispose ();
	}

	public static bool checkUITempFile () {
		string fileDir = Application.dataPath + "/Scripts/UI";
		if (! Directory.Exists (fileDir)) {
			Directory.CreateDirectory (fileDir);
		}
		string tempInfoFileName = fileDir + "/" + "tempUiInfo.txt";
		FileInfo temp_file = new FileInfo (tempInfoFileName);  
		if (temp_file.Exists) {
			StreamReader sr = temp_file.OpenText ();

			string line = sr.ReadLine ();
			while (line != null) {
				string[] dialogInfos = line.Split (' ');
				if (dialogInfos.Length == 2) {
					string dialogName = dialogInfos [0];
					string instanceId = dialogInfos [1];

					GameObject dialogGo = EditorUtility.InstanceIDToObject(int.Parse(instanceId)) as GameObject;
					if (dialogGo != null) {
						EditorHelper.getReferenceForUIScript (dialogGo, dialogName);
					}
				}
				line = sr.ReadLine ();
			}

			sr.Close ();
			sr.Dispose ();

			EditorHelper.deleteFileFromProject (tempInfoFileName, false);
			return true;
		}

		return false;
	}

	public static void getReferenceForUIScript (GameObject go, string dialogClassName){
		// check if Dialog Script On gameObject, if not, add it
		UIDialog dialog = go.GetComponent<UIDialog> () as UIDialog;
		if (dialog == null) {
			Type myUIClass = typeof(UIDialog).Assembly.GetType(dialogClassName);
			if (myUIClass != null) {
				go.AddComponent(myUIClass);
			}
		}

		// check every child uiObject, get reference 
		dialog = go.GetComponent<UIDialog> () as UIDialog;
		if(dialog != null){
			UiObject[] uiArray = go.GetComponentsInChildren<UiObject> ();

			foreach (UiObject uiObj in uiArray) {
				FieldInfo fi = dialog.GetType().GetField ("m_" + uiObj.gameObject.name);
				if (uiObj.gameObject.GetComponent<Image> () != null) {
					fi.SetValue (dialog, uiObj.gameObject.GetComponent<Image> ());
				}
			}
		}
	}
	#endregion // UI
}
