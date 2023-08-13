using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set;}
    public Piece activePiece { get; private set;}
    public TetrominoData[] tetrominos;
    public Vector3Int spawnPosition;

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < tetrominos.Length; i++)
        {
            this.tetrominos[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    private void SpawnPiece()
    {
        var random = Random.Range(0, tetrominos.Length);
        var data = tetrominos[random];

        this.activePiece.Initialize(this, this.spawnPosition, data);
        Set(activePiece);
    }

    public void Set(Piece piece)
    {
        foreach (var cellPosition in piece.cells)
        {
            var tilePosition = cellPosition + piece.position;
            this.tilemap.SetTile(tilePosition, piece.tetrominoData.tile);
        }
    }
}
