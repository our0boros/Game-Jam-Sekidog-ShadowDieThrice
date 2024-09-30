using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayPlayerData : MonoBehaviour {

    public GameManager gameManager;
    [Header("是否启用玩家动作重播（该操作会被GM自动管理）")]
    public bool replay;

    private List<RecordPlayerData.PlayerData> playerDataList; // 存储记录的数据
    private int currentIndex = 0;

    public float replayInterval = 0.005f; // 重现间隔

    void Start() {
        StartCoroutine(ReplayData());
    }

    private IEnumerator ReplayData() {
        while (true) {
            if (replay & gameManager.isGameStart & currentIndex < playerDataList.Count - 1) {
                var data = playerDataList[currentIndex];
                transform.position = data.position;
                transform.rotation = data.rotation;
                if (data.attack) {
                    GetComponent<PlayerShooting>().Shoot();
                }

                // 根据需要处理动作数据，比如播放动画等
                //Debug.Log($"Replaying: {data.ToString()}");
                currentIndex++;
            }
            yield return new WaitForSecondsRealtime(replayInterval);
        }
    }

    public void setPlayerData(List<RecordPlayerData.PlayerData> playerDatas) {
        playerDataList = playerDatas;
    }
    /**
     * 是否从头开始重新播放录制
     */
    public void RestartReplay() {
        currentIndex = 0;
    }
}
