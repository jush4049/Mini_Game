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
        speed = Random.Range(5, 8f); // 10 ~ 15
        anim.speed = 1 + (speed - 5) / 3; // 1 ~ 2
    }

    void CheckAlive()
    {
        Vector3 worldsPos = transform.position;
        worldsPos.z = 20;

        Vector2 pos = Camera.main.WorldToScreenPoint(worldsPos);
        if ((dir == -1 && pos.x < -60) || // 왼쪽으로 벗어남
            (dir == 1 && pos.x > Screen.width + 60)) { // 오른쪽으로 벗어남
            Destroy(gameObject);
        }
    }
}
