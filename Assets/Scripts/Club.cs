using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Club : NetworkBehaviour
{
    [SerializeField]
    private int damage = 5;

    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private Player player;

    private bool swinging = false;
    private bool returning = false;

    public void Swing(InputAction.CallbackContext context)
    {
        if (player.Alive)
        {
            StartCoroutine(SwingClub());
        }
    }
    private IEnumerator SwingClub()
    {
        if (!swinging && !returning)
        {
            swinging = true;
            while (swinging)
            {
                float currentX = transform.localRotation.eulerAngles.x;
                float rotateAngle = Mathf.Clamp(speed * Time.deltaTime * 100f, 0f, 90f - currentX);
                transform.Rotate(rotateAngle, 0f, 0f);

                if (currentX + rotateAngle == 90f)
                {
                    swinging = false;
                }
                yield return null;
            }
            swinging = false;
            returning = true;
            while (returning)
            {
                float currentX = transform.localRotation.eulerAngles.x;
                float rotateAngle = Mathf.Clamp(speed * Time.deltaTime * 100f, 0f, currentX);
                transform.Rotate(-rotateAngle, 0f, 0f);
                if (currentX - rotateAngle == 0f)
                {
                    returning = false;
                }
                yield return null;
            }
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            returning = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!IsServer) return;
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { player.ClientID }
                }
            };

            player.Health.ReceiveHitClientRpc(damage, clientRpcParams);
        }
    }
}