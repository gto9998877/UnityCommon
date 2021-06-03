
using UnityEngine;

namespace Vee.Debugs.CustomGizmos {
    public class Gizmo_DrawSphereAtPosition : Gizmo_Base {
        public bool useCustomPos = false;
        public Vector3 customPos;
        public float radius = 0.1f;

        public void SetCustomPos(Vector3 pos) {
            customPos = pos;
        }

#if UNITY_EDITOR
        protected override void DoDrawGizmos() {
            var drawPos = transform.position;
            if (useCustomPos) {
                drawPos = customPos;
            }

            UnityEngine.Gizmos.color = DrawColor;
            UnityEngine.Gizmos.DrawSphere(drawPos, radius);
        }
#endif
    }
}
