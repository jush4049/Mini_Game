using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator anim; // Animator

    int dir; // 방향
    float speed; // 속도

    void Start()
    {
        anim = GetComponent<Animator>();

        InitEnemy(); // 오브젝트 초기화
    }

    void Update()
    {
        float amount = speed * dir * Time.deltaTime;
        transform.Translate(Vector3.right * amount);

        CheckAlive(); // 화면 밖으로 나갔는지 체크
    }

    void InitEnemy()
    {
        // 오브젝트 이동 방향
        dir = (Random.Range(0,2) == 0) ? -1 : 1; // -1 or 1
        transform.localScale = new Vector3(dir, 1, 1);

        // 이동 속도
        speed = Random.Range(5, 8f); // 5 ~ 8
        anim.speed = 1 + (speed - 5) / 3; // 1 ~ 2

        // 오브젝트 위치를 Screen 좌표로 전환
        Vector3 worldPos = transform.position;
        Vector3 pos = Camera.main.WorldToScreenPoint(worldPos);

        // 이동 방향 조사
        if (dir == -1)
        {
            pos.x = Screen.width + 60;
        }
        else
        {
            pos.x = -60;
        }
    }

    void CheckAlive()
    {
        Vector3 worldsPos = transform.position;
        worldsPos.z = 20;

        Vector2 pos = Camera.main.WorldToScreenPoint(worldsPos);
        if ((dir == -1 && pos.x < -60) || // 왼쪽으로 벗어남
            (dir == 1 && pos.x > Screen.width + 60)) // 오른쪽으로 벗어남
        {
            Destroy(gameObject);
        }
    }

    // 충돌 처리
    void DropEnemy()
    {
        // GameManager에 통지
        FindObjectOfType<GameManager>().SendMessage("EnemyStrike");

        GetComponent<AudioSource>().Play();
        transform.localEulerAngles = new Vector3(0, 0, 180);
        //anim.enabled = false;

        // 콜라이더 제거 및 중력 적용
        Destroy(GetComponent<Collider2D>());
        GetComponent<Rigidbody2D>().gravityScale = 1;
        speed = 0;

        // 점수
        GameObject score = Instantiate(Resources.Load("Score")) as GameObject;
        score.transform.position = transform.position;

        score.SendMessage("SetScore", -100);
    }
}
