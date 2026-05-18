using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score;

    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI levelText;

    public TextMeshProUGUI highScoreText;

    public int level = 1;

    public int highScore;

    private int totalLinesCleared;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        highScore =
    PlayerPrefs.GetInt(
        "HighScore",
        0
    );

        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = "Score: " + score;

        levelText.text = "Level: " + level;

        highScoreText.text = "High Score: " + highScore;
    }

    public void AddScore(int lines)
    {
        switch (lines)
        {
            case 1:
                score += 100;
                break;

            case 2:
                score += 300;
                break;

            case 3:
                score += 500;
                break;

            case 4:
                score += 800;
                break;
        }

        totalLinesCleared += lines;

        UpdateLevel();

        if (score > highScore)
        {
            highScore = score;

            PlayerPrefs.SetInt(
                "HighScore",
                highScore
            );

            PlayerPrefs.Save();
        }

        UpdateUI();

        Debug.Log("Score: " + score);
    }

    private void UpdateLevel()
    {
        level =
            1 + totalLinesCleared / 10;

        Debug.Log("LEVEL: " + level);
    }
}