//================Generate Time : 02/26/2018 13:23:00================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIDialogLoading : UIDialog {
	//=====================  Auto Generate UI Code Start, DO NOT MODIFY !========================
	public Slider m_Slider;
	//=====================  Auto Generate UI Code End  ========================


	//异步对象
	AsyncOperation async;

	//读取场景的进度，它的取值范围在0 - 1 之间。
	int loadProgress = 0;
	int showProgress = 0;

	void Start()
	{
		vee.Utils.LogInEditor ("loading Start");
		async = SceneManager.LoadSceneAsync(Global.loadName/*, LoadSceneMode.Additive*/);
		StartCoroutine(loadScene());
	}

	IEnumerator loadScene()
	{
		async.allowSceneActivation = false;  //如果加载完成，也不进入场景  

		loadProgress = 0;           
		showProgress = 0;  

		//测试了一下，进度最大就是0.9  
		while (async.progress < 0.9f)   
		{  
			//计算读取的进度，
			//progress 的取值范围在0.1 - 1之间， 但是它不会等于1
			//也就是说progress可能是0.9的时候就直接进入新场景了。
			//为了计算百分比 所以直接乘以100即可
			loadProgress =  (int)(async.progress *100);
			Debug.Log("Loading Progress : " +loadProgress);
			while ( showProgress<loadProgress)  
			{  
				showProgress++;  
				m_Slider.value = showProgress / 100f;
				yield return new WaitForEndOfFrame(); //等待一帧  
			}  
		}  
		//计算0.9---1   其实0.9就是加载好了，我估计真正进入到场景是1    
		loadProgress = 100;                    

		while (showProgress < loadProgress )  
		{  
			showProgress++;  
			m_Slider.value = showProgress / 100f;
			yield return new WaitForEndOfFrame(); //等待一帧  
		}  

//		yield return new WaitForSeconds (2f);
//		SceneManager.UnloadSceneAsync ("LoadingScene");
		async.allowSceneActivation = true;  //如果加载完成，可以进入场景  
	}
}
