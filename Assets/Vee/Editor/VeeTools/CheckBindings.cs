using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Vee.UnityExtend.Binding;

namespace Vee.Editor {
    public static class CheckBindings {
        [MenuItem("Veewo/CheckBindings", false)]
        public static void CheckBindingLostInSelect() {
            var selection = EditorUtils.GetSelectGameObjects();
            if (selection.Count <= 0) {
                return;
            }
            
            foreach (var obj in selection) {
                // Debug.Log(obj.name);
                var bindRoot = obj.GetComponent<BindRoot>();
                if (bindRoot != null) {
                    Check(bindRoot);
                }
                else {
                    Debug.LogWarning("Select GameObject do NOT have BindRoot component : " + obj.name);
                }
            }

            AssetDatabase.Refresh();
            
            Debug.Log("Check Finished");
        }

        public static void Check(BindRoot bindRoot) {
            var root = bindRoot.gameObject;
            var scriptName = bindRoot.ScriptName;

            if (!VeeUtils.CheckStringValid(scriptName)) {
                Debug.LogWarning("Bind Root Name is NOT valid on gameobject " + root.name);
                return;
            }

            var fullClassName = scriptName;
            if (VeeUtils.CheckStringValid(bindRoot.NameSpace)) {
                fullClassName = bindRoot.NameSpace + "." + scriptName;
            }
            var types = ReflectionHelper.GetTypesInAllLoadedAssemblies(t => t.FullName == fullClassName).ToList();
            if (types.Count <= 0) {
                Debug.LogWarning($"Not Found class [{fullClassName}] in Assemblies");
                return;
            }
            
            if (types.Count > 1) {
                Debug.LogWarning($"Multy Type found for class [{scriptName}], please fill nameSpace ");
                return;
            }

            var comType = types[0];
            var com = bindRoot.gameObject.GetComponent(comType);
            if (com == null) {
                Debug.LogWarning($"Not Found {fullClassName} Component on {root.name}");
                return;
            }

            var eles = root.GetComponentsInChildren<BindElement>(true);
            var oldNames = new List<string>();
            foreach (var ele in eles) {
                if (ele.RootName != scriptName) continue;

                var eleBindName = ele.BindName;
                if (oldNames.Contains(eleBindName))
                {
                    Debug.LogWarning($"Checking bindname [{eleBindName}] on [{ele.gameObject.name}], name Repeated !!!");
                }
                else
                {
                    oldNames.Add(eleBindName);
                    Debug.Log($"Checking bindname [{eleBindName}] on [{ele.gameObject.name}]");
                }
                
                var bindField = comType.GetField(eleBindName);
                if (bindField == null) {
                    Debug.LogWarning($"BindElement [{eleBindName}] is not a field of {scriptName}");
                    continue;
                }

                var fieldType = bindField.FieldType;
                var eleType = ele.EleType;

                if (fieldType != eleType) {
                    Debug.LogWarning($"BindElement [{eleBindName}] Type [{ele.Type}] not matching field type [{bindField.FieldType}] in {scriptName}");
                    continue;
                }

                UnityEngine.Object eleClass = null;
                if (fieldType.FullName == "UnityEngine.GameObject") {
                    eleClass = ele.gameObject;
                }
                else {
                    eleClass = ele.gameObject.GetComponent(fieldType);
                }
                
                if (eleClass == null) {
                    Debug.LogWarning($"Not Found [{fieldType}] Class on [{ele.gameObject}], bind name [{eleBindName}]");
                    continue;
                }

                if (bindField.GetValue(com) != (object)eleClass) {
                    Debug.Log($"[{eleBindName}] is filled");
                    bindField.SetValue(com, eleClass);
                }
            }
            
            Debug.Log($"Check finished on [{bindRoot.ScriptName}]");
        }
    }
}