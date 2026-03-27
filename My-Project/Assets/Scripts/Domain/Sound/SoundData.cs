
using System.Collections.Generic;
using UnityEngine;
using ZZZ;
using Enum.Sound;
// 用角色和类型对音源进行标记
[CreateAssetMenu(fileName = "SoundData", menuName = "Create/Assets/SoundData")]//右键创建SoundData对象
public class SoundData : ScriptableObject
{
    //便于Inspector编辑
    [System.Serializable]
    public class SoundInfo
    {
        public SoundStyle soundStyle;
        public CharacterNameList characterName;
        public AudioClip[] clips;
    }
    [SerializeField]public List<SoundInfo> soundInfoList = new List<SoundInfo>();
    public AudioClip GetAudioClip(SoundStyle soundStyle,CharacterNameList characterName)
    {
        if (characterName == CharacterNameList.Null)
        {
            for (int i = 0; i < soundInfoList.Count; i++)
            {
                if (soundStyle == soundInfoList[i].soundStyle)
                {
                    return soundInfoList[i].clips[Random.Range(0, soundInfoList[i].clips.Length)];
                }
            }

        }
        else
        {
            SoundInfo targetSound = soundInfoList.Find(i => i.soundStyle == soundStyle && i.characterName == characterName);
            if (targetSound != null)
            {
                return targetSound.clips[Random.Range(0, targetSound.clips.Length)];
            }
        }
        return null;
       
    }
  
}
