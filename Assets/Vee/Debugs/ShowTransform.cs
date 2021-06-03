using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Vee.Debugs {
    public class ShowTransform : MonoBehaviour {
        [BoxGroup("3D")] [ReadOnly] public Vector3 LocalPosition;
        [BoxGroup("3D")] [ReadOnly] public Vector3 WorldPosition;
        
        [BoxGroup("3D")] [ReadOnly] public Vector3 LocalRotation;
        [BoxGroup("3D")] [ReadOnly] public Vector3 WorldRotation;
        
        [BoxGroup("3D")] [ReadOnly] public Vector3 LocalScale;
        [BoxGroup("3D")] [ReadOnly] public Vector3 LossyScale;

        [BoxGroup("UI")] [ReadOnly] public bool IsRectTransform;
        [BoxGroup("UI")] [ReadOnly] public Vector2 Size;

        RectTransform rectTran;

        void Awake() {
            rectTran = transform as RectTransform;
            IsRectTransform = rectTran != null;
        }

        // Start is called before the first frame update
        void Start() {
            
        }

        // Update is called once per frame
        void Update() {
            Refresh();
        }

        [Button(ButtonSizes.Large)]
        void Refresh() {
            var target = transform;
            LocalPosition = target.localPosition;
            WorldPosition = target.position;

            LocalRotation = target.localRotation.eulerAngles;
            WorldRotation = target.rotation.eulerAngles;

            LocalScale = target.localScale;
            LossyScale = target.lossyScale;

            if (IsRectTransform) {
                Size = rectTran.sizeDelta;
            }
        }
    }
}