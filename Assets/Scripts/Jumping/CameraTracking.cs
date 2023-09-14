using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    Transform target; // Ʈ��ŷ ���
    float height; // target�� �ִ� ����

    void Start()
    {
        // �÷��̾�� �ִ� ����
        target = GameObject.Find("Player").transform;
        height = target.position.y;
    }

    void LateUpdate()
    {
        // target�� ����
        float ty = target.position.y;
        if (ty <= height) return;

        // ���������� ����
        float cy = transform.position.y;
        cy = Mathf.Lerp(cy, ty, 5 * Time.deltaTime);

        // ī�޶� ���� ����
        Vector3 pos = transform.position;
        pos.y = cy + 0.05f;
        transform.position = pos;

        // �ִ� ���� ����
        height = ty;
    }
}
