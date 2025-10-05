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
        if (score < 10) return;
        ChangeScore(-10);
        PoolManager.Instance.GetObj("Prefabs/Bullet", (obj) =>
        {
            obj.transform.position=transform.position+(Vector3)movement.GetCurDir()*0.8f;
            obj.transform.rotation=transform.rotation;
            obj.GetComponent<Bullet>().dir = movement.GetCurDir();
        });
    }

    public void ChangeScore(int val)
    {
        score += val;
        switch (playerType)
        {
            case PlayerType.P1 :
                EventCenter.Instance.EventTrigger("Player1ScoreChange", score);
                break;
            case PlayerType .P2:
                EventCenter.Instance.EventTrigger("Player2ScoreChange", score);
                break;
            default:
                break;
        }
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

    public void Dead()
    {
        Debug.Log(name + "Dead");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bean"))
        {
            collision.gameObject.SetActive(false);
            movement.ChangeSpeed(lowSpeed);
            ChangeScore(collision.gameObject.GetComponent<Beans>().score);
        }
    }
}
