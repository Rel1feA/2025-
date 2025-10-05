using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Player p1;
    private Player p2;
    private int p1Score;
    private int p2Score;
    private float gTime;


    public Transform p1StartPos;
    public Transform p2StartPos;

    public Sprite p1WinSprite;
    public Sprite p2WinSprite;

    public float gameTime;//一局游戏持续多少秒
    public float playerRelifeTime;//玩家重生时间

    public GameObject itemObj;//玩家死亡时掉落的背包豆子预制件

    private void OnEnable()
    {
        EventCenter.Instance.AddListener<Player>("PlayerDead", OnPlayerDead);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveListener<Player>("PlayerDead", OnPlayerDead);
    }

    private void Start()
    {
        UIManager.Instance.ShowPanel<ScorePanel>("ScorePanel");
        CreatePlayer();
        gTime = gameTime;
    }

    private void Update()
    {
        gTime -= Time.deltaTime;
        if(gTime<=0)
        {
            GameOver();
            gTime = gameTime;
        }
    }

    public void CreatePlayer()
    {
        ResourcesManager.Instance.LoadAsync<Player>("Prefabs/P1", (player)=>
        {
            p1 = player;
            Instantiate(p1.gameObject);
            p1.transform.position = p1StartPos.position;
        });
        ResourcesManager.Instance.LoadAsync<Player>("Prefabs/P2", (player) =>
        {
            p2 = player;
            Instantiate(p2.gameObject);
            p2.transform.position = p2StartPos.position;
        });
    }

    public void ChangeScore(Player player,int val)
    {
        switch (player.playerType)
        {
            case PlayerType.P1:
                p1Score += val;
                break;
            case PlayerType.P2:
                p2Score += val;
                break;
        }
        EventCenter.Instance.EventTrigger<Player>("PlayerGetScore", player);
    }

    public int GetScore(Player player)
    {
        switch (player.playerType)
        {
            case PlayerType.P1:
                return p1Score;
            case PlayerType.P2:
                return p2Score;
        }
        return 0;
    }

    public void OnPlayerDead(Player player)
    {
        if(player.GetScore() > 0)
        {
            CreateBigBean(player);
        }
        StartCoroutine(IRel1fePlayer(player));
    }

    public void CreateBigBean(Player player)
    {
        ResourcesManager.Instance.LoadAsync<GameObject>("Prefabs/BigBean", (obj) =>
        {
            obj.transform.position=player.transform.position;
            obj.GetComponent<Beans>().score = player.GetScore();
            player.ChangeScore(-player.GetScore());
        });
    }

    public void RelifePlayer(Player player)
    {
        switch (player.playerType)
        {
            case PlayerType.P1:
                player.transform.position=p1StartPos.position;
                break;
            case PlayerType.P2:
                player.transform.position=p2StartPos.position;
                break;
        }
        player.gameObject.SetActive(true);
    }

    public IEnumerator IRel1fePlayer(Player player)
    {
        yield return new WaitForSeconds(playerRelifeTime);
        RelifePlayer(player);
    }

    public int GetGTime()
    {
        return (int)gTime;
    }

    public void GameOver()
    {
        if(p1Score>=p2Score)
        {
            UIManager.Instance.ShowPanel<WinPanel>("WinPanel", E_UI_Layer.System, (panel) =>
            {
                panel.SetImg(p1WinSprite);
            });
        }
        else
        {
            UIManager.Instance.ShowPanel<WinPanel>("WinPanel", E_UI_Layer.System, (panel) =>
            {
                panel.SetImg(p2WinSprite);
            });
        }
        AudioManager.Instance.StopAudio("BGM");
        AudioManager.Instance.PlayAudio("Win");
        Time.timeScale = 0;
        StartCoroutine(IBackToMenu(6f));
    }

    public void BackToMenu()
    {
        AudioManager.Instance.PlayAudio("BGM");
        UIManager.Instance.HidePanel("ScorePanel");
        UIManager.Instance.HidePanel("WinPanel");
        LoadSceneManager.Instance.LoadSceneAsync("StartScene", null, null);
        Time.timeScale = 1;
    }

    public IEnumerator IBackToMenu(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        BackToMenu();
    }
}
