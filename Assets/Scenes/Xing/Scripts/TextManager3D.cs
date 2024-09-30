using UnityEngine;
using TMPro;

public class TextManager3D : MonoBehaviour
{
    public static TextManager3D Instance { get; private set; }

    public TextMeshPro textMeshPro;
    public float yOffset = 2f; // 文本在屏幕上方的偏移量

    private Camera mainCamera;

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

        mainCamera = Camera.main;
        if (textMeshPro == null)
        {
            textMeshPro = GetComponentInChildren<TextMeshPro>();
        }
    }

    private void LateUpdate()
    {
        if (textMeshPro != null && mainCamera != null)
        {
            // 将文本放置在屏幕上方
            Vector3 screenTop = new Vector3(Screen.width / 2f, Screen.height - yOffset, 10f);
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenTop);

            // 使文本面向摄像机
            transform.position = worldPosition;
            transform.rotation = mainCamera.transform.rotation;
        }
    }

    public void SetText(string message)
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = message;
        }
    }

    public void SetTextColor(Color color)
    {
        if (textMeshPro != null)
        {
            textMeshPro.color = color;
        }
    }

    public void SetFontSize(float size)
    {
        if (textMeshPro != null)
        {
            textMeshPro.fontSize = size;
        }
    }

    public void SetFont(TMP_FontAsset font)
    {
        if (textMeshPro != null)
        {
            textMeshPro.font = font;
        }
    }
}