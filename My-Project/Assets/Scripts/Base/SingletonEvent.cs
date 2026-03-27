using UnityEngine;

/*
*   抽象单例工厂 用于创建逻辑对象
*/
namespace Event
{
    public abstract class SingletonEvent<T> where T : class,new()
    {

        private static T Instance;
        private static object LockObject = new object();

        public static T MainInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (LockObject)
                    {
                        // 确保多线程情况下都能正确创建实例
                        Instance ??= new T();
                    }
                }
                return Instance;
            }
        }
    }
}
