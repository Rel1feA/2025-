using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesManager : NormalSingleton<ResourcesManager>
{
    //同步加载资源
    public T Load<T>(string path)where T:Object
    {
        T res = Resources.Load<T>(path);
        if(res is GameObject)
        {
            return GameObject.Instantiate(res);
        }
        else
        {
            return res;
        }
    }


    //异步加载资源
    public void LoadAsync<T>(string path,UnityAction<T> callback) where T:Object
    {
        MonoController.Instance.StartCoroutine(ReallyLoadAsync(path, callback));
    }

    private IEnumerator ReallyLoadAsync<T>(string path,UnityAction<T> callback) where T:Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(path);
        yield return r;
        if(r.asset is GameObject)
        {
            callback(GameObject.Instantiate(r.asset) as T);
        }
        else
        {
            callback(r.asset as T);
        }
    }
}
