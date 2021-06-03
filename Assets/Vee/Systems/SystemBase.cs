using Vee.Interfaces;
using UnityEngine;

namespace Vee.Systems
{
    /// <summary>
    /// System 游戏相关逻辑
    /// </summary>
    public abstract class SystemBase : MonoBehaviour, ISystem
    {
        protected bool Inited;

        protected virtual bool OnlyInitOnce {
            get { return true; }
        }

        public void Initialize() {
            if (OnlyInitOnce && Inited) return;
        
            OnInitialize();
            Inited = true;
        }

        public void SetEnable(bool bEnable) {
            gameObject.SetActive(bEnable);
        }

        protected virtual void OnInitialize()
        {}

        protected virtual void OnDestroy()
        {    
        }
    }
}