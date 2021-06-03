using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Vee
{
    public class UpdateController : MonoBehaviour
    {
        public static UpdateController BindTo (GameObject go, Callback updateFunc, bool autoStart = false) {
            var updateCtrl = go.GetOrAddComponent<UpdateController>();
            updateCtrl.SetUpdateFunc (updateFunc);
            if (autoStart)
            {
                // 先执行一次
                updateCtrl.Execute();
                
                updateCtrl.StartUpdate();
            } else {
                updateCtrl.StopUpdate ();
            }
            return updateCtrl;
        }
        public float MinStep = 0.1f;
        public float MaxStep = 0.1f;
        [ShowInInspector, ReadOnly]
        float updateTimer = 0f;
        [ShowInInspector, ReadOnly]
        float NextStep;
        public bool UpdateEnable = false;

        public float GetDeltaTime () {
            return updateTimer;
        }
        public void StartUpdate(bool resetTimer = false)
        {
            if (resetTimer) updateTimer = 0f;
            
            UpdateEnable = true;
            CalcNextStep();
        }

        public void StopUpdate()
        {
            UpdateEnable = false;
        }

        public float DeltaTimeSinceLastUpdate {
            get { return updateTimer; }
        }


        public void SetStep(float min, float max = -1f)
        {
            if (min < 0)
            {
                min = 0;
            }

            MinStep = min;
            if (max < MinStep)
            {
                max = MinStep;
            }
            MaxStep = max;

            CalcNextStep();
        }

        Callback updateFunc = null;
        public void SetUpdateFunc(Callback func)
        {
            updateFunc = func;
        }
        private void Awake()
        {

        }

        float CalcNextStep()
        {
            NextStep = VeeUtils.RandomFloat(MinStep, MaxStep);
            return NextStep;
        }

        void Execute()
        {
            updateFunc?.Invoke();
        }

        // Update is called once per frame
        void Update()
        {
            if (! UpdateEnable) return;

            updateTimer += Time.deltaTime;
            if (updateTimer > NextStep)
            {
                Execute();

                CalcNextStep();
                updateTimer = 0f;
            }
        }
    }
}