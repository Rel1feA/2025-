using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSingleton<T> where T:class,new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    //public static T GetInstance()
    //{
    //    if (instance == null)
    //    {
    //        instance = new T();
    //    }
    //    return instance;
    //}

}
