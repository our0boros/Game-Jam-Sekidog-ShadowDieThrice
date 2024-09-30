using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public bool isGameStart;
    public int currentRound = 1;
    public float roundDuration = 15f; // 回合时长（秒）
    public float endingTime = 5f; // 等待回合结算时长
    public float startTime;
    public float remainTime;

    [Header("游戏开始按钮")]
    public KeyCode startNewRound = KeyCode.Return;
    [Header("三场比赛中分别用到的两个战斗角色")]
    public GameObject[] players;
    [Header("角色投影以及本身的材质")]
    public Material[] materials;

    private Coroutine roundCoroutine;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start() {
        isGameStart = false;

        // Reset player materials
        int playerIdx = 0;
        foreach (GameObject gameObject in players) {
            // Reset player movement
            gameObject.GetComponent<PlayerMovementCC>().DeactivateMovement();
            // Reset layer and material
            gameObject.layer = LayerMask.NameToLayer("Player");
            if (playerIdx % 2 == 0) {
                // Red team
                SwitchMaterial(gameObject, 0);
            } else {
                // Blue team
                SwitchMaterial(gameObject, 2);
            }
            playerIdx++;
            // hide all objects
            gameObject.SetActive(false);
            // record and replay false
            gameObject.GetComponent<RecordPlayerData>().record = false;
            gameObject.GetComponent<ReplayPlayerData>().replay = false;
            // 共享playerdata 数据地址
            gameObject.GetComponent<ReplayPlayerData>().setPlayerData(
                gameObject.GetComponent<RecordPlayerData>().GetPlayerData());
        }
    }

    void Update() {
        if(remainTime < -endingTime)
        {
            UIManager.Instance.ShowMessage(1);
        }

        if (Input.GetKeyDown(startNewRound) & (remainTime < -endingTime | startTime == 0)) {
            isGameStart = !isGameStart;
            Debug.Log("[GameManager] Current Game is " + (isGameStart ? "Start" : "Finish"));

            if (isGameStart) {
                Debug.Log("[GameManager] Current Round is " + currentRound);
                UIManager.Instance.ShowMessage(currentRound + 1);
                if (roundCoroutine != null) {
                    StopCoroutine(roundCoroutine);
                }
                roundCoroutine = StartCoroutine(RoundTimer(roundDuration));

                StartRound();
            }
        }
        remainTime = roundDuration - Time.time + startTime;
    }

    void StartRound() {
        startTime = Time.time;
        switch (currentRound) {
            case 1:
                AudioManager.Instance.PlayRound1BGM();
                ActivatePlayers(0, 1);
                break;
            case 2:
                AudioManager.Instance.PlayRound2BGM();
                ActivatePlayers(2, 3);
                ReplayPlayers(0, 1, 1, 3, "Shadow");
                ResetPreviousReplay(0, 1);
                break;
            case 3:
                AudioManager.Instance.PlayRound3BGM();
                ActivatePlayers(4, 5);
                ReplayPlayers(0, 1, 1, 3, "Shadow");
                ReplayPlayers(2, 3, 1, 3, "Shadow");
                ResetPreviousReplay(0, 1);
                ResetPreviousReplay(2, 3);
                break;
        }
    }

    void EndRound() {
        isGameStart = false;
        if(currentRound == 3)
            UIManager.Instance.ShowMessage(6);
        else {
            UIManager.Instance.ShowMessage(5);
            //倒带声音
            AudioManager.Instance.PlayEndGameBGM();
        }
        
        
        foreach (GameObject gameObject in players) {
            gameObject.GetComponent<PlayerMovementCC>().DeactivateMovement();
            gameObject.GetComponent<PlayerHealth>().ResetHealth();
            gameObject.GetComponent<RecordPlayerData>().record = false;
        }
        currentRound++;
        if (currentRound == 4)
            currentRound = 1;
    }

    void ActivatePlayers(int index1, int index2) {
        players[index1].SetActive(true);
        players[index2].SetActive(true);
        players[index1].GetComponent<PlayerMovementCC>().ActivateMovement();
        players[index2].GetComponent<PlayerMovementCC>().ActivateMovement();
        players[index1].GetComponent<RecordPlayerData>().record = true;
        players[index2].GetComponent<RecordPlayerData>().record = true;
    }

    void ReplayPlayers(int index1, int index2, int material1, int material2, string layer) {
        players[index1].GetComponent<ReplayPlayerData>().replay = true;
        players[index2].GetComponent<ReplayPlayerData>().replay = true;
        SwitchMaterial(players[index1], material1);
        SwitchMaterial(players[index2], material2);
        players[index1].layer = LayerMask.NameToLayer(layer);
        players[index2].layer = LayerMask.NameToLayer(layer);
    }

    void ResetPreviousReplay(int index1, int index2) {
        players[index1].GetComponent<ReplayPlayerData>().RestartReplay();
        players[index2].GetComponent<ReplayPlayerData>().RestartReplay();
    }

    IEnumerator RoundTimer(float duration) {
        yield return new WaitForSeconds(duration);
        isGameStart = false;
        Debug.Log("[GameManager] Time's up! Ending round.");
        EndRound();
    }

    void SwitchMaterial(GameObject gameObject, int materialIdx) {
        gameObject.GetComponent<RecordPlayerData>().renderer.material = materials[materialIdx];
    }
}
