namespace ZZZ
{
    public abstract class StateMachine
    {
        //定义一个继承自BindableProperty类型的IState字段，获取Istate需要通过.Value
        public BindableProperty<IState> currentState = new BindableProperty<IState>();

        /// <summary>
        /// 切换状态的接口API
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(IState newState)
        { 
           
            //先退出当前状态，逻辑如下
            currentState.Value?.Exit();

            currentState.Value = newState;

            currentState.Value.Enter();
        }

        /// <summary>
        /// 处理输入的接口API
        /// </summary>
        public void HandInput()
        {
            //只在当前状态下处理输入
            currentState.Value?.HandInput();
        }
        /// <summary>
        /// 处理帧更新逻辑的接口API
        /// </summary>
        public void Update()
        {
            currentState.Value?.Update();
        }
        /// <summary>
        /// 执行动画转换事件的接口API
        /// </summary>
        public void OnAnimationTranslateEvent(IState translateState)
        {
            currentState.Value?.OnAnimationTranslateEvent(translateState);
        }
        public void OnAnimationExitEvent() 
        {
            currentState.Value?.OnAnimationExitEvent();
        }
    }
}
