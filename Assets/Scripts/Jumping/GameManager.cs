using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;

[RequireComponent (typeof(AudioSource))]

public class GameManager : MonoBehaviour
{
    AudioSource music; // 배경음악
    Transform spawnPoint; // 스폰 포인트
    Vector3 worldSize; // 화면의 크기 (월드 좌표)

    Transform player; // 플레이어

    Image panelButton; // 패널
    Image panelGameOver;

    Text textHeight; // 텍스트
    Text textCoin;
    Text textEnemy;
    Text textScore;

    float playerHeight = 0; // 점수
    int coinScore = 0;
    int coinCount = 0;
    int enemyCount = 0;
    int score = 0;

    public bool isMobile; // 모바일 디바이스 확인
    public float buttonAxis; // 버튼 값 (-1.0 ~ 1.0)

    int dir; // -1 : 왼쪽 버튼, 1 : 오른쪽 버튼
    bool isGameOver; // 게임 오버 확인


    void Awake()
    {
        InitGame();
        InitWidget();
    }

    void Update()
    {
        MakeFoothold();
        MakeEnemy();
        MakeItem();

        if (!isGameOver) SetScore();
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

    // 발판 생성
    void MakeFoothold()
    {
        // 발판 갯수 구하기
        int count = GameObject.FindGameObjectsWithTag("Foothold").Length;
        if (count > 3) return;

        // 스폰 포인트 높이에 지그재그로 배치
        Vector3 pos = spawnPoint.position;
        pos.x = Random.Range(-worldSize.x * 1f, worldSize.x * 1f);

        // 발판 오브젝트
        GameObject footHold = Instantiate(Resources.Load("Foothold")) as GameObject;
        footHold.transform.position = pos;

        // 스폰 포인트를 위로 이동
        spawnPoint.position += new Vector3(0, 10, 0);
    }

    // 적 생성
    void MakeEnemy()
    {
        int count = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (count > 7 || Random.Range(0, 1000) < 980) return;

        Vector3 pos = spawnPoint.position;
        pos.y -= Random.Range(0, 5f);

        GameObject enemy = Instantiate(Resources.Load("Enemy")) as GameObject;
        enemy.transform.position = pos;
    }

    void MakeItem()
    {
        int count = GameObject.FindGameObjectsWithTag("Item").Length;
        if (count > 5 || Random.Range(0, 1000) < 980) return;

        Vector3 pos = spawnPoint.position;
        pos.x = Random.Range(-worldSize.x * 1f, worldSize.x * 1f);
        pos.y += Random.Range(0.5f, 1.5f);

        GameObject item = Instantiate(Resources.Load("Item")) as GameObject;
        item.name = "Item" + Random.Range(0, 3);
        item.transform.position = pos;
    }

    void InitWidget()
    {
        // 모바일 디바이스인지 확인

        isMobile = Application.platform == RuntimePlatform.Android ||
                   Application.platform == RuntimePlatform.IPhonePlayer;

        // isMobile = true; // *테스트 코드

        Cursor.visible = isMobile;

        // 모바일 디바이스에서만 패널 생성
        panelButton = GameObject.Find("PanelButton").GetComponent<Image>();
        panelButton.gameObject.SetActive(isMobile);

        // 종료 패널
        panelGameOver = GameObject.Find("PanelGameOver").GetComponent<Image>();
        panelGameOver.gameObject.SetActive(false);

        // 점수
        textHeight = GameObject.Find("HeightText").GetComponent<Text>();
        textCoin = GameObject.Find("CoinText").GetComponent<Text>();
        textEnemy = GameObject.Find("EnemyText").GetComponent<Text>();
        textScore = GameObject.Find("ScoreText").GetComponent<Text>();

        // 플레이어
        player = GameObject.Find("Player").transform;
    }

    void SetScore()
    {
        // 플레이어 최대 높이 계산
        if (player.position.y > playerHeight)
        {
            playerHeight = player.position.y;
        }

        score = Mathf.FloorToInt(playerHeight) * 100 + coinScore - enemyCount * 100;

        // 화면 표시
        textHeight.text = playerHeight.ToString("#,##0.0");
        textCoin.text = coinCount.ToString();
        textEnemy.text = enemyCount.ToString();
        textScore.text = score.ToString("#,##0");
    }

    // 외부 호출
    void GetCoin (int kind)
    {
        coinCount++;
        coinScore += (kind * 100) + 100;
    }

    // 외부 호출
    void EnemyStrike()
    {
        enemyCount++;
    }

    void SetGameOver()
    {
        isGameOver = true;
        panelGameOver.gameObject.SetActive(true);
        Cursor.visible = true;

        // 배경음악 변경
        music.clip = Resources.Load("Jumping_GameOver_BGM", typeof(AudioClip)) as AudioClip;
        // music.loop = false;
        music.Play();
    }

    public void OnButtonClick (GameObject button)
    {
        switch (button.name)
        {
            case "AgainButton":
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case "QuitButton":
                Application.Quit();
                break;
        }
    }

    public void OnButtonPress (GameObject button)
    {
        switch (button.name)
        {
            case "LeftButton":
                dir = -1;
                StartCoroutine(GetButtonAxis());
                break;
            case "RightButton":
                StartCoroutine(GetButtonAxis());
                dir = 1;
                break;
        }
    }

    public void OnButtonUp()
    {
        //buttonAxis = 0;
        dir = 0;
        StartCoroutine(GetButtonAxis());
    }

    // 버튼 가속도 처리
    IEnumerator GetButtonAxis()
    {
        while (true)
        {
            // 버튼을 눌렀을 때 0 근처이면 보간 중지
            if (dir == 0 && Mathf.Abs(buttonAxis) < 0.01)
            {
                buttonAxis = 0;
                yield break;
            }

            // 결과 값의 오차가 0.01 미만이면 보간 중지
            if (Mathf.Abs(dir) - Mathf.Abs(buttonAxis) < 0.01)
            {
                buttonAxis = dir;
                yield break;
            }

            // 선형 보간
            buttonAxis = Mathf.MoveTowards(buttonAxis, dir, 3 * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
