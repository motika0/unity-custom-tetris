using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject gameOverPanel;
    public TextMeshProUGUI nextPieceText;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void UpdateNextPiece(string pieceId)
    {
        nextPieceText.text = pieceId;
    }
}