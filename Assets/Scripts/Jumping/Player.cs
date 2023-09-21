using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameObject player;
    Animator anim;
    Transform checkPoint;

    float moveSpeed = 8f;
    float jumpSpeed = 25f;
    float gravity = 19f;

    Vector3 moveDir;
    bool isDead = false;

    GameManager manager;

    void Start()
    {
        anim = GetComponent<Animator>();
        checkPoint = transform.Find("CheckPoint");

        manager = FindObjectOfType<GameManager>();
        player = GameObject.Find("Player");

        player.GetComponent<Player>().enabled = false;
    }

    void Update()
    {
        if (isDead) return;

        CheckFoothold();
        Move();
    }

    // 스크립트 활성화
    void PlayerStart()
    {
        player.GetComponent<Player>().enabled = true;
    }

    void Move()
    {
        // 캐릭터가 화면 아래를 벗어났는지 확인
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        if (pos.y < -300)
        {
            isDead = true;
            return;
        }

        // 키 입력
        float keyValue = Input.GetAxis("Horizontal");

        // 화면 가장자리인지 확인
        if ((keyValue < 0 && pos.x < 40) ||
            (keyValue > 0 && pos.x > Screen.width - 40))
        {
            keyValue = 0;
        }

        // 게임 오버
        if (pos.y < -100)
        {
            isDead = true;
            manager.SendMessage("SetGameOver");
            return;
        }

        if (manager.isMobile)
        {
            keyValue = manager.buttonAxis;
        }
        moveDir.x = keyValue * moveSpeed;

        // 중력
        moveDir.y -= gravity * Time.deltaTime;

        // 이동
        transform.Translate(moveDir * Time.deltaTime);
    }

    void CheckFoothold()
    {
        // 체크 포인트에서 아래쪽 0.1 이내 조사
        RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, 0.2f);

        // 디버그 출력
        Debug.DrawRay(checkPoint.position, Vector2.down * 1f, Color.blue);

        // 조사한 오브젝트가 발판이면 점프 속도 설정
        if (hit.collider != null && hit.collider.tag == "Foothold")
        {
            Debug.Log("점프");
            moveDir.y = jumpSpeed;
        }
    }

    // 충돌 처리
    void OnTriggerEnter2D (Collider2D coll)
    {
        Transform other = coll.transform;

        switch (other.tag)
        {
            case "Foothold":
                moveDir.y = jumpSpeed;
                break;
            case "Enemy":
                other.SendMessage("DropEnemy");
                break;
            case "Item":
                other.SendMessage("GetCoin");
                break;
            case "Bomb":
                Destroy(player.GetComponent<Rigidbody2D>());
                other.SendMessage("FireBomb");
                break;
        }
    }
}
