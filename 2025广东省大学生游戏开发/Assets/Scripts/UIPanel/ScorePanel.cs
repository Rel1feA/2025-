using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : BasePanel
{
    private void OnEnable()
    {
        EventCenter.Instance.AddListener<Player>("PlayerGetScore", UpdateScoreText);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveListener<Player>("PlayerGetScore", UpdateScoreText);
    }
    
    public void UpdateScoreText(Player player)
    {
        switch (player.playerType)
        {
            case PlayerType.P1:
                GetControl<Text>("P1Score").text=GameManager.Instance.GetScore(player).ToString();
                break;
            case PlayerType.P2:
                GetControl<Text>("P2Score").text = GameManager.Instance.GetScore(player).ToString();
                break;
        }
    }
}
