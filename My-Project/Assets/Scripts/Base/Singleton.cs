using UnityEngine;

namespace Base
{
    /*
    *   抽象单例工厂 用于创建系统对象
    */
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        // 系统逻辑对象实例
        public static T Instance;
        // lock对象 监视器
        public static object LockObject = new object();

        public static T MainInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (LockObject)
                    {
                        Instance = FindObjectOfType<T>() as T;
                        if (Instance == null)
                        {
                            GameObject singletonObject = new GameObject(typeof(T).Name);
                            Instance = singletonObject.AddComponent<T>();
                        }
                    }
                }
                return Instance;
            }
        }
        // 确保在Awake中创建实例，避免在其他脚本的Start中访问时出现null引用
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
