using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : BasePanel
{
    private void OnEnable()
    {
        EventCenter.Instance.AddListener<int>("Player1ScoreChange", UpdateP1ScoreText);
        EventCenter.Instance.AddListener<int>("Player2ScoreChange", UpdateP2ScoreText);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveListener<int>("Player1ScoreChange", UpdateP1ScoreText);
        EventCenter.Instance.RemoveListener<int>("Player2ScoreChange", UpdateP2ScoreText);
    }

    public void UpdateP1ScoreText(int score)
    {
        GetControl<Text>("P1Score").text = score.ToString();
    }

    public void UpdateP2ScoreText(int score)
    {
        GetControl<Text>("P2Score").text = score.ToString();
    }
}
