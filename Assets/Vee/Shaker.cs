using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Games {
    public class Shaker : MonoBehaviour {
        [Serializable]
        public class ShakeParams {
            public float Duration = 0.2f; // 持续时间
            public Vector3 Strength = new Vector3(3f, 3f, 0f); // 强度
            public int Vibrato = 10; // 颤度
            public float Randomness = 90f; // 随机度
            public bool SnapToInt = false; // 取整
            public bool FadeOut = true; // 淡出
        }

        public Transform _Target;
        [BoxGroup("Position")]
        [Tooltip("2D : Strength的z=0")]
        public ShakeParams ShakePositionConfig;
        [BoxGroup("Rotation")]
        [Tooltip("2D : Strength的x=0, y=0")]
        public ShakeParams ShakeRotationConfig;
        [BoxGroup("Scale")]
        [Tooltip("2D : Strength的z=0")]
        public ShakeParams ShakeScaleConfig;

        public Transform Target {
            get {
                if (_Target == null) {
                    _Target = transform;
                }

                return _Target;
            }
        }


        Tweener positionTweener;

        [BoxGroup("Position")] [Button(ButtonSizes.Medium)]
        public void ShakePosition(int loop = 1) {
            StopShakePosition();

            positionTweener = Target.DOShakePosition(
                ShakePositionConfig.Duration,
                ShakePositionConfig.Strength,
                ShakePositionConfig.Vibrato,
                ShakePositionConfig.Randomness,
                ShakePositionConfig.SnapToInt,
                ShakePositionConfig.FadeOut).SetLoops(loop);
        }

        public void StopShakePosition() {
            if (positionTweener != null) {
                positionTweener.Kill(true);
                positionTweener = null;
            }
        }
        
        
        Tweener rotationTweener;

        [BoxGroup("Rotation")] [Button(ButtonSizes.Medium)]
        public void ShakeRotation(int loop = 1) {
            StopShakeRotation();

            rotationTweener = Target.DOShakeRotation(
                ShakeRotationConfig.Duration,
                ShakeRotationConfig.Strength,
                ShakeRotationConfig.Vibrato,
                ShakeRotationConfig.Randomness,
                ShakeRotationConfig.FadeOut).SetLoops(loop);
        }

        public void StopShakeRotation() {
            if (rotationTweener != null) {
                rotationTweener.Kill(true);
                rotationTweener = null;
            }
        }
        
        
        Tweener scaleTweener;

        [BoxGroup("Scale")] [Button(ButtonSizes.Medium)]
        public void ShakeScale(int loop = 1) {
            StopShakeScale();

            positionTweener = Target.DOShakeScale(
                ShakeScaleConfig.Duration,
                ShakeScaleConfig.Strength,
                ShakeScaleConfig.Vibrato,
                ShakeScaleConfig.Randomness,
                ShakeScaleConfig.FadeOut).SetLoops(loop);
        }

        public void StopShakeScale() {
            if (scaleTweener != null) {
                scaleTweener.Kill(true);
                scaleTweener = null;
            }
        }


        public void StopAll() {
            StopShakePosition();
            StopShakeRotation();
            StopShakeScale();
        }

        [Button(ButtonSizes.Large)]
        public void ShakeAll() {
            ShakePosition();
            ShakeRotation();
            ShakeScale();
        }
    }
}