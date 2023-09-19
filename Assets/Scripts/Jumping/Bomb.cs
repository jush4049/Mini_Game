using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    void Update()
    {
        // ȭ���� ����� ����
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        if (pos.y < -60)
        {
            Destroy(gameObject);
        }
    }

    void FireBomb()
    {
        GetComponent<AudioSource>().Play();

        GameObject Fire = Instantiate(Resources.Load("Explosion")) as GameObject;
        Fire.transform.position = transform.position;

        // 1�� �� ����
        Destroy(GetComponent<Collider>());
        GetComponent<SpriteRenderer>().sprite = null;
        Destroy(gameObject, 1.0f);
    }
}
