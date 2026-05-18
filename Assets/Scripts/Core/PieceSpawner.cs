using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public PieceFactory factory;

    public Vector3 spawnPosition =
        new Vector3(4, 15, 0);

    private PieceData nextPieceData;

    private void Start()
    {
        GenerateNextPiece();

        SpawnPiece();
    }

    public void SpawnPiece()
    {
        PieceData currentData = nextPieceData;

        GenerateNextPiece();

        UIManager.Instance.UpdateNextPiece(
            nextPieceData.id
        );

        GameObject piece =
            factory.CreatePiece(
                currentData,
                spawnPosition
            );

        if (!GridManager.Instance.IsValidPosition(piece.transform))
        {
            UIManager.Instance.ShowGameOver();

            Destroy(piece);

            return;
        }

        MobileInput input =
            FindFirstObjectByType<MobileInput>();

        if (input != null)
        {
            input.currentPiece =
                piece.GetComponent<Piece>();
        }
    }

    private void GenerateNextPiece()
    {
        int randomIndex =
            Random.Range(
                0,
                ConfigLoader.Instance.database.pieces.Count
            );

        nextPieceData =
            ConfigLoader.Instance.database.pieces[randomIndex];
    }
}