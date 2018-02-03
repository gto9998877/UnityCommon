//================Generate Time : 02/03/2018 14:33:21================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIDialogTest : UIDialog {
	//=====================  Auto Generate UI Code Start, DO NOT MODIFY !========================
	public Image m_img1;
	public Text m_Text;
	//=====================  Auto Generate UI Code End  ========================

	void OnEnable () {
		vee.Utils.LogInEditor ("UIDialogTest onenable");

		m_Text.text = vee.Utils.DecorateWithSize (vee.Utils.DecorateWithColor ("yellow", "purple"), 20);
	}
}
