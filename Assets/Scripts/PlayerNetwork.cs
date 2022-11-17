using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<int> moveSpeed = new NetworkVariable<int>(1);

    [SerializeField]
    private float rotateSpeed = 1f;

    [SerializeField]
    private GameObject cam;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private GameObject gun;

    private void Update()
    {
        if (!IsOwner) return;
        Vector3 moveDir = Vector3.zero;
        float rotateDir = 0f;

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.Q)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.E)) moveDir.x = +1f;

        if (Input.GetKey(KeyCode.A)) rotateDir = -1f;
        if (Input.GetKey(KeyCode.D)) rotateDir = +1f;

        transform.Translate(moveDir * moveSpeed.Value * Time.deltaTime);
        transform.Rotate(new Vector3(0f, rotateDir * rotateSpeed * Time.deltaTime, 0f));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireServerRpc();
        }
    }

    public override void OnNetworkSpawn()
    {
        transform.localPosition = new Vector3(
            Random.Range(-50f, 50f),
            0f,
            Random.Range(-50f, 50f)
        );
        if (IsOwner) cam.SetActive(true);
    }

    [ServerRpc]
    private void FireServerRpc()
    {
        GameObject _bullet = Instantiate(bullet, gun.transform.position, gun.transform.rotation);
        _bullet.GetComponent<NetworkObject>().Spawn();
    }
}