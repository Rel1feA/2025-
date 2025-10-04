using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoolData
{
    public GameObject fatherObj;
    public List<GameObject> poolList;

    public PoolData(GameObject obj,GameObject poolObj)
    {
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform;
        poolList = new List<GameObject>();
        PushObj(obj);
    }

    public GameObject GetObj()
    {
        GameObject obj = null;
        obj = poolList[0];
        poolList.RemoveAt(0);
        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }

    public void PushObj(GameObject obj)
    {
        obj.SetActive(false);
        poolList.Add(obj);
        obj.transform.parent= fatherObj.transform;
    }
}


public class PoolManager : Singleton<PoolManager>
{
    public Dictionary<string,PoolData> poolDic = new Dictionary<string,PoolData>();
    private GameObject poolObj;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void GetObj(string name,UnityAction<GameObject> callback)
    {
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count>0)
        {
            callback(poolDic[name].GetObj());
        }
        else
        {
            ResourcesManager.Instance.LoadAsync<GameObject>(name, ((o) =>
            {
                o.name = name;
                callback(o);
            }));
        }
    }

    public void PushObj(string name,GameObject obj)
    {
        if (poolObj == null)
        {
            poolObj = new GameObject("Pool");
        }
        if(poolDic.ContainsKey(name))
        {
            poolDic[name].PushObj(obj);
        }
        else
        {
            poolDic.Add(name, new PoolData(obj,poolObj));
        }
    }

    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
