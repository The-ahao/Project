using System.Collections.Generic;
using UnityEngine;
// 音源素材
[CreateAssetMenu(fileName ="SFXData",menuName ="Asset/SFX/SFXData")]
public class SFXData : ScriptableObject
{
    [SerializeField] public List<AudioClip>  SFXList = new List<AudioClip>();
}
