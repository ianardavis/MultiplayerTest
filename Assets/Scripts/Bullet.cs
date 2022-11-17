using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float velocity;

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        StartCoroutine(SelfDestruct());
        rb.AddRelativeForce(new Vector3(0f, velocity, 0f));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) return;
        collision.gameObject.SendMessage("ReceiveHit", SendMessageOptions.DontRequireReceiver);
        //GetComponent<NetworkObject>().Despawn();
    }
}
