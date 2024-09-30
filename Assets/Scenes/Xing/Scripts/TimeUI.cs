using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{

    public Image clockFace;
    public Image clockHand;
    public float totalTime = 15f;

    private float currentTime;

    public GameManager manager;

    void Start()
    {
        currentTime = totalTime;
        SetupClockSize();
    }

    void Update()
    {
        if (manager.isGameStart)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                UpdateClockHand();
            }
            else
            {
                currentTime = totalTime;
                // 可能需要在这里添加回合结束的逻辑
                // manager.EndRound();
            }
        }
    }

    void UpdateClockHand()
    {
        float rotationAmount = 360f * (1f - (currentTime / totalTime));
        clockHand.transform.rotation = Quaternion.Euler(0f, 0f, -rotationAmount);
    }
    // 添加一个重置方法，以便在游戏开始或重新开始时调用
    public void ResetTimer()
    {
        currentTime = totalTime;
        UpdateClockHand();
    }

    void SetupClockSize()
    {
        // 这个方法可以用来设置钟表的初始大小
        // 如果您想要根据屏幕大小动态设置钟表大小，可以在这里添加逻辑
    }
}