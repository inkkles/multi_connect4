using Unity.Netcode;
using UnityEngine;

public class BoardCell : NetworkBehaviour
{
    [SerializeField] SpriteRenderer sr;
    public Vector2 boardPos;

    public bool IsFilled = false;

    [Rpc(SendTo.Everyone)]
    public void ChangeColorRpc(int team)
    {
        if (team == -1)
        {
            sr.color = Color.black;
            IsFilled = false;
            return;
        }
        if(team == 0) sr.color = Color.red;
        if(team == 1) sr.color = Color.blue;
        IsFilled = true;
    }
}
