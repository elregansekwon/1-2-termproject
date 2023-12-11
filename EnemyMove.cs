using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer renderer;
    public int nextMove;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 5);
    }

    void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platforms"));
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
            renderer.flipX = nextMove == 1;
        }

        //Set Next Active
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove *= -1;
        renderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 5);
    }
}
