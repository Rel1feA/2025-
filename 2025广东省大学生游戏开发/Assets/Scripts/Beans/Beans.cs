using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Beans : MonoBehaviour
{
    public float score;

    public float GetLowerSpeed()
    {
        return -score/1000;
    }
}
