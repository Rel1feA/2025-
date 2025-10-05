using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Player shootPlayer;
    public BoxCollider2D boxCol;
    private Collider2D ignoreCollider;
    public Vector2 dir;
    public float speed;
    

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        boxCol=GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        Move();      
    }

    public void Move()
    {
        rb2d.MovePosition(rb2d.position + dir * speed * Time.fixedDeltaTime);
    }

    public void SetShootPlayer(Player player)
    {
        if(player != null)
        {
            this.shootPlayer = player;
        }
    }

    public void SetIgnoreCol(Collider2D col)
    {
        ignoreCollider=col;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall"))
        {
            shootPlayer.SetCanShoot(true);
            if(ignoreCollider!=null)
            {
                Physics2D.IgnoreCollision(ignoreCollider, boxCol, false);
            }
            PoolManager.Instance.PushObj("Prefabs/Bullet", gameObject);
        }
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Dead();
            shootPlayer.SetCanShoot(true);
            if (ignoreCollider != null)
            {
                Physics2D.IgnoreCollision(ignoreCollider, boxCol, false);
            }
            PoolManager.Instance.PushObj("Prefabs/Bullet", gameObject);
        }
    }
}
