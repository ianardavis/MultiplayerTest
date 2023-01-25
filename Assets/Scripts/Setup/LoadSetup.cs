using UnityEngine;
using Unity.Netcode;

public class LoadSetup : NetworkBehaviour
{
    [SerializeField]
    private GameObject PlayerSettings;

    [SerializeField]
    private GameObject GameSettings;

    public override void OnNetworkSpawn()
    {
        GameSettings.SetActive(IsServer);
        PlayerSettings.SetActive(!IsServer);
    }
    public void OnNameChange(string name)
    {
        PlayerSetup player = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerSetup>();
        if (player != null)
        {
            player.SetName(name);
        }
    }
}