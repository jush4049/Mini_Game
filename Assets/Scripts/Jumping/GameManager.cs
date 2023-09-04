using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Random = UnityEngine.Random;

[RequireComponent (typeof(AudioSource))]

public class GameManager : MonoBehaviour
{
    AudioSource music; // �������
    Transform spawnPoint; // ���� ����Ʈ
    Vector3 worldSize; // ȭ���� ũ�� (���� ��ǥ)

    void Start()
    {
        InitGame();
    }

    // ���� �ʱ�ȭ
    void InitGame()
    {
        // �������
        music = GetComponent<AudioSource>();
        music.loop = true;

        if (music.clip != null)
        {
            music.Play();
        }

        // ���� ����Ʈ
        spawnPoint = GameObject.Find ("SpawnPoint").transform;

        // ȭ���� ũ��
        Vector3 screenSize = new Vector3(Screen.width, Screen.height);
        screenSize.z = 10;
        worldSize = Camera.main.ScreenToWorldPoint(screenSize);
    }
}
