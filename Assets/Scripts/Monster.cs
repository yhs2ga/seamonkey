using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Monster : MonoBehaviour
{
    public float radius1;
    public float radius2;
    public LayerMask layer;
    public Collider2D[] colliders1;
    private bool delay = true;

    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject weapon2;
    [SerializeField] private GameObject player;

    private int direction = -1;

    private float health = 10;
    private SpriteRenderer spr;
    private bool isDamage;
    private float a;

    private bool isAttack = false;

    private int pattern = 1;
    private float patternTime = 0;
    private float coolTime = 0f;

    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        colliders1 = Physics2D.OverlapCircleAll(transform.position, radius1, layer);
        StartCoroutine(Delay());

        if (isDamage)
        {
            a += 1.5f * Time.deltaTime;
            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, a);
            if (a >= 1) isDamage = false;
        }

        coolTime += Time.deltaTime;
        if (direction == 1)
        {
            spr.flipX = true;
            weapon.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (direction == -1)
        {
            spr.flipX = false;
            weapon.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        patternTime += Time.deltaTime;
        Debug.Log(patternTime + ", " + pattern);
        if (patternTime > 0)
        {
            if (pattern == 1)
            {
                if (transform.position.x - player.transform.position.x < 0) direction = 1;
                if (transform.position.x - player.transform.position.x > 0) direction = -1;

                transform.position = new Vector2(transform.position.x + 3f * direction * Time.deltaTime, transform.position.y);
                if (patternTime > 3)
                {
                    pattern = Random.Range(1, 4);
                    patternTime = -1f;
                }
            }
            else if (pattern == 2)
            {
                if (patternTime < 2) transform.position = new Vector2(transform.position.x - 3f * direction * Time.deltaTime, transform.position.y);
                if (patternTime > 2)
                {
                    StartCoroutine(attack2());
                    pattern = Random.Range(1, 4);
                    patternTime = -1;
                }
            }
            else if (pattern == 3)
            {
                StartCoroutine(attack3());
                pattern = Random.Range(1, 4);
                patternTime = -3f;
            }
        }




        if (health <= 0) Destroy(gameObject);
    }

    public void OnDamage(float damage)
    {
        if (!isDamage)
        {
            health -= damage;
            Debug.Log(health);
            a = 0.5f;
            isDamage = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius1);
        Gizmos.DrawWireSphere(transform.position, radius2);
    }

    IEnumerator Delay()
    {
        foreach (Collider2D collider in colliders1)
        {
            if (collider.CompareTag("Player") && delay && !isAttack && coolTime > 0)
            {
                delay = false;
                isAttack = true;
                StartCoroutine(attack1());
                yield return new WaitForSeconds(1);
                delay = true;
                coolTime = -3;
            }
        }
    }

    IEnumerator attack1()
    {
        weapon.transform.position = new Vector2(weapon.transform.position.x + 1 * direction, weapon.transform.position.y);
        weapon.GetComponent<Weapon1>().isAttack = true;
        weapon.GetComponent<Weapon1>().damage = 1;
        yield return new WaitForSeconds(0.1f);
        weapon.GetComponent<Weapon1>().isAttack = false;
        weapon.transform.position = new Vector2(weapon.transform.position.x - 1 * direction, weapon.transform.position.y);
        isAttack = false;
    }

    IEnumerator attack2()
    {
        weapon.GetComponent<Weapon1>().isAttack = true;
        weapon.GetComponent<Weapon1>().damage = 1;
        int time = 0;
        List<Vector2> dL = new List<Vector2>();
        while (time < 200)
        {
            // weapon.transform.position = new Vector2(weapon.transform.position.x + 1 * direction, weapon.transform.position.y);
            if (time < 100)
            {
                dL.Add(Vector2.Lerp(weapon.transform.position, new Vector2(4 * direction, weapon.transform.position.y), Time.deltaTime * 6));
                weapon.transform.position = Vector2.Lerp(weapon.transform.position, new Vector2(4 * direction, weapon.transform.position.y), Time.deltaTime * 6);
                Debug.Log(dL.Count());
            }
            else if (100 <= time && time < 200)
            {
                Debug.Log(50 - (time - 100));
                weapon.transform.position = dL[99 - (time - 100)];
            }
            time += 1;
            yield return new WaitForSeconds(0.005f);
        }
        weapon.transform.localPosition = new Vector2(0, weapon.transform.localPosition.y);
        yield return new WaitForSeconds(1f);
        weapon.GetComponent<Weapon1>().isAttack = false;
        isAttack = false;
        // weapon.transform.position = new Vector2(weapon.transform.position.x - 1 * direction, weapon.transform.position.y);
    }

    IEnumerator attack3()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(weapon2, new Vector3(Random.Range(-15f, 15f), Random.Range(-4f, 9f), 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(weapon2, new Vector3(Random.Range(-15f, 15f), Random.Range(-4f, 9f), 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(weapon2, new Vector3(Random.Range(-15f, 15f), Random.Range(-4f, 9f), 0), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        isAttack = false;
    }
}
