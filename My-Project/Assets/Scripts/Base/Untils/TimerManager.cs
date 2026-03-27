using UnityEngine;
using System;
using Base;
using System.Collections.Generic;

// 计时器管理脚本
public class TimerManager : Singleton<TimerManager>
{
    // 初始化计时器管理器
    [SerializeField, Header("定时器数量")] private int timerCount;
    // 计时器对象池
    private Queue<GameTimer> notWorkTimers = new Queue<GameTimer>();
    // 正在工作的计时器列表
    private LinkedList<GameTimer> isWorkingTimers = new LinkedList<GameTimer>();

    // 限制外部访问保留子类重写
    protected void Start()
    {
        for (int i = 0; i < timerCount; i++)
        {
            CreateTimer();// 创建计时器对象
        }
    }

    private void CreateTimer()
    {
        GameTimer timer = new GameTimer();
        notWorkTimers.Enqueue(timer);
    }

    private void Update()
    { 
        UpdateTime();
    }
    private void UpdateTime()
    {
        if (isWorkingTimers.Count == 0) { return; }
        LinkedListNode<GameTimer> node = isWorkingTimers.Last; // 反向遍历起点
        while (node != null)
        {
            LinkedListNode<GameTimer> nextNode = node.Previous; // 先缓存下一个节点（防止移除后丢失）
            GameTimer timer = node.Value;
            if (timer.TimerState == TimerState.ingWork)
            {
                if (!timer.IsRealTime)
                {
                    timer.UpdateTimer();
                }
                else
                {
                    timer.UpdateScaleTimer();
                }
            }
            else if (timer.TimerState == TimerState.doneWork)
            {
                timer.Init();
                notWorkTimers.Enqueue(timer);
                isWorkingTimers.Remove(node); // LinkedList.Remove(node) 是O(1)
            }
            node = nextNode;
        }
    }
    // 不受scaleTime影响的计时器
    public void GetOneTimer(float timer, Action action)
    {
        if (notWorkTimers.Count == 0) { CreateTimer(); }
        GameTimer gameTimer = null;
        gameTimer = notWorkTimers.Dequeue();// 取出队首的闲置计时器对象
        gameTimer.OpTimer(false, action, timer);// 开启计时器
        isWorkingTimers.AddLast(gameTimer);
    }
    public GameTimer GetTimer(float timer, Action action)
    {
        if (notWorkTimers.Count == 0) { CreateTimer(); }
        GameTimer gameTimer = null;
        gameTimer = notWorkTimers.Dequeue();// 取出队首的闲置计时器对象
        gameTimer.OpTimer(false, action, timer);// 开启计时器
        isWorkingTimers.AddLast(gameTimer);
        return gameTimer;
    }
    // 受scaleTime影响的计时器
    public GameTimer GetScaleTimer(float timer, Action action)
    {
        if (notWorkTimers.Count == 0) { CreateTimer(); }
        GameTimer gameTimer = null;
        gameTimer = notWorkTimers.Dequeue();// 取出队首的闲置计时器对象
        gameTimer.OpTimer(true, action, timer);// 开启计时器
        isWorkingTimers.AddLast(gameTimer);
        return gameTimer;
    }
    // 取消计时器？ 计时器对象回收
    public void CancelTimer(GameTimer timer)
    {
        if (timer == null) { return; }
        //如果是正在工作的计时器，不能被注销，因为可能已注销过相关事件。未开始的无需注销
        if (timer.TimerState != TimerState.doneWork) { return; }
        timer.Init();
        notWorkTimers.Enqueue(timer);
        isWorkingTimers.Remove(timer);
    }
}