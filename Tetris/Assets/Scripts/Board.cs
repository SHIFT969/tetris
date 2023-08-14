using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set;}
    public Piece activePiece { get; private set;}
    public TetrominoData[] tetrominos;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public readonly RectInt bounds;

    public Board()
    {
        var position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
        bounds = new RectInt(position, boardSize);
    }

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

    public void SpawnPiece()
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

    public void Clear(Piece piece)
    {
        foreach (var cellPosition in piece.cells)
        {
            var tilePosition = cellPosition + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        foreach (var cellPosition in piece.cells)
        {
            var tilePosition = cellPosition + position;

            if (!bounds.Contains((Vector2Int)tilePosition)
                || this.tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }
}
