using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;

[RequireComponent (typeof(AudioSource))]

public class GameManager : MonoBehaviour
{
    AudioSource music; // �������
    Transform spawnPoint; // ���� ����Ʈ
    Vector3 worldSize; // ȭ���� ũ�� (���� ��ǥ)

    Transform player; // �÷��̾�

    Image panelButton; // �г�
    Image panelGameOver;

    Text textHeight; // �ؽ�Ʈ
    Text textCoin;
    Text textEnemy;
    Text textScore;

    float playerHeight = 0; // ����
    int coinScore = 0;
    int coinCount = 0;
    int enemyCount = 0;
    int score = 0;

    public bool isMobile; // ����� ����̽� Ȯ��
    public float buttonAxis; // ��ư �� (-1.0 ~ 1.0)

    int dir; // -1 : ���� ��ư, 1 : ������ ��ư
    bool isGameOver; // ���� ���� Ȯ��


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

    // ���� ����
    void MakeFoothold()
    {
        // ���� ���� ���ϱ�
        int count = GameObject.FindGameObjectsWithTag("Foothold").Length;
        if (count > 3) return;

        // ���� ����Ʈ ���̿� ������׷� ��ġ
        Vector3 pos = spawnPoint.position;
        pos.x = Random.Range(-worldSize.x * 1f, worldSize.x * 1f);

        // ���� ������Ʈ
        GameObject footHold = Instantiate(Resources.Load("Foothold")) as GameObject;
        footHold.transform.position = pos;

        // ���� ����Ʈ�� ���� �̵�
        spawnPoint.position += new Vector3(0, 10, 0);
    }

    // �� ����
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
        // ����� ����̽����� Ȯ��

        isMobile = Application.platform == RuntimePlatform.Android ||
                   Application.platform == RuntimePlatform.IPhonePlayer;

        // isMobile = true; // *�׽�Ʈ �ڵ�

        Cursor.visible = isMobile;

        // ����� ����̽������� �г� ����
        panelButton = GameObject.Find("PanelButton").GetComponent<Image>();
        panelButton.gameObject.SetActive(isMobile);

        // ���� �г�
        panelGameOver = GameObject.Find("PanelGameOver").GetComponent<Image>();
        panelGameOver.gameObject.SetActive(false);

        // ����
        textHeight = GameObject.Find("HeightText").GetComponent<Text>();
        textCoin = GameObject.Find("CoinText").GetComponent<Text>();
        textEnemy = GameObject.Find("EnemyText").GetComponent<Text>();
        textScore = GameObject.Find("ScoreText").GetComponent<Text>();

        // �÷��̾�
        player = GameObject.Find("Player").transform;
    }

    void SetScore()
    {
        // �÷��̾� �ִ� ���� ���
        if (player.position.y > playerHeight)
        {
            playerHeight = player.position.y;
        }

        score = Mathf.FloorToInt(playerHeight) * 100 + coinScore - enemyCount * 100;

        // ȭ�� ǥ��
        textHeight.text = playerHeight.ToString("#,##0.0");
        textCoin.text = coinCount.ToString();
        textEnemy.text = enemyCount.ToString();
        textScore.text = score.ToString("#,##0");
    }

    // �ܺ� ȣ��
    void GetCoin (int kind)
    {
        coinCount++;
        coinScore += (kind * 100) + 100;
    }

    // �ܺ� ȣ��
    void EnemyStrike()
    {
        enemyCount++;
    }

    void SetGameOver()
    {
        isGameOver = true;
        panelGameOver.gameObject.SetActive(true);
        Cursor.visible = true;

        // ������� ����
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

    // ��ư ���ӵ� ó��
    IEnumerator GetButtonAxis()
    {
        while (true)
        {
            // ��ư�� ������ �� 0 ��ó�̸� ���� ����
            if (dir == 0 && Mathf.Abs(buttonAxis) < 0.01)
            {
                buttonAxis = 0;
                yield break;
            }

            // ��� ���� ������ 0.01 �̸��̸� ���� ����
            if (Mathf.Abs(dir) - Mathf.Abs(buttonAxis) < 0.01)
            {
                buttonAxis = dir;
                yield break;
            }

            // ���� ����
            buttonAxis = Mathf.MoveTowards(buttonAxis, dir, 3 * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
