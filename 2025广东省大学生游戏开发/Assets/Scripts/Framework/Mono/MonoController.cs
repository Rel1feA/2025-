using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoController : Singleton<MonoController>
{
    public event UnityAction updateEvent;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(updateEvent != null)
        {
            updateEvent();
        }
    }

    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }

    public void RemoveUpdateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }

    public Coroutine MonoStartCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }
}
