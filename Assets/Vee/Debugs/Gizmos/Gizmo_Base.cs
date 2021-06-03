
using UnityEngine;

namespace Vee.Debugs.CustomGizmos {
    public abstract class Gizmo_Base : MonoBehaviour {
        public Color DrawColor = Color.red;
        
        protected virtual void OnEnable() {
            
        }
        
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (enabled) {
                DoDrawGizmos();
            }
        }
#endif
        protected virtual void DoDrawGizmos() {
            
        }
    }
}