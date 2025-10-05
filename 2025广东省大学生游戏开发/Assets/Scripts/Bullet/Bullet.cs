using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Player shootPlayer;
    public Vector2 dir;
    public float speed;
    

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall"))
        {
            shootPlayer.SetCanShoot(true);
            PoolManager.Instance.PushObj("Prefabs/Bullet", gameObject);
        }
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Dead();
            shootPlayer.SetCanShoot(true);
            PoolManager.Instance.PushObj("Prefabs/Bullet", gameObject);
        }
    }
}
