using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//유은호
public class PlayerControl : MonoBehaviour
{
    public float horizontal; //움직임 감지 변수
    public float speed, jump; //횡움직임 속도 점프 속도 변수

    public Rigidbody2D Rigidbody;
    public Animator Anim;

    public bool canJump = false;
    public bool facingRight = true;
    public int HP = 5;

    public GameObject[] Skill;
    public int skillNumber = 0;
    public int stage = 0;
    Vector3 playerPos;

    public bool canShoot = true;
    public int ammo;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PCAnim();//캐릭터 애니메이션
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
            
            if (canJump && Input.GetKeyDown(KeyCode.LeftAlt))//점프가능한지? + 점프키 확인
            {
                Anim.SetBool("InAir", true);
                Rigidbody.AddForce(Vector2.up * jump, ForceMode2D.Impulse);//점프 움직임
            }

            if (Input.GetKey(KeyCode.LeftControl) && canShoot && ammo > 0)//스킬사용
            {
                StartCoroutine(PCAttack(skillNumber));
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift))//스킬변경
            {
                PCSkill();
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
        if (collision.gameObject.tag == "Enemy")//몬스터 피격시 HP 1감소
        {
            HP -= 1;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")//바닥이랑 붙어있을시 점프 가능
        { 
            canJump = true;
            Anim.SetBool("InAir", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)//바닥이랑 떨어지게 되면 점프 불가능
    {
        if(collision.gameObject.tag =="Ground")
        {
            canJump = false;
            Anim.SetBool("InAir", true);
        }        
    }

    void PCMove()//좌우 움직임
    {
        horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;

        this.transform.Translate(horizontal, 0.0f, 0.0f);
    }

    void PCDied()//플레이어 사망시
    {
        Anim.SetBool("IsDead", true);
    }

    IEnumerator PCAttack(int skilltype)//플레이어 공격시
    {
        canShoot = false;
        ammo -= 1;
        GameObject potion = Instantiate(Skill[skilltype]) as GameObject;
        playerPos = this.transform.position;
        if(facingRight)
        {
            playerPos.x += 0.2f;
        }
        else
        {
            playerPos.x -= 0.2f;
        }
           
        playerPos.y += 1.0f;
        potion.transform.position = playerPos;

        yield return new WaitForSeconds(0.5f);

        canShoot = true;
    }

    void PCSkill()//플레이어 스킬변경
    {
        skillNumber++;
        if(skillNumber > stage)//스테이지 클리어에 따른 스킬 가능 증가(stage 변수)
        {
            skillNumber = 0;
        }
    }

    void PCAnim()
    {
        if(Input.GetAxis("Horizontal") != 0)
        {
            Anim.SetBool("IsMoving", true);
        }
        else
        {
            Anim.SetBool("IsMoving", false);
        }
    }
}
