
using System.Collections.Generic;
using System;
using Event;
using ZZZ.Tool;
// 事件中心.提供注册事件，触发事件，移除事件
public class GameEventsManager : SingletonEvent<GameEventsManager>
{
    private interface IEventface{}
    private class EventHander : IEventface
    {
        //注册事件
        private event Action act;
        //提供调用
        public EventHander(Action action)
        {
            act = action;//构造函数需要为字段初始化，不需要为null
        }
        //添加回调
        public void AddCallBack(Action action)
        {
            act += action;
        }
        //移除回调
        public void RemoveCallBack(Action action)
        {
            act -= action;
        }
        //触发回调
        public void CallBack()
        {
            act?.Invoke();
        }
    }
    private class EventHander<T> : IEventface
    {
        //注册事件
        private event Action<T> act;
        //提供调用的方法
        public EventHander(Action<T> action)
        {
            act = action;
        }

        public void AddCallBack(Action<T> action)
        {
            act += action;
        }

        public void RemoveCallBack(Action<T> action)
        {
            act -= action;
        }
        public void CallBack(T value)
        {
            act?.Invoke(value);
        }
    }
    private class EventHander<T1,T2> : IEventface
    {
        //注册事件
        private event Action<T1, T2> act;
        //提供调用的方法
        public EventHander(Action<T1,T2> action)
        {
            act = action;
        }

        public void AddCallBack(Action<T1, T2> action)
        {
            act += action;
        }

        public void RemoveCallBack(Action<T1, T2> action)
        {
            act -= action;
        }
        public void CallBack(T1 t1 ,T2 t2)
        {
            act?.Invoke(t1,t2);
        }

    }
    private class EventHander<T1, T2, T3, T4, T5,T6> : IEventface
    {
        //注册事件
        private event Action<T1, T2, T3, T4, T5,T6> act;
        //提供调用的方法
        public EventHander(Action<T1, T2, T3, T4, T5, T6> action)
        {
            act = action;
        }

        public void AddCallBack(Action<T1, T2, T3, T4, T5, T6> action)
        {
            act += action;
        }

        public void RemoveCallBack(Action<T1, T2, T3, T4, T5, T6> action)
        {
            act -= action;
        }
        public void CallBack(T1 t1, T2 t2, T3 t3,T4 t4, T5 t5,T6 t6)
        {
            act?.Invoke(t1, t2, t3, t4, t5,t6);
        }

    }

    //字典存储事件中心，静态的
    private Dictionary<string, IEventface> EventCenters = new Dictionary<string, IEventface>();

    //注册事件用EventHander(action)写到字典中，添加回调
    public void AddEventListening(string name, Action action)
    {
        if (EventCenters.TryGetValue(name, out var e))
        {
            (e as EventHander)?.AddCallBack(action);
        }
        else
        {
            EventCenters.Add(name, new EventHander(action));
        }

    }
    public void AddEventListening<T>(string name, Action<T> action)
    {
        if (EventCenters.TryGetValue(name, out var e))
        {
            (e as EventHander<T>)?.AddCallBack(action);
        }
        else
        {
            EventCenters.Add(name, new EventHander<T>(action));
        }

    }
    public void AddEventListening<T1, T2>(string name, Action<T1, T2> action)
    {
        if (EventCenters.TryGetValue(name, out var e))
        {
            (e as EventHander<T1, T2>)?.AddCallBack(action);
        }
        else
        {
            EventCenters.Add(name, new EventHander<T1, T2>(action));
        }

    }
    public void AddEventListening<T1, T2, T3, T4, T5,T6>(string name, Action<T1, T2, T3, T4, T5, T6> action)
    {
        if (EventCenters.TryGetValue(name, out var e))
        {
            (e as EventHander<T1, T2, T3, T4, T5, T6>)?.AddCallBack(action);
        }
        else
        {
            EventCenters.Add(name, new EventHander<T1, T2, T3, T4, T5, T6>(action));
        }

    }
    public void CallEvent(string name)
    {
        if (EventCenters.TryGetValue(name, out var e))
        {
            (e as EventHander)?.CallBack();
        }
        else
        {
            DevelopmentToos.WTF("当前事件中心不存在");
        }
    }

    public void CallEvent<T>(string name, T value)
    {
        if (EventCenters.TryGetValue(name, out var e))
        {
            (e as EventHander<T>)?.CallBack(value);
        }
        else
        {
            DevelopmentToos.WTF("当前事件中心不存在");
        }
    }
    public void CallEvent<T1,T2>(string name, T1 t1,T2 t2)
    {
        if (EventCenters.TryGetValue(name, out var e))
        {
            (e as EventHander<T1, T2>)?.CallBack(t1,t2);
        }
        else
        {
            DevelopmentToos.WTF("当前事件中心不存在");
        }
    }
    public void CallEvent<T1, T2, T3, T4, T5, T6>(string name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5,T6 t6)
    {
        if (EventCenters.TryGetValue(name, out var e))
        {
            (e as EventHander<T1,T2,T3,T4,T5,T6>)?.CallBack(t1,t2,t3,t4,t5, t6);
        }
        else
        {
            DevelopmentToos.WTF("当前事件中心不存在");
        }
    }
    public void RemoveEvent(string name,Action action)
    {
        if (EventCenters.TryGetValue(name, out var e))
        {
            (e as EventHander)?.RemoveCallBack(action);
        }
        else
        {
            DevelopmentToos.WTF("当前事件中心不存在");
        }
        
    }
    public void RemoveEvent<T1>(string name, Action<T1> action)
    {
        if (EventCenters.TryGetValue(name, out var e))
        {
            (e as EventHander<T1>)?.RemoveCallBack(action);
        }
        else
        {
            DevelopmentToos.WTF("当前事件中心不存在");
        }

    }
    public void RemoveEvent<T1,T2>(string name, Action<T1,T2> action)
    {
        if (EventCenters.TryGetValue(name, out var e))
        {
            (e as EventHander<T1, T2>)?.RemoveCallBack(action);
        }
        else
        {
            DevelopmentToos.WTF("当前事件中心不存在");
        }

    }
    public void RemoveEvent<T1, T2, T3, T4, T5, T6>(string name, Action<T1, T2, T3, T4, T5, T6> action)
    {
        if (EventCenters.TryGetValue(name, out var e))
        {
            (e as EventHander<T1, T2, T3, T4, T5, T6>)?.RemoveCallBack(action);
        }
        else
        {
            DevelopmentToos.WTF("当前事件中心不存在");
        }

    }
}
