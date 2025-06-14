using System.Collections;
using System.Threading;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float radius;
    public LayerMask layer;
    public Collider2D[] colliders;
    private bool delay = true;

    void Update()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, radius, layer);
        StartCoroutine(Delay());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    IEnumerator Delay()
    {
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player") && delay)
            {
                delay = false;
                yield return new WaitForSeconds(1);
                collider.GetComponent<PlayerMove>().OnDamaged(5);
                delay = true;
            }
        }
    }
}
