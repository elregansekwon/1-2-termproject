using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();// anim 변수 초기화 추가
    }

    void Update()
    {
        
        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded){
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
        //Stop Speed
        if (Input.GetButtonUp("Horizontal")) {
            
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);

        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
        }
        //Reset isJumping 파라미터
        if (isGrounded)
        {
            anim.SetBool("isJumping", false);
        }
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
        if(rigid.velocity.x > maxSpeed) //Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if(rigid.velocity.x < maxSpeed*(-1)) //Left Max Speed
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);

        //Landing Platform
        if(rigid.velocity.y < 0) {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position,Vector3.down, 1, LayerMask.GetMask("Platform"));

            if(rayHit.collider != null) {
                if(rayHit.distance < 0.5f)
                    anim.SetBool("isJumping",false);

                }
        
            }
        }
    }
}
