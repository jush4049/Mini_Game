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
        /*kind = int.Parse(transform.name.Substring(4, 1));*/

        // ������Ʈ ��������Ʈ ����
        /*Sprite[] sprites = Resources.LoadAll<Sprite>("Coin");
        GetComponent<SpriteRenderer>().sprite = sprites[kind];*/
    }
}
