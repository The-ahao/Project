
using System.Collections.Generic;
using UnityEngine;
using ZZZ;
using Enum.Sound;
using Enum.Combo;

[CreateAssetMenu(fileName ="ComboData",menuName ="Create/Asset/ComboData")]
public class ComboData : ScriptableObject
{
    [SerializeField, Header("角色名列表")] public CharacterNameList characterName1;
    [SerializeField, Header("攻击类型")] private AttackStyle attackStyle1;
    [SerializeField] private string comboName1;
    [SerializeField] private float comboColdTime1;
    [SerializeField] private float comboDamage1;
    [SerializeField] private float attackDistance1;
    [SerializeField] private float comboOffset1;
    [SerializeField] private string[] hitName1;
    [SerializeField] private string[] parryName1;
    [SerializeField, Header("效果配置")] private bool appAudioPrefab1 = false;
    [SerializeField] private AudioClip[] weaponSound1;
    [SerializeField] private AudioClip[] characterVoice1;
    [SerializeField, Header("通用音效")] private SoundStyle universalSound1;
    [SerializeField, Header("战斗参数")]private int attackCount1;
    [SerializeField] private float pauseFrameTime1;
    [SerializeField] private float[] shakeForceList1;
    [SerializeField] private float[] pauseFrameTimeList1;


    #region 属性包装
    public AttackStyle attackStyle => attackStyle1;
    public string comboName => comboName1;
    public float comboDamage => comboDamage1;
    public float comboColdTime => comboColdTime1;
    public float attackDistance => attackDistance1;
    public float comboOffset => comboOffset1;
    public AudioClip[] weaponSound => weaponSound1;
    public AudioClip[] characterVoice => characterVoice1;
    public string hitName => hitName1[Random.Range(0, hitName1.Length)];
    public string parryName => parryName1[Random.Range(0, parryName1.Length)];
    public float[] shakeForce => shakeForceList1;
    public SoundStyle universalSound => universalSound1;
    public float pauseFrameTime => pauseFrameTime1;
    public float[] pauseFrameTimeList => pauseFrameTimeList1;
    public int attackCount => attackCount1;
    public bool appAudioPrefab => appAudioPrefab1;
    #endregion
}
