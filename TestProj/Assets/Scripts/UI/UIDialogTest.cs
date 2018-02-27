//================Generate Time : 02/27/2018 13:45:40================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class Player
{
	public int id;
	public string name;
	public int life;
}

public class UIDialogTest : UIDialog {
	//=====================  Auto Generate UI Code Start, DO NOT MODIFY !========================
	public Image m_img1;
	public RawImage m_RawImage;
	public Text m_Text;
	public Button m_ButtonA;
	public Text m_btnAText;
	public Toggle m_Toggle1;
	public Slider m_Slider;
	//=====================  Auto Generate UI Code End  ========================

	void OnEnable () {

		string id = "test";
		int number = 100;
		string msg = "hello,world";

		UnityNetwork.NetPacket p = new UnityNetwork.NetPacket ();
		p.BeginWrite (id);
		p.WriteInt (number);
		p.WriteString (msg);
		p.EncodeHeader ();

		string id2 = "";
		int number2 = 0;
		string msg2 = "";

		UnityNetwork.NetPacket p2 = new UnityNetwork.NetPacket (p);
		p2.BeginRead (out id2);
		p2.ReadInt (out number2);
		p2.ReadString (out msg2);
		Debug.Log (string.Format("{0}, {1}, {2}", id2, number2, msg2));

		Debug.Log ("onenable finish");

		//m_Text.text = vee.Utils.DecorateWithSize (vee.Utils.DecorateWithColor ("yellow", "purple"), 20);
	}

	public void OnButtonAClick () {
		vee.Utils.LogInEditor ("OnButtonAClick");

		SceneMgr.LoadSceneWithLoadingDialog ("newSceneB");
	}


	void Update () {
//		if (vee.Utils.IsClickUI()) {
//			vee.Utils.LogInEditor ("clickUI");
//		}
	}

	void DialogShowEvent () {
		vee.Utils.LogInEditor ("DialogShowEvent");
	}

	public void OnImageClick () {
		vee.Utils.LogInEditor ("OnImageClick");

	}
}
