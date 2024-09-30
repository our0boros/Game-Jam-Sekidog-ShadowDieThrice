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
                // ������Ҫ��������ӻغϽ������߼�
                // manager.EndRound();
            }
        }
    }

    void UpdateClockHand()
    {
        float rotationAmount = 360f * (1f - (currentTime / totalTime));
        clockHand.transform.rotation = Quaternion.Euler(0f, 0f, -rotationAmount);
    }
    // ���һ�����÷������Ա�����Ϸ��ʼ�����¿�ʼʱ����
    public void ResetTimer()
    {
        currentTime = totalTime;
        UpdateClockHand();
    }

    void SetupClockSize()
    {
        // ��������������������ӱ�ĳ�ʼ��С
        // �������Ҫ������Ļ��С��̬�����ӱ��С����������������߼�
    }
}