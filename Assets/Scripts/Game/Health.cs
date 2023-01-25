using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField]
    private Player player;

    private NetworkVariable<int> current = new NetworkVariable<int>(
        100,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );
    private NetworkVariable<int> max = new NetworkVariable<int>(
        100,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );
    public int Current
    {
        get { return current.Value; }
    }
    public int Max
    {
        get { return max.Value; }
    }

    [ClientRpc]
    public void ReceiveHitClientRpc(int damage, ClientRpcParams clientRpcParams = default)
    {
        current.Value -= damage;
        if (current.Value == 0)
        {
            player.Alive = false;
        }
    }
}