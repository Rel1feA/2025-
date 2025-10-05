using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadSceneManager.Instance.LoadSceneAsync("StartScene", null, null);
    }
}
