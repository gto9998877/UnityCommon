using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MapCamera : MonoBehaviour {

	Camera cameraCom;
	float frustumHeight;
	float frustumWidth;
	// Use this for initialization
	void Start () {
		cameraCom = gameObject.GetComponent<Camera> ();

		ReCalcFrustum ();
	}
	
	// Update is called once per frame
	void Update () {
//		ReCalcFrustum ();
	}

	void ReCalcFrustum () {
		frustumHeight = cameraCom.orthographicSize * 2;
		frustumWidth = frustumHeight * cameraCom.aspect;
		vee.Utils.LogInEditor ("Map Camera Frustum : width " + frustumWidth + "  height " + frustumHeight);
	}
}
