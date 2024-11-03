using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Player : NetworkBehaviour
{
    bool myTurn = false;
    GameObject selectionUI;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        selectionUI = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        myTurn = NetworkObjectId == 1; //Host always gets first turn
    }

    // Update is called once per frame
    void Update()
    {
        //selectionUI.SetActive(myTurn);

        if (IsClient && IsOwner)// && myTurn && GameManager.singleton.Start)
        {
            float col = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - (GameManager.singleton.transform.position.x);

            selectionUI.transform.position = new Vector3(Mathf.Round(col / GameManager.singleton.Board_Spacing)*GameManager.singleton.Board_Spacing + GameManager.singleton.transform.position.x, GameManager.singleton.transform.position.y + GameManager.singleton.Board_Spacing * GameManager.singleton.board.GetLength(0), 0);

            if (Input.GetMouseButtonDown(0))
            {
                //Destroy(selectionUI.gameObject);
                PlaceTileServerRpc(NetworkObjectId, Mathf.RoundToInt(col/ GameManager.singleton.Board_Spacing));
            }
        }
    }

    [Rpc(SendTo.Server)]
    public void PlaceTileServerRpc(ulong networkID, int col)
    {
        Debug.Log("P " + networkID + ":\t" + col);
        myTurn = !myTurn;
        SwapTurnsRpc();
        GameManager.singleton.PlaceTile(col, networkID == 1 ? 0 : 1); //host will always be team 0, all other clients will be team 1
    }

    [Rpc(SendTo.NotServer)]
    void SwapTurnsRpc()
    {
        myTurn = !myTurn;
    }
}
