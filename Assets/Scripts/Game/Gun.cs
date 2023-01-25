using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : NetworkBehaviour
{
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Player player;

    public void Fire(InputAction.CallbackContext context)
    {
        if (!IsOwner || player.SettingUp) return;

        if (player.Alive && context.started)
        {
            FireServerRpc(transform.position, transform.rotation);
        }
    }

    [ServerRpc]
    private void FireServerRpc(Vector3 position, Quaternion rotation)
    {
        GameObject _bullet = Instantiate(bullet, position, rotation);
        _bullet.GetComponent<Bullet>().Shooter = player.ClientID;
        _bullet.GetComponent<NetworkObject>().Spawn();
    }
}
