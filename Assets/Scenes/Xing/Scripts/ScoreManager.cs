using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int player1Score = 0;
    private int player2Score = 0;

    public delegate void ScoreChangedDelegate(int playerNumber, int newScore);
    public event ScoreChangedDelegate OnScoreChanged;

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

    public void AddScore(int playerNumber, int points)
    {
        if (playerNumber == 1)
        {
            player1Score += points;
            OnScoreChanged?.Invoke(1, player1Score);
        }
        else if (playerNumber == 2)
        {
            player2Score += points;
            OnScoreChanged?.Invoke(2, player2Score);
        }
    }

    public int GetScore(int playerNumber)
    {
        return playerNumber == 1 ? player1Score : player2Score;
    }

    public void ResetScores()
    {
        player1Score = 0;
        player2Score = 0;
        OnScoreChanged?.Invoke(1, player1Score);
        OnScoreChanged?.Invoke(2, player2Score);
    }
}