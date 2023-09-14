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
        kind = int.Parse(transform.name.Substring(4, 1));
    }

    void GetCoin()
    {
        // GameManager에 통지
        GameObject.Find("GameManager").SendMessage("GetCoin", kind);

        GetComponent<AudioSource>().Play();

        GameObject score = Instantiate(Resources.Load("Score")) as GameObject;
        score.SendMessage("SetScore", 100 + kind * 100); // 100 ~ 300
        score.transform.position = transform.position;

        // 0.5초 후 삭제
        Destroy(GetComponent<Collider>());
        GetComponent<SpriteRenderer>().sprite = null;
        Destroy(gameObject, 0.5f);
    }
}
