using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordPlayerData : MonoBehaviour {

    // 定义一个类来存储每个时间点的数据
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
    [Header("绑定需要修改的Render")]
    public Renderer renderer;
    [Header("是否启用玩家动作录制（该操作会被GM自动管理）")]
    public bool record;
    public float recordInterval = 0.005f; // 记录间隔
    private List<PlayerData> playerDataList = new List<PlayerData>(); // 存储数据的列表
    private float elapsedTime = 0f;     // 已经过去的时间

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
                // 如果记录到一次射击就将其重置
                if (GetComponent<PlayerShooting>().shootABullet) {
                    GetComponent<PlayerShooting>().shootABullet = false;
                }
                
            }
            yield return new WaitForSecondsRealtime(recordInterval);
        }
    }

    // 提供一个方法来获取记录的数据
    public List<PlayerData> GetPlayerData() {
        return playerDataList;
    }

    public void ResetPlayerData() {
        playerDataList = new List<PlayerData>();
    }
}
