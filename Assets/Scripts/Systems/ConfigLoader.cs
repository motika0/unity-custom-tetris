using UnityEngine;

public class ConfigLoader : MonoBehaviour
{
    public static ConfigLoader Instance;

    public PieceDatabase database;

    private void Awake()
    {
        Instance = this;

        LoadConfig();
    }

    private void LoadConfig()
    {
        TextAsset json = Resources.Load<TextAsset>("pieces");

        database = JsonUtility.FromJson<PieceDatabase>(json.text);

        Debug.Log("Loaded pieces: " + database.pieces.Count);
    }
}