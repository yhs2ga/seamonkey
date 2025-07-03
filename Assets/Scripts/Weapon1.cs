using System;
using UnityEngine;

public class Weapon1 : MonoBehaviour
{
    public float damage = 0;
    public bool isAttack;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isAttack && other.tag == "Player")
        {
            PlayerMove player = other.GetComponent<PlayerMove>();
            player.OnDamaged(damage);
            isAttack = false;
        }
    }
}
