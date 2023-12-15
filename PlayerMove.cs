using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capesuleCollider;
    Animator anim;
    int jumpCount = 0;
    int maxJumpCount = 2;
    public float bouncePower;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capesuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            rigid.velocity = Vector2.zero; // 점프 시 현재 속도를 초기화
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            //anim.SetBool("isJumping", true);
            jumpCount++;
        }

        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);

            //Animation
            // if (Mathf.Abs(rigid.velocity.x) < 0.3)
            //     anim.SetBool("isWalking", false);
            // else
            //     anim.SetBool("isWalking", true);
        }

        //Direction Sprite
        if(Input.GetButton("Horizontal")) {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;
        }

        //Reset isJumping 파라미터
        //anim.SetBool("isJumping", false);
    }

    void FixedUpdate()
    {
        //Move Speed
        float h = Input.GetAxisRaw("Horizontal");

        if (h == 0)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
        else
        {
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            //Max Speed
            if (rigid.velocity.x > maxSpeed) //Right Max Speed
                rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
            else if (rigid.velocity.x < maxSpeed * (-1)) //Left Max Speed
                rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            jumpCount = 0;

            // 바닥에 닿으면 튕기는 효과 추가
            rigid.AddForce(Vector2.up * bouncePower, ForceMode2D.Impulse);
        }
    
        else if (collision.gameObject.tag == "Enemy")
        {
            //Attack
            if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            
            else //Damaged
            {
                OnDamaged(collision.transform.position);
            }
        }
        else if (collision.gameObject.tag == "Obstacle")
        {
            OnDamaged(collision.transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) // 클로버 아이템
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        if (collision.gameObject.tag == "Item")
        {   // Point
            bool isClover = collision.gameObject.name.Contains("Clover");
            gameManager.stagePoint += 50;

            //Deactive Item
            collision.gameObject.SetActive(false);
        }
       else if(collision.gameObject.tag == "Finish")
       {
            //Next Stage
            gameManager.NextStage();
       }
    }

    void OnAttack(Transform enemy)
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        //Point
        gameManager.stagePoint += 100;

        //Reaction Force
        rigid.AddForce(Vector2.up * 30, ForceMode2D.Impulse);

        //Enemy Die
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }

    void OnDamaged(Vector2 targetPos)
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        //Health Down
        gameManager.HealthDown();

        //Change Layer (Immortal Active)
        gameObject.layer = 9;

        //View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 30, ForceMode2D.Impulse); //값 클수록 많이 튀어오름

        Invoke("OffDamaged", 3);
        
    }

    void OffDamaged()
    {
         gameObject.layer = 8;
         spriteRenderer.color = new Color(1, 1, 1, 1);

    }

    public void OnDie()
    {
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Sprite Flip Y
        spriteRenderer.flipY = true;
        //Collider Disable
        capesuleCollider.enabled = false;
        //Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

     public void VelocityZero()
    {
       rigid.velocity = Vector2.zero;
    }
}
