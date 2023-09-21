using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    GameObject GameListView;

    void Start()
    {
        GameListView = GameObject.Find("GameListView");
        GameListView.SetActive(false);
    }

    public void OnButtonClick(GameObject button)
    {
        switch (button.name)
        {
            case "StartButton":
                GameListView.SetActive(true);
                break;
            case "ExitButton":
                GameListView.SetActive(false);
                break;
            case "JumpingButton":
                SceneManager.LoadScene("Jumping");
                break;
            case "QuitButton":
                Application.Quit();
                break;
        }
    }
}
