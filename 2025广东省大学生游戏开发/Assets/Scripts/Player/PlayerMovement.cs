using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerType
{
    P1,
    P2
}


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 currentDir;
    private Vector2 inputDir;
    private PlayerKey inputActions;

    public float speed;
    [Range(0.3f,1f)]
    public float speedMultiplier;

    public Vector2 rayBoxSize;

    public PlayerType playerType;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        inputActions=new PlayerKey();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        currentDir = Vector2.down;
    }

    private void Update()
    {
        SetInputDir();
        ChangeDir(inputDir);
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetInputDir( )
    {
        switch (playerType)
        {
            case PlayerType.P1:
                inputDir = inputActions.Player1.Move.ReadValue<Vector2>();
                break;
            case PlayerType.P2:
                inputDir = inputActions.Player2.Move.ReadValue<Vector2>();
                break;
            default:
                break;
        }
    }

    public void Move()
    {
        rb2d.MovePosition(rb2d.position + currentDir * speed * speedMultiplier * Time.fixedDeltaTime);
    }

    public void ChangeDir(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) == Mathf.Abs(dir.y))
        {
            return;
        }
        if(Occupied(dir))
        {
            return;
        }
        currentDir = dir;
    }

    public bool Occupied(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, rayBoxSize, 0, dir, 0.5f, LayerMask.GetMask("Wall"));
        return hit.collider!=null;
    }

    public void ChangeSpeed(float val)
    {
        speedMultiplier+=val;
        if(speedMultiplier<0.3f)
        {
            speedMultiplier = 0.3f;
        }
        if(speedMultiplier>1f)
        {
            speedMultiplier = 1f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, rayBoxSize);
    }
}
