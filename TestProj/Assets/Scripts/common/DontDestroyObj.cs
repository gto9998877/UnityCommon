using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObj : MonoBehaviour {

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
}
