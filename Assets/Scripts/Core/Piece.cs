using UnityEngine;

public class Piece : MonoBehaviour
{
    [Header("Movement")]
    private float fallTime;

    private float fallTimer;

    private PieceSpawner spawner;

    [HideInInspector]
    public PieceData pieceData;

    private void Start()
    {
        fallTime =
            Mathf.Clamp(
                1f - (ScoreManager.Instance.level - 1) * 0.1f,
                0.15f,
                1f
            );

        spawner = FindFirstObjectByType<PieceSpawner>();
    }

    private void Update()
    {
        HandleInput();
        HandleFall();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector2.left);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector2.right);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Rotate();
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            Move(Vector2.down, false);
        }
    }

    private void HandleFall()
    {
        fallTimer += Time.deltaTime;

        if (fallTimer >= fallTime)
        {
            Move(Vector2.down);

            fallTimer = 0f;
        }
    }

    private bool Move(Vector2 direction, bool lockPiece = true)
    {
        transform.position += (Vector3)direction;

        // SNAP TO GRID
        transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            0
        );

        if (!GridManager.Instance.IsValidPosition(transform))
        {
            transform.position -= (Vector3)direction;

            transform.position = new Vector3(
                Mathf.Round(transform.position.x),
                Mathf.Round(transform.position.y),
                0
            );

            if (direction == Vector2.down && lockPiece)
            {
                LandPiece();
            }

            return false;
        }

        // SIMPLE SCALE ANIMATION
        foreach (Transform block in transform)
        {
            block.localScale = Vector3.one * 0.9f;
        }

        return true;
    }

    public void HardDrop()
    {
        while (Move(Vector2.down, false))
        {

        }

        LandPiece();
    }

    private void LandPiece()
    {
        AddToGrid();

        GridManager.Instance.CheckLines();

        enabled = false;

        spawner.SpawnPiece();
    }

    private void AddToGrid()
    {
        foreach (Transform block in transform)
        {
            Vector2 position =
                block.position - GridManager.Instance.transform.position;

            int x = Mathf.RoundToInt(position.x);
            int y = Mathf.RoundToInt(position.y);

            if (y >= GridManager.Instance.height)
            {
                GameManager.Instance.GameOver();

                return;
            }

            if (
                x >= 0 &&
                x < GridManager.Instance.width &&
                y >= 0 &&
                y < GridManager.Instance.height
            )
            {
                GridManager.Instance.AddBlockToGrid(block, x, y);
            }
        }
    }

    private void Rotate()
    {
        Vector2 pivot =
            new Vector2(
                pieceData.pivot.x,
                pieceData.pivot.y
            );

        foreach (Transform block in transform)
        {
            Vector2 position = block.localPosition;

            position -= pivot;

            float x = position.x;
            float y = position.y;

            position.x = -y;
            position.y = x;

            position += pivot;

            block.localPosition =
                new Vector3(
                    Mathf.Round(position.x),
                    Mathf.Round(position.y),
                    0
                );
        }

        if (!GridManager.Instance.IsValidPosition(transform))
        {
            if (!TryWallKick())
            {
                RotateBack();
            }
        }
    }

    private void RotateBack()
    {
        Vector2 pivot =
            new Vector2(
                pieceData.pivot.x,
                pieceData.pivot.y
            );

        foreach (Transform block in transform)
        {
            Vector2 position = block.localPosition;

            position -= pivot;

            float x = position.x;
            float y = position.y;

            position.x = y;
            position.y = -x;

            position += pivot;

            block.localPosition =
                new Vector3(
                    Mathf.Round(position.x),
                    Mathf.Round(position.y),
                    0
                );
        }
    }

    private bool TryWallKick()
    {
        Vector2[] offsets =
        {
            Vector2.right,
            Vector2.left,
            Vector2.up,
            Vector2.right * 2,
            Vector2.left * 2
        };

        foreach (Vector2 offset in offsets)
        {
            transform.position += (Vector3)offset;

            if (GridManager.Instance.IsValidPosition(transform))
            {
                return true;
            }

            transform.position -= (Vector3)offset;
        }

        return false;
    }

    public void MoveLeft()
    {
        Move(Vector2.left);
    }

    public void MoveRight()
    {
        Move(Vector2.right);
    }

    public void MoveDown()
    {
        Move(Vector2.down);
    }

    public void RotatePiece()
    {
        Rotate();
    }

    public void HardDropPiece()
    {
        HardDrop();
    }
}