
using UnityEngine;

public class EffectItem : PoolItemBase
{
    //特效时长控制，若循环需特殊处理
    [SerializeField,Header("特效播放时长")] private float playTime;

    [SerializeField, Header("特效播放速率")] private float playSpeed;

    private ParticleSystem[] ParticleSystem;

    private void Awake()
    {
        ParticleSystem =GetComponentsInChildren<ParticleSystem>();
       
        for (int i = 0; i < ParticleSystem.Length; i++) 
        {
            VFXManager.MainInstance.AddVFX(ParticleSystem[i], playSpeed);
        }
       
    }
    protected override void Spawn()
    {
        StartPlay();
    }
    private void StartPlay()
    {
        for (int i = 0;i < ParticleSystem.Length;i++) 
        {
            ParticleSystem[i].Play();
        }
      
        TimerManager.MainInstance.GetOneTimer(playTime, StartReCycle);
    }
    private void StartReCycle()
    { 
       this.gameObject.SetActive(false);  
    }
    protected override void ReSycle()
    {

        for (int i = 0; i < ParticleSystem.Length; i++)
        {
            ParticleSystem[i].Stop();
        }
    }
}
