using UnityEngine;
using UnityEngine.UI;
using System;

public class GameStartUI : MonoBehaviour
{
    public Button startGameButton;
    public GameObject startPanel;

    public static event Action OnGameStart;

    private void Start()
    {
        if (startGameButton != null)
        {
            startGameButton.onClick.AddListener(StartGame);
        }
        else
        {
            Debug.LogError("Start Game Button is not assigned in the GameStartUI script!");
        }

        // 确保开始面板在游戏开始时是可见的
        if (startPanel != null)
        {
            startPanel.SetActive(true);
        }
    }

    private void StartGame()
    {
        //测试 播放bgm
        //AudioManager.Instance.PlayRound1BGM();
        //测试 开始第一回合
        //GameManager.Instance.StartRound();
        // 触发游戏开始事件
        OnGameStart?.Invoke();

        // 隐藏开始面板
        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }

        // 禁用这个脚本，因为我们不再需要它了
        this.enabled = false;
    }
}