using UnityEngine;

public class PieceFactory : MonoBehaviour
{
    public GameObject blockPrefab;

    public Transform currentPieceParent;

    public GameObject CreatePiece(PieceData data, Vector3 spawnPosition)
    {
        GameObject pieceObject = new GameObject(data.id);

        pieceObject.transform.position = spawnPosition;

        foreach (BlockData block in data.blocks)
        {
            GameObject newBlock = Instantiate(
                blockPrefab,
                pieceObject.transform
            );

            newBlock.transform.localPosition =
                new Vector3(block.x, block.y, 0);

            SpriteRenderer renderer =
                newBlock.GetComponent<SpriteRenderer>();

            renderer.color = HexToColor(data.color);
        }

        Piece piece = pieceObject.AddComponent<Piece>();

        piece.pieceData = data;

        return pieceObject;
    }

    private Color HexToColor(string hex)
    {
        Color color;

        ColorUtility.TryParseHtmlString(hex, out color);

        return color;
    }
}