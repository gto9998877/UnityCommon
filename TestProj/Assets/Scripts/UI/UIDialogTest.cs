//================Generate Time : 02/03/2018 11:10:00================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIDialogTest : UIDialog {
	//=====================  Auto Generate UI Code Start, DO NOT MODIFY !========================
	public Image m_Image1;
	public Image m_Image2;
	public Image m_Image3;
	//=====================  Auto Generate UI Code End  ========================

	void OnEnable () {
		vee.Utils.DebugLogInEditor ("UIDialogTest  onenable");
		this.m_Image1.gameObject.transform.Translate (new Vector3(0, 100, 0));
		this.m_Image2.gameObject.transform.Translate (new Vector3(0, -100, 0));
	}

	void OnDisable () {
		vee.Utils.DebugLogInEditor ("UIDialogTest  OnDisable");
		this.m_Image1.gameObject.transform.Translate (new Vector3(0, -100, 0));
		this.m_Image2.gameObject.transform.Translate (new Vector3(0, 100, 0));
	}
}
