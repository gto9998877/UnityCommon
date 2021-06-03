using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Vee.Debugs
{
    // debug脚本，仅在编辑器中有效
//用Gizmo的方式渲染场景中UI Graphic的rect，
//RaycastTarget为红色(截断input)，
//非RayCastTarget为蓝色(穿透input，showUnTarget为true时渲染)
    public class DrawUIRect : MonoBehaviour
    {
        [Tooltip(
            "Draw all ui Graphic elements in scene use red rect, when false, only draw select object's rect transform")]
        public bool drawAllInScene = false;

        [Tooltip(
            "Draw ui Graphic elements that is not RaycaseTarget use blue rect, is not effect when drawAllInScene is false")]
        public bool showUnRaycastTarget = false;

        private void OnEnable() { }

        private void OnDisable() { }
#if UNITY_EDITOR
        static Vector3[] fourCorners = new Vector3[4];
        void OnDrawGizmos()
        {
            if (!this.enabled) return;

            List<MaskableGraphic> drawList = new List<MaskableGraphic>();
            if (drawAllInScene)
            {
                drawList.AddRange(GameObject.FindObjectsOfType<MaskableGraphic>());
                foreach (MaskableGraphic g in drawList)
                {
                    if (g.raycastTarget || showUnRaycastTarget)
                    {
                        RectTransform rectTransform = g.transform as RectTransform;
                        rectTransform.GetWorldCorners(fourCorners);
                        Gizmos.color = g.raycastTarget ? Color.red : Color.blue;
                        for (int i = 0; i < 4; i++)
                        {
                            Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);
                        }
                    }
                }
            }
            else
            {
                UnityEngine.Object[] selectObjs =
                    Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.TopLevel);
                int selectCount = selectObjs.Length;
                for (var i = 0; i < selectCount; ++i)
                {
                    var obj = selectObjs[i] as GameObject;
                    if (obj == null) continue;
                    RectTransform rt = obj.GetComponent<RectTransform>();
                    if (rt != null)
                    {
                        rt.GetWorldCorners(fourCorners);
                        Gizmos.color = Color.red;
                        for (int j = 0; j < 4; j++)
                        {
                            Gizmos.DrawLine(fourCorners[j], fourCorners[(j + 1) % 4]);
                        }
                    }
                }
            }
        }
#endif
    }
}