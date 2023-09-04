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
        // 화면을 벗어나면 제거
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        if (pos.y < -60)
        {
            Destroy(gameObject);
        }
    }

    // 오브젝트 초기화
    void InitFoothold()
    {
        // 오브젝트 크기
        float sx = Random.Range(7f, 10);

        // 오브젝트 방향
        int x = (Random.Range(0, 2) == 0) ? -1 : 1; // -1 or 1

        // 방향 및 크기 설정
        transform.localScale = new Vector3(sx * x, 10, 1);
    }
}
