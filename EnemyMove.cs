using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public int nextMove; // 행동 지표 결정 변수
    CapsuleCollider2D capesuleCollider;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capesuleCollider = GetComponent<CapsuleCollider2D>();
        Invoke("Think", 3);
    }

    void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector2(nextMove * 6, rigid.velocity.y);

        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
        //[THINK]
        // Ray가 너무 짧아서 Platform까지 닿지 않는 문제가 있었고
        // Platform collider의 사이 사이를 낭떠러지로 판단하는 경우가 있어서, Box
        Debug.DrawRay(frontVec, Vector3.down * 5.0f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 5.0f, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Debug.Log("경고! 이 앞 낭떠러지다");
            Turn();
        }
    }

    void Think()
    {
        //Set Next Active
        nextMove = Random.Range(-1, 2);

        //Flip Sprite
        if(nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
        }

        //Set Next Active
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 3);
    }

    public void OnDamaged()//enemy가 죽음 함수: 죽었을 때 취해야하는 액션 구현
    {
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Sprite Flip Y
        spriteRenderer.flipY = true;
        //Collider Disable
        capesuleCollider.enabled = false;
        //Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //Destroy
        Invoke("DeActive", 3);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}