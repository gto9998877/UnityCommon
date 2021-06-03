using UnityEngine;
using Vee.UnityExtend;

namespace Vee
{
    public class MonoSingletonHelper {
        public const string RootGameObjectName = "MonoSingletonRoot";

        public static GameObject GetRoot() {
            var root = GameObject.Find(RootGameObjectName);
            if (root == null) {
                root = CreateRootGameObject();
            }

            return root;
        }
        static GameObject CreateRootGameObject () {
            var go = new GameObject(RootGameObjectName);
            go.AddComponent<DontDestroyObj>();
            return go;
        }
    }
    
    public abstract class MonoSingleton<T> : MonoBehaviour
        where T : MonoSingleton<T>
    {
        protected static bool _isDestroied;
        protected static T m_instance;

        public static T instance
        {
            get
            {
                if (!m_instance && !_isDestroied)
                {
                    m_instance = FindObjectOfType<T>();
                    if (m_instance == null)
                    {
                        var go = new GameObject(typeof(T).Name);
                        m_instance = go.AddComponent<T>();
                        var parent = MonoSingletonHelper.GetRoot();
                        if (parent != null)
                        {
                            go.transform.SetParent(parent.transform);
                        }
                    }
                }

                return m_instance;
            }
        }

        public void Startup()
        {

        }

        protected virtual void Awake()
        {
            // 已经有实例存在了，删除新的
            if (m_instance != null && m_instance != this) {
                var scriptName = name;
                var gameObjectName = gameObject.name;
                // Debug.LogWarning($"MonoSingleton is duplicated, gameobject name [{gameObjectName}], script name [{scriptName}]");
                enabled = false;
                gameObject.SetActive(false);
                this.DestroyGameObject(true);
                return;
            }
            
            if (m_instance == null)
            {
                m_instance = this as T;
            }

            DontDestroyOnLoad(gameObject);
            Init();
        }

        public virtual void Init()
        {

        }
        
        protected virtual void OnDestroy()
        {
            if (m_instance != null && m_instance == this) {
                m_instance = null;
                _isDestroied = true;   
            }
        }
    }
}