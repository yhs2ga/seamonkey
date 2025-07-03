using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public String isAttack = null;
    public float damage = 0;

    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (isAttack != null && other != null)
        {
            if (other.tag == isAttack)
            {
                Monster monster = other.GetComponent<Monster>();
                monster.OnDamage(damage);
                isAttack = null;
            }
        }
    }
}
