using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Enum.Sound;

[CreateAssetMenu(fileName ="ComboContainerData",menuName ="Create/Asset/CoomboContainerData")]
public class ComboContainerData : ScriptableObject
{
    [SerializeField] public List<ComboData> comboDates = new List<ComboData>();
    [SerializeField, Header("闪避A")] public ComboData DodgeAttackData;
    [SerializeField, Header("后闪避A")] public ComboData BackDodgeAttackData;

    private ComboData firstComboData;
    
    public void Init()
    {
        if (comboDates.Count == 0) { return; }
        //获取第一个连击
        firstComboData = comboDates[0];
        Debug.Log("初始化");
    }

    public string GetComboName(int index)
    {
        if (comboDates.Count == 0) { return null; }
        if (comboDates[index].comboName == null) { Debug.LogWarning(index + "连击数据没有连击名"); }
        return comboDates[index].comboName;
    }
  
    public void SwitchDodgeATK()
    { 
        if (DodgeAttackData==null) { return; }
        comboDates[0]= DodgeAttackData;
    }
    public void SwitchBackDodgeATK()
    {
        if (BackDodgeAttackData == null) { return; }
        comboDates[0] = BackDodgeAttackData;
    }
    public void ResetComboDates()
    {
        if (comboDates == null) { Debug.Log(comboDates + "是空的"); return; }
        if (comboDates[0] != firstComboData)
        {
            comboDates[0] = firstComboData;
            Debug.Log("切换的连击为" + comboDates[0].name);
        }
    }
    public float GetComboColdTime(int index)
    {
        if (comboDates.Count == 0) { return 0; }
        if (comboDates[index].comboColdTime == 0) { Debug.LogWarning(index + "连击数据没有连击的冷却时间"); }
        return comboDates[index].comboColdTime;
    }

    public float GetComboDistance(int index)
    {
        if (comboDates.Count == 0) { return 0; }
        if (comboDates[index].attackDistance== 0) { Debug.LogWarning(index + "连击数据没有连击的攻击距离"); }
        return comboDates[index].attackDistance;
    }
    public float GetComboOffset(int index)
    {
        if (comboDates.Count == 0) { return 0; }
        if (comboDates[index].comboOffset == 0) { Debug.LogWarning(index + "连击数据没有连击的攻击偏移量"); }
        return comboDates[index].comboOffset;
    }
    //public AudioClip  GetComboSound(int index)
    //{
    //    if (comboDates.Count == 0) { return null; }
    //    if (comboDates[index].weaponSound == null) { Debug.LogWarning(index + "连击数据没有连击音效"); }
    //    return comboDates[index].weaponSound;
    //}
    //public AudioClip GetCharacterVoice(int index)
    //{
    //    if (comboDates.Count == 0) { return null; }
    //    if (comboDates[index].characterVoice == null) { Debug.LogWarning(index + "连击数据没有角色语音"); }
    //    return comboDates[index].characterVoice;
    //}
    //public GameObject GetCharacterVoicePrefab(int index)
    //{
    //    if (comboDates.Count == 0) { return null; }
    //    if (comboDates[index].characterVoicePrefab == null) { Debug.LogWarning(index + "连击数据没有角色语音预制件"); }
    //    return comboDates[index].characterVoicePrefab;
    //}

    //public GameObject GetComboSoundPrefab(int index)
    //{
    //    if (comboDates.Count == 0) { return null; }
    //    if (comboDates[index].weaponSoundPrefab == null) { Debug.LogWarning(index + "连击数据没有武器音效预制件"); }
    //    return comboDates[index].weaponSoundPrefab;
    //}

    public string GetComboHitName(int index)
    {
        if (comboDates.Count == 0) { return null; }
        if (comboDates[index].hitName == null) { Debug.LogWarning(index + "连击数据没有命中名称"); }
        return comboDates[index].hitName;
    }
    public string GetComboParryName(int index)
    {
        if (comboDates.Count == 0) { return null; }
        if (comboDates[index].parryName == null) { Debug.LogWarning(index + "连击数据没有格挡名称"); }
        return comboDates[index].parryName;
    }
    public int GetComboMaxCount()
    {
        if (comboDates.Count == 0) { return 0; }
        return comboDates.Count;
    } 
    public float GetComboDamage(int index)
    { 
        if(comboDates.Count == 0) { return 0f; }
        if (comboDates[index].comboDamage == 0) { Debug.LogWarning(index + "连击数据没有伤害"); }
        return comboDates[index].comboDamage;
    }

    public SoundStyle GetComboSoundStyle(int index)
    {
      
        if (comboDates[index].comboDamage == 0) { Debug.LogWarning(index + "连击数据没有通用音效Style"); }
        return comboDates[index].universalSound;
    }

    public float GetComboShakeForce(int index, int attackIndex)
    {
        // Debug.Log("ATKIndex为" + ATKIndex);
        // Debug.Log("comboDates[index].shakeForce.Length为" + (comboDates[index].shakeForce.Length ));
      
        if (comboDates[index].shakeForce == null||attackIndex >= comboDates[index].shakeForce.Length )
        {
         // 说明没有设置 Force 或者没有设置全 Force 数组，每个 ATK 都没有设置
            return 0;
        }
        return comboDates[index].shakeForce[attackIndex - 1];
    }
   
    public int GetComboATKCount(int index)
    {
        return comboDates[index].attackCount;
    }
    public float GetPauseFrameTime(int index,int attackIndex)
    {
        if (comboDates[index].pauseFrameTimeList== null|| attackIndex >= comboDates[index].pauseFrameTimeList.Length)
        {
           return GetComboPauseFrameTime(index);
        }
       
        return comboDates[index].pauseFrameTimeList[attackIndex - 1];

    }
    private float GetComboPauseFrameTime(int index)
    {
        return comboDates[index].pauseFrameTime;
    }
}
