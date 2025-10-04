using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : BasePanel
{
    private void OnEnable()
    {
        EventCenter.Instance.AddListener<int>("Player1ScoreChange", UpdateP1ScoreText);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveListener<int>("Player1ScoreChange", UpdateP1ScoreText);
    }

    public void UpdateP1ScoreText(int score)
    {
        GetControl<Text>("P1Score").text = score.ToString();
    }
}
