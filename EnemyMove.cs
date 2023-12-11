using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Invoke("Think", 5);
    }

    
    void FixedUpdate()
    {  
         //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);// 절댓값 클수록 빨라짐

    }

    //재귀 함수:자신을 스스로 호출하는 함수
    void Think()
    {
        nextMove = Random.Range(-10, 20);// 속도 -10에서 20 사이 왔다갔다 랜덤으로 변수 저장
        Invoke("Think", 5);
    }
}