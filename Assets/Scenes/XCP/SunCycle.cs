using UnityEngine;

public class SunController : MonoBehaviour
{
    private float dayDuration; // һ��ĳ���ʱ�䣨�룩
    public Vector3 initialRotation = new Vector3(50, -30, 0); // ��ʼ��ת�Ƕ�
    public float reverseSpeedMultiplier = 10f; // ������ת���ٶȱ���

    public GameManager GameManager;

    private float rotationSpeed;
    private Quaternion startRotation;
    private float elapsedTime;
    private bool isReversing;
    private bool isStopped;

    public bool sunHasReseted;

    void Start()
    {
        sunHasReseted = false;
        dayDuration = GameManager.Instance.roundDuration;
        rotationSpeed = 360f / dayDuration;
        startRotation = Quaternion.Euler(initialRotation);
        transform.rotation = startRotation;
        elapsedTime = 0f;
        isReversing = false;
        isStopped = false;
    }

    void Update()
    {
        if (!isStopped)
        {
            if (!isReversing)
            {
                NormalRotation();
            }
            else
            {
                ReverseRotation();
            }

            if (elapsedTime >= dayDuration && !isReversing)
            {
                isReversing = true;
                elapsedTime = 0f;
            }
        }

        if (GameManager.Instance.isGameStart & !sunHasReseted)
        {
            ResetSun();
            sunHasReseted = true;
        }

        if (!GameManager.Instance.isGameStart) {
            sunHasReseted = false;
        }
    }

    void NormalRotation()
    {
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        elapsedTime += Time.deltaTime;
    }

    void ReverseRotation()
    {
        float reverseRotationSpeed = rotationSpeed * reverseSpeedMultiplier;
        transform.Rotate(-Vector3.right * reverseRotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, startRotation) < 1f)
        {
            transform.rotation = startRotation;
            isStopped = true;
        }
    }

    void ResetSun()
    {
        transform.rotation = startRotation;
        elapsedTime = 0f;
        isReversing = false;
        isStopped = false;
    }
}
