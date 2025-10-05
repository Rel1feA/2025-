using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPos : MonoBehaviour
{
    public PlayerType playerType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player player =collision.GetComponent<Player>();
            if(player.playerType==playerType)
            {
                GameManager.Instance.ChangeScore(player,player.GetScore());
                player.ChangeScore(-player.GetScore());
            }
        }
    }
}
