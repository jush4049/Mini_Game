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
        // ĳ���Ͱ� ȭ�� �Ʒ��� ������� Ȯ��
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        if (pos.y < -100)
        {
            isDead = true;
            return;
        }

        // Ű �Է�
        float keyValue = Input.GetAxis("Horizontal");

        // ȭ�� �����ڸ����� Ȯ��
        if ((keyValue < 0 && pos.x < 40) ||
            (keyValue > 0 && pos.x > Screen.width - 40))
        {
            keyValue = 0;
        }

        moveDir.x = keyValue * moveSpeed;

        // �߷�
        moveDir.y -= gravity * Time.deltaTime;

        // �̵�
        transform.Translate(moveDir * Time.deltaTime);

        // �ִϸ��̼�
        //anim.SetFloat("velocity", moveDir.y);
    }

    void CheckFoothold()
    {
        // üũ ����Ʈ���� �Ʒ��� 0.1 �̳� ����
        RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, 0.2f);

        // ����� ���
        Debug.DrawRay(checkPoint.position, Vector2.down * 1f, Color.blue);

        // ������ ������Ʈ�� �����̸� ���� �ӵ� ����
        if (hit.collider != null && hit.collider.tag == "Foothold")
        {
            moveDir.y = jumpSpeed;
        }

    }
}
