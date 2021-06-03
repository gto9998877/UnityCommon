
using Sirenix.OdinInspector;
using UnityEngine;

namespace Vee {
    public class Tracker2D : MonoBehaviour {        
        public Camera _camera;
        [Tooltip("Enable时自动开始追踪")]
        public bool AutoStartOnEnable = true;
        [Tooltip("到达后自动停止追踪")]
        public bool StopOnArrive = true;
        
        [Tooltip("起始速度")]
        public Vector2 StartSpeed = Vector2.zero;
        [Tooltip("检查到达半径")] [MinValue(0.001f)]
        public float CheckArriveRadius = 0.1f;
        [Tooltip("追踪强度")] [MinValue(0.1f)]
        public float TrackPower = 10f;
        [Tooltip("是否正在追踪")]
        public bool Tracking = false;
        [Tooltip("目标位置 (世界)")]
        public Vector3 TargetPos;

        #region Editor Values
        [Tooltip("目标位置 (屏幕)")] [ShowInInspector, ReadOnly]
        protected Vector2 _TargetScreenPos;
        [Tooltip("当前速度")] [ShowInInspector, ReadOnly]
        protected Vector2 _CurrentSpeed;
        [Tooltip("当前位置 (世界)")] [ShowInInspector, ReadOnly]
        protected Vector2 _CurrentPos;
        [Tooltip("当前位置 (屏幕)")] [ShowInInspector, ReadOnly]
        protected Vector2 _CurrentScreenPos;
        #endregion

        
        [Tooltip("是否已经到达")] [ReadOnly]
        public bool IsArrived = false;

        public Action<Tracker2D> OnArrive;

        protected virtual Camera Viewer {
            get {
                if (_camera == null) {
                    _camera = Camera.main;
                }

                return _camera;
            }
        }

        
        
        public virtual Vector3 Position {
            get { return transform.position; }
            set { transform.position = new Vector3(value.x, value.y, Position.z); }
        }
        
        public virtual Vector2 ScreenPos {
            get { return Viewer.WorldToScreenPoint(Position); }
        }
        
        public virtual Vector3 TargetScreenPos {
            get { return Viewer.WorldToScreenPoint(TargetPos); }
        }

        public virtual void TrackScreenPos(Vector2 pos) {
            TargetPos = Viewer.ScreenToWorldPoint(pos);
            StartTrack();
        }
        
        public virtual void TrackPos(Vector3 pos) {
            TargetPos = new Vector3(pos.x, pos.y);
            StartTrack();
        }
        

        public void StartTrack() {
            if (! Tracking) {
                _CurrentSpeed = StartSpeed;
            }

            Tracking = true;
        }

        public void StopTrack() {
            Tracking = false;
        }
        
        void OnEnable() {
            if (AutoStartOnEnable) {
                StartTrack();
            }
        }

        // Update is called once per frame
        void Update() {
            var dt = Time.unscaledDeltaTime;

#if UNITY_EDITOR
            // Update Status For Editor
            _TargetScreenPos = TargetScreenPos;
            _CurrentPos = Position;
            _CurrentScreenPos = ScreenPos;
#endif 
            
            CheckArrive();
            if (IsArrived) {
                OnArrive?.Invoke(this);
                if (StopOnArrive) {
                    StopTrack();
                }
            }
            
            UpdateTrace(dt);
        }
        
        void CheckArrive () {
            var nowPos = Position;

            var checkSize = CheckArriveRadius * 2;
            var finishBound = new Bounds(TargetPos, new Vector3(checkSize, checkSize));
            IsArrived = finishBound.Contains(nowPos);
        }
        void UpdateTrace(float dt) {
            if (! Tracking) return;
            
            var nowPos = Position;
            
            // calc Force
            var forceElastic = 1f / TrackPower;

            // calc Speed
            var newPos = nowPos;
            for (int index = 0; index < 2; ++index) {
                var speed = _CurrentSpeed[index];
                newPos[index] = Mathf.SmoothDamp(nowPos[index], TargetPos[index], ref speed, 
                    forceElastic, float.PositiveInfinity, dt);

                _CurrentSpeed[index] = speed;
            }

            Position = newPos;
        }
    }
}