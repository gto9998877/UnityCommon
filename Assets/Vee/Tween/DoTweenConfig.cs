
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Vee.Tweens {
    
    public class DoTweenConfig : MonoBehaviour {
        [BoxGroup("Configs")]
        public int TweenerCapacity = 500;
        [BoxGroup("Configs")]
        public int SequenceCapacity = 50;
        // Start is called before the first frame update
        void Start()
        {
            DOTween.SetTweensCapacity(TweenerCapacity, SequenceCapacity);
        }
    }
}

