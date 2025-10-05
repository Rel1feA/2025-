using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : BasePanel
{
    private void OnEnable()
    {
        EventCenter.Instance.AddListener<Player>("PlayerGetScore", UpdateScoreText);
        EventCenter.Instance.AddListener<Player>("PlayerScoreChange", UpdateBulletText);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveListener<Player>("PlayerGetScore", UpdateScoreText);
        EventCenter.Instance.RemoveListener<Player>("PlayerScoreChange", UpdateBulletText);
    }

    private void Update()
    {
        GetControl<Text>("TimeTXT").text = GameManager.Instance.GetGTime().ToString();
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

    public void UpdateBulletText(Player player)
    {
        switch (player.playerType)
        {
            case PlayerType.P1:
                GetControl<Text>("P1BT").text = (player.GetScore()/10).ToString();
                break;
            case PlayerType.P2:
                GetControl<Text>("P2BT").text = (player.GetScore()/10).ToString();
                break;
        }
    }

}
