using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        UIManager.Instance.ShowPanel<ScorePanel>("ScorePanel");
    }
}
