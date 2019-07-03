using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//왕환민
public class MonsterControl : MonoBehaviour
{
    Animator animator; 
    public float movePower = 1f;
    Vector3 movement;
    private bool isMoving = true;
    int movementFlag = 0;  //0:normal 1:lft  2:right 

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>(); 

        StartCoroutine("ChangeMovement");
    }
    
    IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(0, 3);

        if (movementFlag ==0)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }
        yield return new WaitForSeconds(1f);

        StartCoroutine("ChangeMovement");//ChangeMovenet 다시 시작
    }

    private void FixedUpdate()
    {
        Move();
    }
    //움직임 설정
    void Move()
    {
        Vector3 MoveVelocity = Vector3.zero;
        if (movementFlag==1)
        {
            MoveVelocity = Vector3.left;
            transform.localScale = new Vector3 (0.95f, 1.43f, 1);//방향
        }
        else if (movementFlag==2)
        {
            MoveVelocity = Vector3.right;
            transform.localScale = new Vector3(-0.95f, 1.43f, 1);//방향
        }
        transform.position += MoveVelocity * movePower * Time.deltaTime;
    }
    void Update()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);



        if (pos.x < 0.12f) pos.x = 0.12f;

        if (pos.x > 0.97f) pos.x = 0.97f;

        if (pos.y < 0f) pos.y = 0f;

        if (pos.y > 1f) pos.y = 1f;



        transform.position = Camera.main.ViewportToWorldPoint(pos);



    
    }

}
