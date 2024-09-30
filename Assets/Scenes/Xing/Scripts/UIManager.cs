using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Round Messages")]
    public string hint;
    public string round1 = "Round 1";
    public string round2 = "Round 2";
    public string round3 = "Round 3";
    public string roundOver;
    public string end = "Game Over";

    [Header("UI References")]
    public Text messageText;
    public CanvasGroup messageCanvasGroup;

    [Header("Animation Settings")]
    public float fadeDuration = 0.5f;
    public float displayDuration = 2f;

    private Coroutine displayCoroutine;
    private bool isDisplaying = false;

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

    private void Start()
    {
        if (messageCanvasGroup != null)
        {
            messageCanvasGroup.alpha = 0f;
        }
        ShowMessage(1);
    }

    public void ShowMessage(int roundNumber)
    {
        string message = GetMessageForRound(roundNumber);
        if (!string.IsNullOrEmpty(message))
        {
            if (isDisplaying)
            {
                // 如果当前正在显示消息，停止当前的协程
                if (displayCoroutine != null)
                {
                    StopCoroutine(displayCoroutine);
                }
                // 立即隐藏当前消息
                HideMessage();
                if (roundNumber == 4)
                {
                    StartCoroutine(LoadSceneAfter(GameManager.Instance.roundDuration + 3));
                   
                }
            }
            // 开始显示新消息
            displayCoroutine = StartCoroutine(DisplayMessageCoroutine(message));
        }
    }

    private string GetMessageForRound(int roundNumber)
    {
        switch (roundNumber)
        {
            case 1: return hint;
            case 2: return round1;
            case 3: return round2;
            case 4: return round3;
            case 5: return roundOver;
            case 6: return end;
            default:
                Debug.LogWarning("Invalid round number: " + roundNumber);
                return "";
        }
    }

    private IEnumerator DisplayMessageCoroutine(string message)
    {
        isDisplaying = true;
        if (messageText != null && messageCanvasGroup != null)
        {
            messageText.text = message;
            yield return messageCanvasGroup.DOFade(1f, fadeDuration).WaitForCompletion();
            yield return new WaitForSeconds(displayDuration);
            yield return messageCanvasGroup.DOFade(0f, fadeDuration).WaitForCompletion();
        }
        else
        {
            Debug.LogError("Message Text or CanvasGroup is not set in the UIManager");
        }
        isDisplaying = false;
    }

    public void HideMessage()
    {
        if (messageCanvasGroup != null)
        {
            messageCanvasGroup.alpha = 0f;
        }
        isDisplaying = false;
    }

    private IEnumerator LoadSceneAfter(float second)
    {
        yield return new WaitForSeconds(second);
        SceneManager.LoadScene("Loading Scene");
    }
}