using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement movement;
    [Range(-1f,0f)]
    public float lowSpeed;//吃到豆子后减少的速度
    [Range(0f, 1f)]
    public float upSpeed;//射出豆子后增加的速度
    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bean"))
        {
            collision.gameObject.SetActive(false);
            movement.ChangeSpeed(lowSpeed);
        }
    }
}
