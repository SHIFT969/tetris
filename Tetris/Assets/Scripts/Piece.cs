using System.Linq;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public Vector3Int position { get; private set; }
    public TetrominoData tetrominoData { get; private set; }
    public Vector3Int[] cells { get; private set; }

    public void Initialize(Board board, Vector3Int position, TetrominoData tetrominoData)
    {
        this.board = board;
        this.position = position;
        this.tetrominoData = tetrominoData;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[tetrominoData.cells.Length];
        }

        cells = tetrominoData.cells.Select(x => (Vector3Int)x).ToArray();
    }
}
