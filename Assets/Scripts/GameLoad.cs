using Unity.Netcode;
using UnityEngine;

public class GameLoad : NetworkBehaviour
{
    [SerializeField]
    private GameObject cam;

    [SerializeField]
    private GameObject m_objects;

    [SerializeField]
    private GameObject m_floor;

    [SerializeField]
    private GameObject capsule;

    [SerializeField]
    private int capsuleCount;

    [SerializeField]
    private GameObject cube;

    [SerializeField]
    private int cubeCount;

    [SerializeField]
    private GameObject sphere;

    [SerializeField]
    private int sphereCount;


    public override void OnNetworkSpawn()
    {
        Debug.Log("Spawning | " + IsServer);
        if (IsServer)
        {
            SpawnObjects(capsuleCount, capsule, 10f);
            SpawnObjects(cubeCount, cube, 5f);
            SpawnObjects(sphereCount, sphere, 5f);
        }
    }
    
    private void SpawnObjects(int count, GameObject g_object, float height)
    {
        if (count > 0)
        {
            for (int i = 1; i <= count; i++)
            {
                GameObject obj = Instantiate(
                    g_object,
                    new Vector3(
                        Random.Range(-m_floor.transform.localScale.x / 2, m_floor.transform.localScale.x / 2),
                        height,
                        Random.Range(-m_floor.transform.localScale.z / 2, m_floor.transform.localScale.z / 2)
                    ),
                    Quaternion.Euler(Vector3.zero)
                );
                obj.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}
