using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<int> moveSpeed = new NetworkVariable<int>(1);

    private NetworkVariable<int> health = new NetworkVariable<int>(100);
    public int Health
    {
        get { return health.Value; }
    }

    [SerializeField]
    private float rotateSpeed = 1f;

    [SerializeField]
    private GameObject cam;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private GameObject gun;

    [SerializeField]
    private TMP_Text healthIndicator;

    private void Update()
    {
        if (!IsOwner) return;
        float rotateDir = 0f;
        if (Input.GetKey(KeyCode.A)) rotateDir = -1f;
        if (Input.GetKey(KeyCode.D)) rotateDir = +1f;
        transform.Rotate(new Vector3(0f, rotateDir * rotateSpeed * Time.deltaTime, 0f));

        if (health.Value <= 0)
        {
            return;
        }
        Vector3 moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.Q)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.E)) moveDir.x = +1f;

        transform.Translate(moveDir * moveSpeed.Value * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireServerRpc(gun.transform.position, gun.transform.rotation);
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
    private void FireServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject _bullet = Instantiate(bullet, position, rotation);
        _bullet.GetComponent<NetworkObject>().Spawn();
    }

    [ClientRpc]
    public void ReceiveHitClientRpc(int damage)
    {
        health.Value -= damage;
    }
}