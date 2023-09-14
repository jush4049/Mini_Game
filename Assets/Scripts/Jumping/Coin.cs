using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    int kind;

    void Start()
    {
        InitCoin();
    }

    void Update()
    {
        // ȭ���� ����� ����
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        if (pos.y < -60)
        {
            Destroy(gameObject);
        }
    }

    void InitCoin()
    {
        // ������Ʈ �ʱ�ȭ
        kind = int.Parse(transform.name.Substring(4, 1));
    }

    void GetCoin()
    {
        // GameManager�� ����
        GameObject.Find("GameManager").SendMessage("GetCoin", kind);

        GetComponent<AudioSource>().Play();

        GameObject score = Instantiate(Resources.Load("Score")) as GameObject;
        score.SendMessage("SetScore", 100 + kind * 100); // 100 ~ 300
        score.transform.position = transform.position;

        // 0.5�� �� ����
        Destroy(GetComponent<Collider>());
        GetComponent<SpriteRenderer>().sprite = null;
        Destroy(gameObject, 0.5f);
    }
}
