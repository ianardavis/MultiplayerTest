using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameLoad : NetworkBehaviour
{
    [SerializeField]
    private GameObject cam;

    public override void OnNetworkSpawn()
    {

        if (IsOwner) cam.SetActive(true);
    }
}
