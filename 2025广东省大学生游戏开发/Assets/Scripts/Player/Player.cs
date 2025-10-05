using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerType
{
    P1,
    P2
}

public class Player : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerKey inputActions;
    private SpriteRenderer spriteRenderer;
    public PlayerType playerType;

    [Range(-1f,0f)]
    public float lowSpeed;//吃到豆子后减少的速度
    [Range(0f, 1f)]
    public float upSpeed;//射出豆子后增加的速度

    private int score;
    private bool canShoot;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inputActions=new PlayerKey();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        InitInput();
        ResetPlayer();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        ResetPlayer();
    }

    private void Update()
    {
        movement.SetInputDir(inputActions,playerType);
        RotateSprite();
    }

    public void ResetPlayer()
    {
        score = 0;
        canShoot = true;
    }

    public void InitInput()
    {
        switch(playerType)
        {
            case PlayerType.P1:
                inputActions.Player1.Shoot.performed += Shoot;
                break;
            case PlayerType .P2:
                inputActions.Player2.Shoot.performed += Shoot;
                break;
            default:
                break;
        }
    }

    public void Shoot(InputAction.CallbackContext c)
    {
        if (!canShoot) return;
        if (score < 10) return;
        ChangeScore(-10);
        movement.ChangeSpeed(upSpeed);
        canShoot = false;
        AudioManager.Instance.PlayAudio("Fire");
        PoolManager.Instance.GetObj("Prefabs/Bullet", (obj) =>
        {
            obj.transform.position=transform.position+(Vector3)movement.GetCurDir()*0.8f;
            obj.transform.rotation=transform.rotation;
            Bullet bullet = obj.GetComponent<Bullet>();
            bullet.SetShootPlayer(this);
            bullet.dir=movement.GetCurDir();
        });
    }

    public void ChangeScore(int val)
    {
        score += val;
        if(score<0)
        {
            score = 0;
        }
        EventCenter.Instance.EventTrigger<Player>("PlayerScoreChange", this);
    }

    public void RotateSprite()
    {
        Vector2 dir= movement.GetCurDir();
        if(dir==Vector2.right)
        {
            transform.rotation = Quaternion.Euler(0,0,0);
            spriteRenderer.flipX = false;
        }
        else if(dir==Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            spriteRenderer.flipX = false;
        }
        else if( dir==Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
            spriteRenderer.flipX = false;
        }
        else if(dir == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            spriteRenderer.flipX = true;
        }
    }

    public void SetCanShoot(bool canShoot)
    {
        this.canShoot = canShoot;
    }

    public int GetScore()
    {
        return score;
    }

    public PlayerMovement GetMovement()
    {
        return movement;
    }


    public void Dead()
    {
        EventCenter.Instance.EventTrigger<Player>("PlayerDead", this);
        AudioManager.Instance.PlayAudio("Die");
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bean"))
        {
            collision.gameObject.SetActive(false);
            AudioManager.Instance.PlayAudio("EatDot");
            movement.ChangeSpeed(lowSpeed);
            ChangeScore(collision.gameObject.GetComponent<Beans>().score);
        }
    }
}
