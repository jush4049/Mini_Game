using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;
    Transform checkPoint;

    float moveSpeed = 8f;
    float jumpSpeed = 20f;
    float gravity = 19f;

    Vector3 moveDir;
    bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        checkPoint = transform.Find("CheckPoint");
    }

    void Update()
    {
        if (isDead) return;

        CheckFoothold();
        Move();
    }

    void Move()
    {
        // 캐릭터가 화면 아래를 벗어났는지 확인
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        if (pos.y < -100)
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

        moveDir.x = keyValue * moveSpeed;

        // 중력
        moveDir.y -= gravity * Time.deltaTime;

        // 이동
        transform.Translate(moveDir * Time.deltaTime);

        // 애니메이션
        //anim.SetFloat("velocity", moveDir.y);
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
            moveDir.y = jumpSpeed;
        }

    }
}
