
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Assertions;

namespace Vee.Tweens {
    public static class DOTweenHelper {
        /// <summary>
        /// Kills the Tween sequence.
        /// </summary>
        /// <param name="seq">Seq.</param>
        /// <param name="complete">If set to <c>true</c> complete the sequence instant</param>
        public static void KillSequence (ref Sequence seq, bool complete = false) {
            if (seq != null) {
                if (complete) {
                    seq.Complete ();
                }
                seq.Kill ();
                seq = null;
            }
        }

        public static void KillTween (ref DG.Tweening.Tween t, bool complete = false) {
            if (t != null) {
                t.Kill(complete);
                t = null;
            }
        }

        /// <summary>
        /// 延迟回调(至少延迟1帧，即使delay为0)
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="delay">延迟时间(单位秒)</param>
        /// <param name="ignoreTimeScale">是否忽略timescale影响</param>
        /// <returns></returns>
        public static DG.Tweening.Tween DelayCallback (Callback callback, float delay = 0.01f, bool ignoreTimeScale = true) {
            
            return DOVirtual.DelayedCall(delay, ()=> {
                callback?.Invoke();
            }, ignoreTimeScale);
        }
        
        /// <summary>
        /// 取消延迟回调
        /// </summary>
        /// <param name="callbackTween"></param>
        public static void CancelDelayCallback(ref Tween callbackTween) {
            if (callbackTween != null && callbackTween.IsActive() && callbackTween.IsPlaying()) {
                callbackTween.Pause();
                DOTween.Kill(callbackTween);
            }

            callbackTween = null;
        }

        /// <summary>
        /// tween 一个浮点数值
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="dur"></param>
        /// <param name="updateFunc"></param>
        /// <returns></returns>
        public static Tweener TweenValue (float from, float to, float dur, Action<float> updateFunc) {
            return DOVirtual.Float(from, to, dur, (v)=> {
                updateFunc (v);
            });
        }

        /// <summary>
        /// 计算Ease曲线插值
        /// </summary>
        /// <param name="from">区间左端点</param>
        /// <param name="to">区间右端点</param>
        /// <param name="t">区间百分比 0-1</param>
        /// <param name="easeType"></param>
        /// <returns></returns>
        public static float CalcEase (float from, float to, float t, Ease easeType){
            return DOVirtual.EasedValue(from, to, t, easeType);
        }

        /// <summary>
        /// 指定ui对象(2d)执行二次bezier曲线移动动画
        /// </summary>
        /// <param name="node">目标对象</param>
        /// <param name="endPoint">结束点</param>
        /// <param name="ctrlPoint">bezier控制点</param>
        /// <param name="duration">动画时间</param>
        /// <returns></returns>
        public static Tweener DoAnchorPosBezier(
            this RectTransform node, 
            Vector2 endPoint,  
            Vector2 ctrlPoint, 
            float duration) {

            var startPoint = node.anchoredPosition;
            return DOTween.To(t => {
                Vector2 bp = MathUtils.CalcBezierPoint(t, startPoint, ctrlPoint, endPoint);

                node.anchoredPosition = bp;
            }, 0f, 1f, duration);
        }

        public static Tween DOThrowTo(this RectTransform transform, Vector2 startSpeed, Vector2 endPos, float duration)
        {
            var startPos = transform.anchoredPosition;
            // d = vt + 1/2*at^2
            // => a = (d-vt)*2/t^2
            var accelerate = (endPos - startPos - startSpeed * duration) * 2 / duration / duration;
            return DOTween.To(time =>
            {
                var pos = startPos + startSpeed * time + 0.5f * accelerate * time * time;
                var direction = endPos - startPos;
                var delta = endPos - pos;
//                Debug.Log($"{time} => {pos}, {delta}, {direction}");
                if (!Mathf.Approximately(Mathf.Sign(direction.x), Mathf.Sign(delta.x)) && 
                    !Mathf.Approximately(Mathf.Sign(direction.y), Mathf.Sign(delta.y)))
                {
                    pos = endPos;
                }
                transform.anchoredPosition = pos;
            }, 0f, duration, duration);
        }

        public static Tween DOThrowTo(this Transform transform, Vector3 startSpeed, Vector3 endPos, float duration)
        {
            var startPos = transform.position;
            // d = vt + 1/2*at^2
            // => a = (d-vt)*2/t^2
            var accelerate = (endPos - startPos - startSpeed * duration) * 2 / duration / duration;
            return DOTween.To(time =>
            {
                var pos = startPos + startSpeed * time + 0.5f * accelerate * time * time;
                var direction = endPos - startPos;
                var delta = endPos - pos;
//                Debug.Log($"{time} => {pos}, {delta}, {direction}");
                if (!Mathf.Approximately(Mathf.Sign(direction.x), Mathf.Sign(delta.x)) && 
                    !Mathf.Approximately(Mathf.Sign(direction.y), Mathf.Sign(delta.y)))
                {
                    pos = endPos;
                }
                transform.position = pos;
            }, 0f, duration, duration);
        }
        

        #region TextMeshPro
        public static Tweener DOText(this TextMeshPro target, string endValue, float duration)
        {
            return DOTween.To(() => target.text, x => target.text = x, endValue, duration)
//                .SetOptions(richTextEnabled, scrambleMode, scrambleChars)
                .SetTarget(target);
        }
        
        public static Tweener DOText(this TextMeshProUGUI target, string endValue, float duration)
        {
            return DOTween.To(() => target.text, x => target.text = x, endValue, duration)
//                .SetOptions(richTextEnabled, scrambleMode, scrambleChars)
                .SetTarget(target);
        }
        #endregion // TextMeshPro

    }
}