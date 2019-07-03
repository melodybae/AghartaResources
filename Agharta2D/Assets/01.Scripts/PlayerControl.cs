using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//유은호
public class PlayerControl : MonoBehaviour
{
    private float horizontal; //움직임 감지 변수
    public float speed, jump; //횡움직임 속도 점프 속도 변수
    public Rigidbody2D Rigidbody;

    private bool canJump = false;
    private bool facingRight = true;
    public int HP = 5;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(HP > 0)//캐릭터가 사망하지 않았다면(HP 0이상이라면)
        {
            if (facingRight && Input.GetAxisRaw("Horizontal") == -1)
            {
                flip();//좌우 반전
            }
            else if (!facingRight && Input.GetAxisRaw("Horizontal") == 1)
            {
                flip();
            }

            if (Input.GetAxis("Horizontal") != 0)
            {
                PCMove();//좌우 움직임
            }
            
            if (canJump && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))//점프가능한지? + 점프키 확인
            {
                canJump = false;
                Rigidbody.AddForce(Vector2.up * jump, ForceMode2D.Impulse);//점프 움직임
            }
        }
        else
        {
            PCDied();
        }
    }

    void flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")//점프 1회 한정
        {
            canJump = true;
        }
        if(collision.gameObject.tag == "Enemy")//몬스터 피격시 HP 1감소
        {
            HP -= 1;
        }
    }

    void PCMove()//좌우 움직임
    {
        horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;

        this.transform.Translate(horizontal, 0.0f, 0.0f);
    }

    void PCDied()//플레이어 사망시
    {

    }

    void PCAttack()//플레이어 공격시
    {

    }

    void PCSkill()//플레이어 스킬변경
    {

    }

    
}
