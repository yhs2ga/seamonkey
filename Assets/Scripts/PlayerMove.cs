using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public static float playerHP = 10;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spr;

    private int direction = -1;
    private bool isJump = false;
    private bool isAttack = false;
    private bool isDamage = false;
    private float a;

    [SerializeField] private GameObject weapon;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !isAttack)
        {
            isAttack = true;
            StartCoroutine(attack());

        }
        //Jump
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isJump = true;
        }

        if (isDamage)
        {
            a += 2f * Time.deltaTime;
            spr.color = new Color(1, 1, 1, a);
            if (a >= 1) isDamage = false;
        }

        // Debug.Log("Player HP: " + playerHP);
    }

    void FixedUpdate()
    {
        //Move By Key Control
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.linearVelocity.x > maxSpeed) //Right Max Speed
            rigid.linearVelocity = new Vector2(maxSpeed, rigid.linearVelocity.y);
        else if (rigid.linearVelocity.x < maxSpeed * (-1)) //Left Max Speed
            rigid.linearVelocity = new Vector2(maxSpeed * (-1), rigid.linearVelocity.y);
        
        if (h == 0) rigid.linearVelocity = new Vector2(0, rigid.linearVelocityY);

        if (h == 1 && !isAttack)
        {
            spr.flipX = true;
            weapon.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            direction = 1;
        }
        else if (h == -1 && !isAttack)
        {
            spr.flipX = false;
            weapon.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            direction = -1;
        }

    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJump = false;
        }

        if (other.gameObject.CompareTag("Monster")) OnDamaged(1);
    }

    public void OnDamaged(float damage)
    {
        if (!isDamage)
        {
            playerHP -= damage;
            a = 0.5f;
            isDamage = true;
        }
    }

    // void OffDamaged()
    // {
    //     gameObject.layer = 8;
    //     spr.color = new Color(1, 1, 1, 1);
    // }
    
    IEnumerator OffDamaged()
    {
        spr.color = Color.Lerp(spr.color, new Color(1, 1, 1, 1), 10 * Time.deltaTime);
        Debug.Log(spr.color.a);
        yield return new WaitForSeconds(0.5f);
        gameObject.layer = 8;
    }

    IEnumerator attack()
    {
        weapon.transform.position = new Vector2(weapon.transform.position.x + 1 * direction, weapon.transform.position.y);
        // yield return new WaitForSeconds(0.05f);
        weapon.GetComponent<Weapon>().isAttack = "Monster";
        weapon.GetComponent<Weapon>().damage = 1;
        yield return new WaitForSeconds(0.1f);
        weapon.GetComponent<Weapon>().isAttack = null;
        weapon.transform.position = new Vector2(weapon.transform.position.x - 1 * direction, weapon.transform.position.y);
        isAttack = false;
    }
}
