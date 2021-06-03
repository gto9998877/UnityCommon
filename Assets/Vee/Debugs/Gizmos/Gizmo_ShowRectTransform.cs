
using UnityEngine;

namespace Vee.Debugs.CustomGizmos {

    public class Gizmo_ShowRectTransform : Gizmo_Base {
#if UNITY_EDITOR

        public RectTransform RT {
            get {
                if (_rt == null) {
                    _rt = transform.GetComponent<RectTransform>();
                }

                return _rt;
            }
        }

        RectTransform _rt;
        public Vector3[] Corners = new Vector3[4];
        protected override void DoDrawGizmos() {
            if (RT == null) return;

            RT.GetWorldCorners(Corners);
            UnityEngine.Gizmos.color = DrawColor;
            for (int j = 0; j < 4; j++) {
                UnityEngine.Gizmos.DrawLine(Corners[j], Corners[(j + 1) % 4]);
            }
        }
#endif
    }
}