
using System.Collections.Generic;
using Base;
using UnityEngine;
using Enum.Sound;

public class SFXPoolManager : Singleton<SFXPoolManager>
{
    //SoundName对应的音效池信息
    [System.Serializable]
    public class SoundItem
    {
        public SoundStyle soundStyle;
        public string soundName;   
        public GameObject soundPrefab;
        public int soundCount;
        public bool applyBigCenter;// 是否使用大池
    }
    
    [SerializeField] private List<SoundItem> soundPools = new List<SoundItem>();
    private Dictionary<SoundStyle, Queue<GameObject>> soundCenter = new Dictionary<SoundStyle, Queue<GameObject>>();
    // 大池 soundName->soundStyle->对象队列 字典模式
    private Dictionary<string, Dictionary<SoundStyle, Queue<GameObject>>> bigSoundCenter = new Dictionary<string, Dictionary<SoundStyle, Queue<GameObject>>>();
    protected override void Awake()
    {
        base.Awake();
        InitSoundPool();
    }
    private void InitSoundPool()
    {
        if (soundPools.Count == 0) { return; }
        for (int i = 0; i < soundPools.Count; i++)
        {
            if (soundPools[i].applyBigCenter)
            {
                for (int j = 0; j < soundPools[i].soundCount;j++)
                {
                    //实例化音效对象
                    var go = Instantiate(soundPools[i].soundPrefab);
                    //设置父对象
                    go.transform.parent = this.transform;
                    //设置为不激活状态
                    go.SetActive(false);
                    if (!bigSoundCenter.ContainsKey(soundPools[i].soundName))
                    {
                        Debug.Log(soundPools[i].soundName + " 音效大类池创建");
                        bigSoundCenter.Add(soundPools[i].soundName, new Dictionary<SoundStyle, Queue<GameObject>>());
                    }
                    if (!bigSoundCenter[soundPools[i].soundName].ContainsKey(soundPools[i].soundStyle))
                    {
                        bigSoundCenter[soundPools[i].soundName].Add(soundPools[i].soundStyle, new Queue<GameObject>());
                    }
                    bigSoundCenter[soundPools[i].soundName][soundPools[i].soundStyle].Enqueue(go);
                }
            }
            else
            {
                for (int j = 0; j < soundPools[i].soundCount; j++)
                {
                    //实例化音效对象
                    var go = Instantiate(soundPools[i].soundPrefab);
                    //设置父对象
                    go.transform.parent = this.transform;
                    //暂时隐藏
                    go.SetActive(false);
                    //判断是否需要新建音效池
                    if (!soundCenter.ContainsKey(soundPools[i].soundStyle))
                    {
                        //首次建池
                        soundCenter.Add(soundPools[i].soundStyle, new Queue<GameObject>());
                        //加入预制对象
                        soundCenter[soundPools[i].soundStyle].Enqueue(go);
                    }
                    else
                    {
                        //直接入队重用
                        soundCenter[soundPools[i].soundStyle].Enqueue(go);
                    }
                }
            }

        }

    }
    // 大池获取接口
    public void TryGetSoundPool(SoundStyle soundStyle, string soundName, Vector3 position)
    {
        if (bigSoundCenter.ContainsKey(soundName))
        {
            if (bigSoundCenter[soundName].TryGetValue(soundStyle, out var Q))
            {
                GameObject go = Q.Dequeue();
                go.transform.position = position;
                go.gameObject.SetActive(true);
                Q.Enqueue(go);
               // Debug.Log("音效获取成功: " + soundName + " 样式:" + soundStyle);
              
            }
            else
            {
               // Debug.LogWarning(soundStyle + " 未找到对应音效");
            }
        }
        else
        {
           // Debug.LogWarning(soundName + " 未找到该音效大类");
        }

    }
    public void TryGetSoundPool( SoundStyle soundStye, Vector3 position, Quaternion quaternion)
    {
        if (soundCenter.TryGetValue(soundStye, out var sound))
        {
           // Debug.Log(soundStye + " 音效池获取成功");
            GameObject go = sound.Dequeue();
            go.transform.position = position;
            go.gameObject.SetActive(true);
            soundCenter[soundStye].Enqueue(go);
        }
        else
        {
           // Debug.Log(soundStye + " 音效池不存在");
        }
    }
   


}
