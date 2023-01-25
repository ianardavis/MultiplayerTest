using Unity.Netcode;
using UnityEngine;

public class StationaryObject : NetworkBehaviour
{
    [SerializeField]
    private Material mat1;

    [SerializeField]
    private Material mat2;

    [ClientRpc]
    public void ReceiveHitClientRpc()
    {
        Debug.Log("Hit! Object");
        Renderer renderer = GetComponent<Renderer>();
        Debug.Log(renderer.material);
        if (renderer.material == mat1)
        {
            renderer.material = mat2;
        }
        else
        {
            renderer.material = mat1;
        }
    }
}
