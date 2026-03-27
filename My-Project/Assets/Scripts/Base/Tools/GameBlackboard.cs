
using UnityEngine;
using System.Collections.Generic;
using Base;

public class GameBlackboard: Singleton<GameBlackboard>
{
    //<T>表示这是一个泛型，像字段、属性、委托字段都是可以定义泛型T的，只有类、接口、结构体、方法是可以定义泛型，所以我们定义在方法返回值<T>,泛型类型参数使用    //Ŀǰ��ʹ�ù�����ɫ������
    //目前使用过角色查找
    private Dictionary<string , object> GameData = new Dictionary<string , object>();

    public BindableProperty<Transform> enemy=new BindableProperty<Transform>();
   
    public void SetEnemy(Transform Enemy)
    { 
    this.enemy.Value = Enemy; 
    }
    public Transform GetEnemy()
    {
        
      return this.enemy.Value;
    }
    
    public void SetGameData<T>(string DataName, T value )where T : class
    {
        if (GameData.ContainsKey(DataName))
        {
            GameData[DataName]=value;
        }
        else
        {
            GameData.Add(DataName, value);
        }
    
    }
    public T GetGameData<T>(string DataName)where T : class
    {
        if (GameData.TryGetValue(DataName, out var e))
        { 
        return e as T;
        }
        return default(T);
        //返回T的方法返回值是object，保证了引用类型的安全性，因为在调用该方法时需要说明指定的类型，从而直接转换为该类型
        //如果是object类型需要强制转换(转换)object对象e，如果类型不正确就会发生错误
        //如果T是一个值类型，那么返回值也应该是值类型的
        //如果T既可以为值也可以为引用，那么需要if(e is T A){return A} 进行转换，as只支持引用类型的转换
    }
}   
