
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Base;

namespace ZZZ
{
    public class SwitchCharacter : Singleton<SwitchCharacter>
    {
        [System.Serializable]
        public class SwitchCharacterInfo
        {
            public CharacterNameList characterName;
            public Transform aimAtPos;
            public Transform followAtPos;
            public GameObject character;
            public float spawnDistance;
            [HideInInspector]
            public Animator animator;
        }
        [SerializeField] private List<SwitchCharacterInfo> switchCharacterInfos = new List<SwitchCharacterInfo>();
        [SerializeField] public List<CharacterNameList> waitingCharacterList = new List<CharacterNameList>();
        private bool canSwitchInput;
        [SerializeField, Header("切换角色的冷却时间")] private float applyNextSwitchTime;
        [SerializeField, Header("退出角色的存在时间")] private float switchOutCharacterTime;
        [SerializeField] public CharacterNameList currentCharacterName;//上一个角色
        [SerializeField] public BindableProperty<CharacterNameList> newCharacterName = new BindableProperty<CharacterNameList>();
        [SerializeField] private GameObject currentCharacter;
        [SerializeField] public GameObject newCharacter;
        [SerializeField] private CinemachineVirtualCamera[] virtualCameras;
        private int characterIndex = 0;
        protected override void Awake()
        {
            base.Awake();

            InitCharacter();
            InitCamera();
           
        }
        private void OnEnable()
        {
            newCharacterName.OnValueChanged += OnSetWaitingCharacterList;
        }
        private void OnDisable()
        {
            newCharacterName.OnValueChanged -= OnSetWaitingCharacterList;
        }

        private void OnSetWaitingCharacterList(CharacterNameList list)
        {
            if (waitingCharacterList.Contains(list))
            {
                waitingCharacterList.Remove(list);
            }
            else
            {
                Debug.LogWarning(list + "该角色不在等待列表中无法移除");
            }
            if (currentCharacterName != list)//注意开始List和开始的默认CurrentCharacterName不同
            {
                if (!waitingCharacterList.Contains(currentCharacterName))
                {
                    waitingCharacterList.Add(currentCharacterName);
                }
               
            }
        }

        private void InitCamera()
        {
            virtualCameras = FindObjectsOfType<CinemachineVirtualCamera>();
        }

        private void InitCharacter()
        {
            for (int i = 0; i < switchCharacterInfos.Count; i++)
            {
                switchCharacterInfos[i].animator = switchCharacterInfos[i].character.transform.GetComponent<Animator>();
                waitingCharacterList.Add(switchCharacterInfos[i].characterName);
            }
            //初始化默认角色
            newCharacterName.Value = CharacterNameList.AnBi;
        }

        protected void Start()
        { 
            canSwitchInput = true;
            //初始化相机
            SwitchCharacterInfo initCharacterInfo = switchCharacterInfos.Find(i => i.characterName == newCharacterName.Value);
            SwitchCamerasTarget(initCharacterInfo.aimAtPos,initCharacterInfo.followAtPos);
        }
        
        /// <summary>
        /// 外部调用的接口
        /// </summary>
        public void SwitchInput()
        {
            Debug.Log("切换角色输入" + canSwitchInput);
            if (!canSwitchInput) return;
            canSwitchInput = false;
            currentCharacterName = newCharacterName.Value;
            newCharacterName.Value = UpdateCharacter();
            ExecuteSwitchCharacter(newCharacterName.Value,false);
            TimerManager.MainInstance.GetOneTimer(applyNextSwitchTime, ApplyNextSwitch);
        }
        public void SwitchSkillInput(CharacterNameList SwitchInCharacter,string SwitchInSkillName)
        {
            currentCharacterName = newCharacterName.Value;

            newCharacterName.Value = SwitchInCharacter;
             
            ExecuteSwitchCharacter(newCharacterName.Value,true, SwitchInSkillName);
            UpdateNewCharacterIndex(SwitchInCharacter);//更新新角色的索引值
        }
        private void UpdateNewCharacterIndex(CharacterNameList characterName)
        {
            for (int i = 0; i < switchCharacterInfos.Count; i++)
            {
                if (switchCharacterInfos[i].characterName == characterName)
                { 
                   characterIndex = i; return;
                }
            }
        }

        private CharacterNameList UpdateCharacter()
        {
            characterIndex++;
            characterIndex %= switchCharacterInfos.Count;
            return switchCharacterInfos[characterIndex].characterName;
        }

        Coroutine switchOutCharacterTimeCoroutine;
         public void ExecuteSwitchCharacter(CharacterNameList newCharacterName, bool isSwitchATK, string SwitchInAnimation = "SwitchIn", string SwitchOutAnimation = "SwitchOut")
        {
            SwitchCharacterInfo currentCharacterInfo = switchCharacterInfos.Find(i => i.characterName == currentCharacterName);//从列表中找到某个元素，找到那个列表里
            if (currentCharacterInfo != null)
            {
                currentCharacter = currentCharacterInfo.character;

                currentCharacterInfo.animator.CrossFadeInFixedTime(SwitchOutAnimation, 0.1f);
            }

            SwitchCharacterInfo newCharacterInfo = switchCharacterInfos.Find(i => i.characterName == newCharacterName);

            if (newCharacterInfo != null)
            {
                newCharacter = newCharacterInfo.character;

                newCharacter.SetActive(false);
                if (!isSwitchATK)
                {
                    newCharacter.transform.position = currentCharacter.transform.position - currentCharacter.transform.forward * newCharacterInfo.spawnDistance - currentCharacter.transform.right * 0.6f;


                }
                else
                { 
                    //如果是攻击的话，那么生成位置默认为敌人-当前角色前方*3
                   newCharacter.transform.position=GameBlackboard.MainInstance.GetEnemy().position-currentCharacter.transform.forward*3;
                }

                newCharacter.transform.localRotation = currentCharacter.transform.localRotation;

                newCharacter.SetActive(true);

               
                newCharacterInfo.animator.Play(SwitchInAnimation);

                SwitchCamerasTarget(newCharacterInfo.aimAtPos, newCharacterInfo.followAtPos);
             
            }
            if (switchOutCharacterTimeCoroutine != null)
            {
                StopCoroutine(switchOutCharacterTimeCoroutine);
            }
            switchOutCharacterTimeCoroutine = StartCoroutine(CharacterActiveTimerCoroutine(switchOutCharacterTime));

            
        }


        IEnumerator CharacterActiveTimerCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            SetCharacterActive();
        }
        private void SetCharacterActive()
        {
            for (int i = 0; i < switchCharacterInfos.Count; i++)
            {
                if (switchCharacterInfos[i].characterName != newCharacterName.Value)
                {
                    switchCharacterInfos[i].character.SetActive(false);
                }
            }
        }
        private void ApplyNextSwitch()
        {
            canSwitchInput = true;
        }
        private void SwitchCamerasTarget(Transform aimPos,Transform followPos)
        {
            for (int i = 0; i < virtualCameras.Length; i++)
            {
                if (virtualCameras[i].gameObject.tag != "CloseShot")
                {
                    //根据新的角色切换，更新相机
                    virtualCameras[i].LookAt = aimPos;
                    virtualCameras[i].Follow = followPos;
                }
            }
        }
    }
}
