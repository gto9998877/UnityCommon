using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Vee.Debugs.Log {
    [Serializable]
    public sealed class HistoryTable<T> {
        public HistoryTable(int maxCount) {
            MaxCount = maxCount;
        }
        
        [BoxGroup("History")]
        public int MaxCount;
        [BoxGroup("History")] 
        [ShowInInspector]
        List<T> ShowHistory = new List<T>();
        
        
        Queue<T> _history = new Queue<T>();
        public void Add(T ele) {
            _history.Enqueue(ele);

            CheckOverFlow();
        }

        public void Resize(int count) {
            if (count < 0) count = 0;
            MaxCount = count;
            
            CheckOverFlow();
        }

        void CheckOverFlow() {
            var overflow = _history.Count - MaxCount;
            for (var i = 0; i < overflow; ++i) {
                _history.Dequeue();
            }

            RefreshShow();
        }
        
        void RefreshShow() {
            ShowHistory = _history.ToArray().Invert();
        }

        [BoxGroup("History")]
        [Button(ButtonSizes.Medium)]
        public void Clear() {
            _history.Clear();
            RefreshShow();
        }
    }
}