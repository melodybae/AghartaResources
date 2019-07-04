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
    public SpriteRenderer SR;

    public bool canJump = false; //점프 확인 플래그
    public bool facingRight = true; //오른쪽/왼쪽 방향 플래그
    public bool canShoot = true; //공격 가능 확인 플래그
    public bool canMove = true; //이동 가능 확인 플래그
    public bool invinc = false; //무적 시간 적용 플래그

    public int HP = 5;
    public int ammo; //쏠수있는 총알 수

    public GameObject[] Skill;
    public int skillNumber = 0;
    public int stage = 0;
    Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(HP > 0)//캐릭터가 사망하지 않았다면(HP 0이상이라면)
        {
            if (canMove)//움직일수있는지
            {
                if (facingRight && Input.GetAxisRaw("Horizontal") == -1)
                {
                    flip();//좌우 반전
                }
                else if (!facingRight && Input.GetAxisRaw("Horizontal") == 1)
                {
                    flip();//좌우 반전
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
        }
        else if(HP <= 0 || this.transform.position.y < -2f)//피가 소진되었거나 맵 일정이하로 떨어지면
        {
            PCDied();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !invinc)//몬스터 피격시 HP 1감소
        {
            HP -= 1;
            
            if(HP <= 0)//피격후 사망시 피격모션없이 즉시사망
            {
                Rigidbody.velocity = new Vector2(0, 0);
                return;
            }
            else if(collision.gameObject.transform.position.x >= this.transform.position.x)//몬스터한테 피격받은 방향확인
            {
                Rigidbody.velocity = new Vector2(0,0);
                Rigidbody.AddForce(new Vector2(-100, 100));
                invinc = true;
                StartCoroutine(invincible());
            }
            else
            {
                Rigidbody.velocity = new Vector2(0, 0);
                Rigidbody.AddForce(new Vector2(100, 100));
                invinc = true;
                StartCoroutine(invincible());
            }         
        }
    }//몬스터 혹은 피격시 HP감소와 밀려남

    IEnumerator invincible()
    {
        int countTime = 0;
        canMove = false;
        while (countTime < 10)
        {                        
            if(countTime == 7)
            {
                canMove = true;
            }
            if(countTime%2 == 0)
            {
                SR.color = new Color32(255, 255, 255, 90);
            }
            else
            {
                SR.color = new Color32(255, 255, 255, 180);
            }
            yield return new WaitForSeconds(0.1f);
            countTime++;
        }
        SR.color = new Color32(255, 255, 255, 255);

        invinc = false;
    }//피격후 무적시간

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")//바닥이랑 붙어있을시 점프 가능
        { 
            canJump = true;
            Anim.SetBool("InAir", false);
        }
    }//바닥이랑 접해있는동안은 점프 가능

    void OnCollisionExit2D(Collision2D collision)//바닥이랑 떨어지게 되면 점프 불가능
    {
        if(collision.gameObject.tag =="Ground")
        {
            canJump = false;
            Anim.SetBool("InAir", true);
        }        
    }

    void flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }//캐릭 방향전환

    void PCMove()//좌우 움직임
    {
        Anim.SetBool("IsMoving", true);

        horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;

        this.transform.Translate(horizontal, 0.0f, 0.0f);
    }

    void PCDied()//플레이어 사망시
    {
        Debug.Log("PC Died!");
        Anim.SetBool("IsDead", true);
        //Invoke("?", 3f); 죽었을시 다시 시작 선택화면 띄우는 창

    }

    IEnumerator PCAttack(int skilltype)//플레이어 공격시
    {
        canShoot = false;
        yield return new WaitForSeconds(0.3f);//공격모션후 날라가는 딜레이

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
}