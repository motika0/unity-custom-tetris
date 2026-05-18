using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Grid Settings")]
    public int width = 10;
    public int height = 20;

    private Transform[,] grid;

    private void Awake()
    {
        Instance = this;

        grid = new Transform[width, height];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position =
                    transform.position +
                    new Vector3(x, y, 0);

                Gizmos.DrawWireCube(position, Vector3.one);
            }
        }
    }

    public bool IsInsideGrid(Vector2 position)
    {
        return
            position.x >= 0 &&
            position.x < width &&
            position.y >= 0;
    }

    public bool IsValidPosition(Transform piece)
    {
        foreach (Transform block in piece)
        {
            Vector2 position =
                RoundVector(
                    block.position - transform.position
                );

            if (!IsInsideGrid(position))
                return false;

            int x = Mathf.RoundToInt(position.x);
            int y = Mathf.RoundToInt(position.y);

            if (y >= height)
                continue;

            // Проверка занятости
            if (grid[x, y] != null)
            {
                // Игнорируем блоки этой же фигуры
                if (grid[x, y].parent != piece)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private Vector2 RoundVector(Vector2 vector)
    {
        return new Vector2(
            Mathf.Round(vector.x),
            Mathf.Round(vector.y)
        );
    }

    public void AddBlockToGrid(Transform block, int x, int y)
    {
        grid[x, y] = block;
    }

    // =========================
    // LINE CLEAR
    // =========================

    public void CheckLines()
    {
        int clearedLines = 0;

        for (int y = 0; y < height; y++)
        {
            if (IsLineFull(y))
            {
                ClearLine(y);

                MoveAllLinesDown(y + 1);

                y--;

                clearedLines++;
            }
        }

        if (clearedLines > 0)
        {
            ScoreManager.Instance.AddScore(clearedLines);
        }
    }

    private bool IsLineFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    private void ClearLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);

            grid[x, y] = null;
        }
    }

    private void MoveAllLinesDown(int startY)
    {
        for (int y = startY; y < height; y++)
        {
            MoveLineDown(y);
        }
    }

    private void MoveLineDown(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                // Перемещаем ссылку
                grid[x, y - 1] = grid[x, y];

                grid[x, y] = null;

                // Двигаем объект
                grid[x, y - 1].position += Vector3.down;
            }
        }
    }
}