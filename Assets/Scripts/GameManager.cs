using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : NetworkBehaviour
{
    public static GameManager singleton;
    [SerializeField] NetworkObject boardCell;
    [SerializeField] float board_spacing = 1.25f;

    public bool Start;

    public float Board_Spacing => board_spacing;

    public NetworkObject[,] board = new NetworkObject[6,7];
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3(transform.position.x - board_spacing, transform.position.y - board_spacing), new Vector3(transform.position.x + board.GetLength(1)*board_spacing, transform.position.y - board_spacing));
        Gizmos.DrawLine(new Vector3(transform.position.x - board_spacing, transform.position.y - board_spacing), new Vector3(transform.position.x - board_spacing, transform.position.y + board.GetLength(0) * board_spacing));
    }


    //singleton logic
    private void Awake()
    {
        if (singleton == null) singleton = this;
        else Destroy(gameObject);
    }

    int playerCount = 0;
    

    //board creation
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer)
        {
            for (int col = 0; col < board.GetLength(0); col++)
            {
                for (int row = 0; row < board.GetLength(1); row++)
                {
                    NetworkObject newCell = NetworkManager.SpawnManager.InstantiateAndSpawn(boardCell, position: transform.position);
                    newCell.transform.position += new Vector3(board_spacing * row, board_spacing * col);
                    //Instantiate(boardCell, new Vector3(transform.position.x + (board_spacing * row), transform.position.y - (board_spacing * col)), Quaternion.identity);
                    board[col, row] = newCell;
                    newCell.GetComponent<BoardCell>().boardPos = new Vector2(row, col);
                }
            }
        }
    }

    //turn management

    public void PlaceTile(int col, int team)
    {
        for(int row = 0; row < board.GetLength(0); row++)
        {
            BoardCell currCell = board[row, col].GetComponent<BoardCell>();
            if (currCell.IsFilled) continue;

            currCell.ChangeColorRpc(team);
            checkForWin();
            return;
        }
        Debug.Log("Board Full!");
    }

    //TODO: BFS?
    void checkForWin() { }


}
