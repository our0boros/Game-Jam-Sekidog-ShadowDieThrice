using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayPlayerData : MonoBehaviour {

    public GameManager gameManager;
    [Header("�Ƿ�������Ҷ����ز����ò����ᱻGM�Զ�����")]
    public bool replay;

    private List<RecordPlayerData.PlayerData> playerDataList; // �洢��¼������
    private int currentIndex = 0;

    public float replayInterval = 0.005f; // ���ּ��

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

                // ������Ҫ���������ݣ����粥�Ŷ�����
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
     * �Ƿ��ͷ��ʼ���²���¼��
     */
    public void RestartReplay() {
        currentIndex = 0;
    }
}
