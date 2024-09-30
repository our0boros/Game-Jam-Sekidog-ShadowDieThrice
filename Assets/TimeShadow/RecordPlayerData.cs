using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordPlayerData : MonoBehaviour {

    // ����һ�������洢ÿ��ʱ��������
    [System.Serializable]
    public class PlayerData {
        public float timeStamp;
        public Vector3 position;
        public Quaternion rotation;
        public string action;
        public bool attack;

        public PlayerData(float timeStamp, Vector3 position, Quaternion rotation, string action, bool attack) {
            this.timeStamp = timeStamp;
            this.position = position;
            this.rotation = rotation;
            this.action = action;
            this.attack = attack;
        }

        public override string ToString() {
            return $"TimeStamp: {timeStamp}, Position: {position}, " +
                $"Rotation: {rotation.eulerAngles}, Action: {action}, Attack: " 
                + (attack ? "True" : "False");
        }
    }
    // ==============================================================


    public GameManager gameManager;

    public bool isAlive;
    [Header("����Ҫ�޸ĵ�Render")]
    public Renderer renderer;
    [Header("�Ƿ�������Ҷ���¼�ƣ��ò����ᱻGM�Զ�����")]
    public bool record;
    public float recordInterval = 0.005f; // ��¼���
    private List<PlayerData> playerDataList = new List<PlayerData>(); // �洢���ݵ��б�
    private float elapsedTime = 0f;     // �Ѿ���ȥ��ʱ��

    void Start() {
        StartCoroutine(RecordData());
        isAlive = true;
    }

    private IEnumerator<WaitForSecondsRealtime> RecordData() {
        while (true) {
            elapsedTime += recordInterval;
            if (record & gameManager.isGameStart) {
                //string action = DetermineCurrentAction();
                playerDataList.Add(new PlayerData(
                    elapsedTime, 
                    transform.position, 
                    transform.rotation, 
                    "null", 
                    GetComponent<PlayerShooting>().shootABullet));
                // �����¼��һ������ͽ�������
                if (GetComponent<PlayerShooting>().shootABullet) {
                    GetComponent<PlayerShooting>().shootABullet = false;
                }
                
            }
            yield return new WaitForSecondsRealtime(recordInterval);
        }
    }

    // �ṩһ����������ȡ��¼������
    public List<PlayerData> GetPlayerData() {
        return playerDataList;
    }

    public void ResetPlayerData() {
        playerDataList = new List<PlayerData>();
    }
}
