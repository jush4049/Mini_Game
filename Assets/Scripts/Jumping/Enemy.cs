using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator anim; // Animator

    int dir; // ����
    float speed; // �ӵ�

    void Start()
    {
        anim = GetComponent<Animator>();

        InitEnemy(); // ������Ʈ �ʱ�ȭ
    }

    void Update()
    {
        float amount = speed * dir * Time.deltaTime;
        transform.Translate(Vector3.right * amount);

        CheckAlive(); // ȭ�� ������ �������� üũ
    }

    void InitEnemy()
    {
        // ������Ʈ �̵� ����
        dir = (Random.Range(0,2) == 0) ? -1 : 1; // -1 or 1
        transform.localScale = new Vector3(dir, 1, 1);

        // �̵� �ӵ�
        speed = Random.Range(5, 8f); // 10 ~ 15
        anim.speed = 1 + (speed - 5) / 3; // 1 ~ 2
    }

    void CheckAlive()
    {
        Vector3 worldsPos = transform.position;
        worldsPos.z = 20;

        Vector2 pos = Camera.main.WorldToScreenPoint(worldsPos);
        if ((dir == -1 && pos.x < -60) || // �������� ���
            (dir == 1 && pos.x > Screen.width + 60)) { // ���������� ���
            Destroy(gameObject);
        }
    }
}
