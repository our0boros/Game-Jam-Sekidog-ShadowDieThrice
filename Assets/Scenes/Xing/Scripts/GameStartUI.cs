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

        // ȷ����ʼ�������Ϸ��ʼʱ�ǿɼ���
        if (startPanel != null)
        {
            startPanel.SetActive(true);
        }
    }

    private void StartGame()
    {
        //���� ����bgm
        //AudioManager.Instance.PlayRound1BGM();
        //���� ��ʼ��һ�غ�
        //GameManager.Instance.StartRound();
        // ������Ϸ��ʼ�¼�
        OnGameStart?.Invoke();

        // ���ؿ�ʼ���
        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }

        // ��������ű�����Ϊ���ǲ�����Ҫ����
        this.enabled = false;
    }
}