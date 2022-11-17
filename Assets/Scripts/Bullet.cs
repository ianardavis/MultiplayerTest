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
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(IsServer);
        if (!IsServer) return;
        collision.gameObject.SendMessage("ReceiveHitClientRpc", damage, SendMessageOptions.DontRequireReceiver);
    }
}
