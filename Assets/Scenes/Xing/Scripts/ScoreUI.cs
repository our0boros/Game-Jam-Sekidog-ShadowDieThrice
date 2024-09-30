using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ScoreUI : MonoBehaviour
{
    [System.Serializable]
    public class PlayerScoreUI
    {
        public TextMeshProUGUI scoreText;
        public RectTransform scoreTransform;
    }

    public PlayerScoreUI player1ScoreUI;
    public PlayerScoreUI player2ScoreUI;

    [Header("Animation Settings")]
    public float animationDuration = 0.5f;
    public float scaleFactor = 1.2f;

    private void Start()
    {
        ScoreManager.Instance.OnScoreChanged += UpdateScoreUI;
    }

    private void UpdateScoreUI(int playerNumber, int newScore)
    {
        PlayerScoreUI targetUI = (playerNumber == 1) ? player1ScoreUI : player2ScoreUI;
        targetUI.scoreText.text = newScore.ToString();

        // ¶¯»­Ð§¹û
        Sequence sequence = DOTween.Sequence();
        sequence.Append(targetUI.scoreTransform.DOScale(Vector3.one * scaleFactor, animationDuration / 2))
                .Append(targetUI.scoreTransform.DOScale(Vector3.one, animationDuration / 2));
    }

    private void OnDestroy()
    {
        ScoreManager.Instance.OnScoreChanged -= UpdateScoreUI;
    }
}