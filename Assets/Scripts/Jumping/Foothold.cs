using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Random = UnityEngine.Random;

public class Foothold : MonoBehaviour
{
    void Start()
    {
        InitFoothold();
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

    // ������Ʈ �ʱ�ȭ
    void InitFoothold()
    {
        // ������Ʈ ũ��
        float sx = Random.Range(7f, 10);

        // ������Ʈ ����
        int x = (Random.Range(0, 2) == 0) ? -1 : 1; // -1 or 1

        // ���� �� ũ�� ����
        transform.localScale = new Vector3(sx * x, 10, 1);
    }
}
