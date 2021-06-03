using UnityEngine;
using UnityEditor;
using Vee.UnityExtend.Binding;

namespace Vee.Editor {
    [CustomEditor(typeof(BindRoot),true)]
    public class BindRootEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var myTarget = (BindRoot)target;
            if (GUILayout.Button("RefreshBindings"))
            {
                CheckBindings.Check(myTarget);
            }
        }
    }   

}