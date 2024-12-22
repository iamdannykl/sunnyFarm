using System;
using System.Collections.Generic;

namespace SunnyFarm.code;

public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
{
    // 定义一个事件，当添加键值对时触发
    public event Action<TKey, TValue, bool> OnItemAdded;

    // 重写 Add 方法
    public void Add(TKey key, TValue value, bool isFirstAdd)
    {
        base.Add(key, value); // 调用原始的 Add 方法
        OnItemAdded?.Invoke(key, value, isFirstAdd); // 触发事件
    }

    //public void set

    // 添加或更新的方法
    public new TValue this[TKey key]
    {
        get => base[key];
        set
        {
            var isUpdate = ContainsKey(key);
            base[key] = value;
            if (isUpdate) OnItemAdded?.Invoke(key, value, false);
        }
    }
}