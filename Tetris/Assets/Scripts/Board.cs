using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set;}
    public Piece activePiece { get; private set;}
    public TetrominoData[] tetrominos;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public TextMesh scoreboard;
    public readonly RectInt bounds;
    public int score { get; private set; }

    public Board()
    {
        var position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
        bounds = new RectInt(position, boardSize);
    }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        this.score = 0;

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

        if (!IsValidPosition(this.activePiece, this.spawnPosition))
        {
            GameOver();
            return;
        }

        Set(activePiece);
    }

    private void GameOver()
    {
        this.tilemap.ClearAllTiles();
        this.activePiece.stepDelay = 1;
        this.score = 0;
        this.scoreboard.text = $"Score: {this.score}";
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

    public void ClearLines()
    {
        var row = this.bounds.yMin;
        var clearedLines = 0;

        while (row < this.bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
                clearedLines++;
            }
            else
            {
                row++;
            }
        }

        this.score += 10 * clearedLines;
        this.scoreboard.text = $"Score: {this.score}";
        this.activePiece.stepDelay = 1f - score / 1000f;
    }

    private bool IsLineFull( int row)
    {
        for (int col = this.bounds.xMin; col < this.bounds.xMax; col++)
        {
            var postition = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(postition))
            {
                return false;
            }
        }

        return true;
    }

    private void LineClear(int row)
    {
        for (int col = this.bounds.xMin; col < this.bounds.xMax; col++)
        {
            var postition = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(postition, null);
        }

        while (row < bounds.yMax)
        {
            for (int col = this.bounds.xMin; col < bounds.xMax; col++)
            {
                var postition = new Vector3Int(col, row + 1, 0);
                var tileAbove = this.tilemap.GetTile(postition);
                postition = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(postition, tileAbove);
            }

            row++;
        }
    }
}
