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

    // ��ũ��Ʈ Ȱ��ȭ
    void PlayerStart()
    {
        player.GetComponent<Player>().enabled = true;
    }

    void Move()
    {
        // ĳ���Ͱ� ȭ�� �Ʒ��� ������� Ȯ��
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        if (pos.y < -300)
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

        // ���� ����
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

        // �߷�
        moveDir.y -= gravity * Time.deltaTime;

        // �̵�
        transform.Translate(moveDir * Time.deltaTime);
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
            Debug.Log("����");
            moveDir.y = jumpSpeed;
        }
    }

    // �浹 ó��
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
