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
        // 화면을 벗어나면 제거
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        if (pos.y < -60)
        {
            Destroy(gameObject);
        }
    }

    void InitCoin()
    {
        // 오브젝트 초기화
        /*kind = int.Parse(transform.name.Substring(4, 1));*/

        // 오브젝트 스프라이트 설정
        /*Sprite[] sprites = Resources.LoadAll<Sprite>("Coin");
        GetComponent<SpriteRenderer>().sprite = sprites[kind];*/
    }
}
