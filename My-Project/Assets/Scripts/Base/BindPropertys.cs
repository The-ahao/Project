/*
* 观察者模式
* 封装绑定的参数，监控其变化并触发回调
*/
using System;

public class BindPropertys<T>
{
    private T value;
    public Action<T> onValueChanged;

    public T Value
    {
        get => value;
        set
        {
            if (!Equals(this.value, value))
            {
                this.value = value;
                onValueChanged?.Invoke(this.value);// 触发回调，通知监听者值已改变
            }
        }
    }
}