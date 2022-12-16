using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField]
    private Player player;

    private NetworkVariable<int> state = new NetworkVariable<int>(
        100,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );
    public int State
    {
        get { return state.Value; }
    }

    [ClientRpc]
    public void ReceiveHitClientRpc(int damage, ClientRpcParams clientRpcParams = default)
    {
        state.Value -= damage;
        if (state.Value == 0)
        {
            player.Alive = false;
        }
    }
}