using System;
using UnityEngine;

public class Weapon2 : MonoBehaviour
{
    public bool isAttack = true;

    [SerializeField] private Transform target;

    Vector2 moveDirection;

    private float time = 0;

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time < 2)
        {
            moveDirection = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 180f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (time > 3) transform.position += (Vector3)(moveDirection * 15 * Time.deltaTime);

#pragma warning disable CS0618
        if (time > 8) DestroyObject(gameObject);
#pragma warning restore CS0618
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (isAttack && other.tag == "Player")
        {
            PlayerMove player = other.GetComponent<PlayerMove>();
            player.OnDamaged(1);
            isAttack = false;
        }
    }
}
