using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Random = UnityEngine.Random;

[RequireComponent (typeof(AudioSource))]

public class GameManager : MonoBehaviour
{
    AudioSource music; // 배경음악
    Transform spawnPoint; // 스폰 포인트
    Vector3 worldSize; // 화면의 크기 (월드 좌표)

    void Start()
    {
        InitGame();
    }

    // 게임 초기화
    void InitGame()
    {
        // 배경음악
        music = GetComponent<AudioSource>();
        music.loop = true;

        if (music.clip != null)
        {
            music.Play();
        }

        // 스폰 포인트
        spawnPoint = GameObject.Find ("SpawnPoint").transform;

        // 화면의 크기
        Vector3 screenSize = new Vector3(Screen.width, Screen.height);
        screenSize.z = 10;
        worldSize = Camera.main.ScreenToWorldPoint(screenSize);
    }
}
