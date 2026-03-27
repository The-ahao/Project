using Cinemachine;
using System.Collections.Generic;
using ZZZ;
using UnityEngine;
using Base;
using Enum.Combo;

public class CameraSwitcher : Singleton<CameraSwitcher>
{
    private CinemachineBrain brain;
    [SerializeField, Header("切换角色时相机")] private CinemachineVirtualCamera switchCharacterSkillCamera;
    [System.Serializable]
    public class CharacterStateCameraInfo
    {
       public CharacterNameList characterName;
       public List<StateCameraInfo> stateCameraList=new List<StateCameraInfo>();
    }
    [System.Serializable]
    public class StateCameraInfo
    {
        public AttackStyle AttackStyle;
        public CinemachineStateDrivenCamera stateCamera;
    }
    [SerializeField,Header("状态相机信息")] private List<CharacterStateCameraInfo> stateCameraInfoList = new List<CharacterStateCameraInfo>();

    private Dictionary<CharacterNameList, Dictionary<AttackStyle, CinemachineStateDrivenCamera>> stateCameraPool = new Dictionary<CharacterNameList, Dictionary<AttackStyle, CinemachineStateDrivenCamera>>();
    //这种数据结构可以这样实现：键字典里写一个字典；值字典里写自定义类数据结构。对于第二种指定，如果没有值就省略。
    
    
    protected override void Awake()
    {
        base.Awake();
        brain =Camera.main.GetComponent<CinemachineBrain>();
        
    }
    private void Start()
    {
        InitSwitchCamera();
        InitSkillCamera();
    }

    private void InitSwitchCamera()
    {
        if (stateCameraInfoList.Count == 0) { return; }
        for (int i = 0; i < stateCameraInfoList.Count; i++)
        {
            if (stateCameraInfoList[i].stateCameraList.Count == 0) { continue; }//跳过当前元素
            stateCameraPool.Add(stateCameraInfoList[i].characterName, new Dictionary<AttackStyle, CinemachineStateDrivenCamera>());
            for (int j = 0; j < stateCameraInfoList[i].stateCameraList.Count; j++)
            {
                stateCameraInfoList[i].stateCameraList[j].stateCamera.Priority = 0;
                //加入到字典里
                stateCameraPool[stateCameraInfoList[i].characterName].Add(stateCameraInfoList[i].stateCameraList[j].AttackStyle, stateCameraInfoList[i].stateCameraList[j].stateCamera);
            }
        }
    }

    private void InitSkillCamera()
    {
        switchCharacterSkillCamera.Priority = 0;
    }
    public void ActiveStateCamera(CharacterNameList characterName,AttackStyle attackStyle)
    {
        if (stateCameraPool.TryGetValue(characterName, out var stateCameraList))
        {
            //然后从列表里找到并获取需要的元素
            if (stateCameraList.TryGetValue(attackStyle, out var stateDrivenCamera))
            {
                stateDrivenCamera.Priority = 20;
            }
            
        }
    }
    public void UnActiveStateCamera(CharacterNameList characterName, AttackStyle attackStyle)
    {
        if (stateCameraPool.TryGetValue(characterName, out var stateCameraList))
        {
            //然后从列表里找到并获取需要的元素
            if (stateCameraList.TryGetValue(attackStyle, out var stateDrivenCamera))
            {
                stateDrivenCamera.Priority = 0;
            }

        }
    }
    public void ActiveSwitchCamera(bool applySwitchCamera)
    {
        if (applySwitchCamera)
        {
            switchCharacterSkillCamera.Priority = 20;
        }
        else
        {
            switchCharacterSkillCamera.Priority = 0;
        }
    }

    private void OnEnable()
    {
       
        brain.m_CameraActivatedEvent.AddListener(OnCameraActivated);
    }

    private void OnDisable()
    {
       
        brain.m_CameraActivatedEvent.RemoveListener(OnCameraActivated);
    }
   

   
    /// <summary>
    /// 相机切换时的处理
    /// </summary>
    /// <param name="newCamera"></param>
    /// <param name="oldCamera"></param>
    private void OnCameraActivated(ICinemachineCamera newCamera, ICinemachineCamera oldCamera)
    {
       
    }

}
