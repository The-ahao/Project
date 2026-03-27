using System;
using UnityEngine;

// 计时状态枚举
public enum TimerState
{
    noWork,// 计时器未开始
    ingWork,// 计时器进行中
    doneWork// 计时器完成
}
// 游戏计时器类
public class GameTimer
{
    private float startTime; // 计时器开始时间
    private TimerState state; // 计时器状态
    private Action action; // 计时器完成后的回调
    private bool stopTime; // 是否暂停计时
    private bool realTime; // 是否使用真实时间

    // 获取计时状态
    public TimerState TimerState => state;
    // 获取是否使用真实时间(不受缩放影响的时间)
    public bool IsRealTime => realTime;
    // 初始化计时器
    public GameTimer()
    {
        Init();
    }
    public void Init()
    {
        startTime = 0;
        state = TimerState.noWork;
        action = null;
        stopTime = true;
        realTime = false;
    }
    // 开启计时
    public void OpTimer(bool reTime, Action callback, float stTime)
    {
        realTime = reTime;
        stopTime = false;
        action = callback;
        state = TimerState.ingWork;
        startTime = stTime;
    }
    // 计时器
    public void UpdateTimer()
    {
        if (realTime) return;
        if (stopTime == true) return;

        startTime -= Time.deltaTime;
        if (startTime <= 0)
        {
            action?.Invoke();// 事件结束执行回调任务
            stopTime = true;
            state = TimerState.doneWork;
        }
    }
    // 不受scaleTime影响的计时器
    public void UpdateScaleTimer()
    {
        if (!realTime) return;
        if (stopTime == true) return;

        startTime -= Time.unscaledDeltaTime;
        if (startTime <= 0)
        {
            action?.Invoke();// 事件结束执行回调任务
            stopTime = true;
            state = TimerState.doneWork;
        }
    }
}