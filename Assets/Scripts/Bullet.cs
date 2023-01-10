using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float velocity;

    [SerializeField]
    private int damage = 1;

    private ulong shooter;
    public ulong Shooter { set { shooter = value; } }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5f);
        GetComponent<NetworkObject>().Despawn();
    }

    public override void OnNetworkSpawn()
    {
        StartCoroutine(SelfDestruct());
        rb.AddRelativeForce(new Vector3(0f, velocity, 0f));
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!IsServer) return;
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            ulong clientID = player.ClientID;
            Debug.Log(clientID + " | " +shooter);
            if (clientID != shooter)
            {
                ClientRpcParams clientRpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { clientID }
                    }
                };

                player.Health.ReceiveHitClientRpc(damage, clientRpcParams);
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }
}
