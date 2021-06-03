using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public static void LoadSceneWithLoadingDialog(string newSceneName, string loadingSceneName="LoadingScene") {
		Global.loadName = newSceneName;
		SceneManager.LoadScene (loadingSceneName);
	}

	void OnEnable()  
	{  
		SceneManager.sceneLoaded += OnSceneLoaded;  
	}  

	void OnDisable()  
	{  
		SceneManager.sceneLoaded -= OnSceneLoaded;  
	}  

	void OnSceneLoaded(Scene scence, LoadSceneMode mod)
	{
		vee.Utils.LogInEditor ("OnSceneLoadFinished");
	}
}
