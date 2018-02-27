#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// debug脚本，仅在编辑器中有效
//用Gizmo的方式渲染场景中所有UI元素的rect，
//RaycastTarget为红色(截断input)，
//非RayCastTarget为蓝色(穿透input，showUnTarget为true时渲染)
public class DebugUIRect : MonoBehaviour {

	public bool showUnTarget = false;
	static Vector3[] fourCorners = new Vector3[4];

	void OnDrawGizmos()
    {
        foreach(MaskableGraphic g in GameObject.FindObjectsOfType<MaskableGraphic>()) {
			if(g.raycastTarget || showUnTarget)
			{
				RectTransform rectTransform = g.transform as RectTransform;
				rectTransform.GetWorldCorners(fourCorners);
				Gizmos.color = g.raycastTarget ? Color.red : Color.blue;
				for(int i = 0; i < 4; i++) {
					Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);
				}
			}
		}
	}
}
#endif