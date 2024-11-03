using Unity.Netcode;
using UnityEngine;

public class BoardCreation : MonoBehaviour
{
    [SerializeField] GameObject boardCell;
    [SerializeField] float board_spacing = 1.25f;
    public float Board_Spacing => board_spacing;

    public GameObject[,] board = new GameObject[6, 7];

    private void Awake()
    {
        for (int col = 0; col < board.GetLength(0); col++)
        {
            for (int row = 0; row < board.GetLength(1); row++)
            {
                //NetworkObject newCell = NetworkManager.SpawnManager.InstantiateAndSpawn(boardCell, position: transform.position);
                GameObject newCell = Instantiate(boardCell, transform);
                newCell.transform.position += new Vector3(board_spacing * row, -board_spacing * col);
                //Instantiate(boardCell, new Vector3(transform.position.x + (board_spacing * row), transform.position.y - (board_spacing * col)), Quaternion.identity);
                board[col, row] = newCell;
                newCell.GetComponent<BoardCell>().boardPos = new Vector2(row, col);
            }
        }
    }
}
