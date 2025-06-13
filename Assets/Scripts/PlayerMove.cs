using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spr;

    private int direction = -1;
    private bool isJump = false;
    private bool isAttack = false;

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
            StartCoroutine(asdf());
    
        }
        //Jump
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isJump = true;
        }
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
    }

    IEnumerator asdf()
    {
        weapon.transform.position = new Vector2(weapon.transform.position.x + 1 * direction, weapon.transform.position.y);
        yield return new WaitForSeconds(0.1f);
        weapon.transform.position = new Vector2(weapon.transform.position.x - 1 * direction, weapon.transform.position.y);
        isAttack = false;
    }
}
