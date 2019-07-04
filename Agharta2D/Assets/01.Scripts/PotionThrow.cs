using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionThrow : MonoBehaviour
{
    public GameObject PC;
    public float throwPower;
    public float throwAngle;
    public Rigidbody2D rgd;

    private int dir;
    public float speed;
    // Start is called before the first frame update
    void Awake()
    {
        PC = GameObject.Find("PC");//캐릭터 오브젝트 가져오기
        if (PC.GetComponent<PlayerControl>().facingRight)//캐릭터 방향확인
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }
        speed = PC.GetComponent<PlayerControl>().speed;
        rgd = GetComponent<Rigidbody2D>();
        throwAngle = throwAngle / 180.0f * Mathf.PI;//발사각도를 레디언 각도로 변경
    }

    void Start()
    {
        rgd.AddForce(new Vector2((throwPower * Mathf.Cos(throwAngle) + speed * 0.5f * Mathf.Abs(Input.GetAxis("Horizontal"))) * dir, throwPower * Mathf.Sin(throwAngle)), ForceMode2D.Impulse);
    }

    void Update()
    {
        if(this.transform.position.y < -2)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            Destroy(this.gameObject);
        }        
    }

    private void OnDestroy()
    {
        PC.GetComponent<PlayerControl>().ammo++;
    }

}
