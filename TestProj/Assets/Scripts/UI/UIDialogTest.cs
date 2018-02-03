//================Generate Time : 02/03/2018 16:33:45================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIDialogTest : UIDialog {
	//=====================  Auto Generate UI Code Start, DO NOT MODIFY !========================
	public Image m_img1;
	public Text m_Text;
	public Button m_ButtonA;
	public Text m_btnAText;
	public Toggle m_Toggle1;
	public Slider m_Slider;
	//=====================  Auto Generate UI Code End  ========================

	void OnEnable () {
		vee.Utils.LogInEditor ("UIDialogTest onenable");

		//m_Text.text = vee.Utils.DecorateWithSize (vee.Utils.DecorateWithColor ("yellow", "purple"), 20);
	}

	public void OnButtonAClick () {
		vee.Utils.LogInEditor ("OnButtonAClick");

	}


	void Update () {
		if (vee.Utils.IsClickUI()) {
			vee.Utils.LogInEditor ("clickUI");
		}
	}

	void DialogShowEvent () {
		vee.Utils.LogInEditor ("DialogShowEvent");
	}
}
