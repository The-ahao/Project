
using UnityEngine;
using Enum.Sound;

public class ComboSFXtem : PoolItemBase
{
    [SerializeField]private ComboData comboData;
    private AudioSource audioSource;
    [SerializeField] private SoundStyle soundStyle;
    public void SetSoundStyle(SoundStyle soundStyle)
    {
        this.soundStyle =soundStyle;
    }
    public void GetComboData(ComboData comboData)
    { 
      this.comboData = comboData;
    }
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();  
    }
    protected override void Spawn()
    {
        base.Spawn();//保证初始化逻辑完整
       
        if (soundStyle == SoundStyle.ComboVoice)
        {
            if (comboData.characterVoice == null || comboData.characterVoice.Length == 0) return;
            audioSource.clip = comboData.characterVoice[Random.Range(0, comboData.characterVoice.Length)];
        }
        else if (soundStyle == SoundStyle.WeaponSound)
        {
            if (comboData.weaponSound == null || comboData.weaponSound.Length == 0) return;
            audioSource.clip = comboData.weaponSound[Random.Range(0, comboData.weaponSound.Length)];
        }
        if (audioSource.clip == null) { return; }
        audioSource.Play();
    }
    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            StopAudioPlay();
        }
    }
    private void StopAudioPlay()
    {
      this.gameObject.SetActive(false);//回收音频
    }
}
