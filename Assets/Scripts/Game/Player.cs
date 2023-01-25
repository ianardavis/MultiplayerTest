using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<int> moveSpeed = new NetworkVariable<int>(1);

    [SerializeField]
    private NetworkVariable<int> rotateSpeed = new NetworkVariable<int>(1);

    [SerializeField]
    private NetworkVariable<int> jumpHeight = new NetworkVariable<int>(1);

    [SerializeField]
    private NetworkVariable<int> jumpsLeft = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );
    public int JumpsLeft
    {
        get { return jumpsLeft.Value; }
    }

    [SerializeField]
    private NetworkVariable<int> jumpsMax= new NetworkVariable<int>(10);

    private NetworkVariable<bool> alive = new NetworkVariable<bool>(true);
    public bool Alive
    {
        get { return alive.Value; }
        set { alive.Value = value; }
    }

    public Health Health
    {
        get { return GetComponent<Health>(); }
    }

    [SerializeField]
    private GameObject setupPlatform;

    public bool SettingUp
    {
        get { return SceneManager.GetActiveScene().name == "Setup"; }
    }

    public ulong ClientID
    {
        get { return OwnerClientId; }
    }

    [SerializeField]
    private GameObject cam;

    [SerializeField]
    private Rigidbody rb;

    private Vector3 moveValue;

    private Vector3 rotateDir;

    public void Move(InputAction.CallbackContext context)
    {
        if (!IsOwner || SettingUp) return;

        Vector2 action = context.ReadValue<Vector2>();
        moveValue = new Vector3(action.x, 0f, action.y) * Time.deltaTime * moveSpeed.Value;
    }
    public void Rotate(InputAction.CallbackContext context)
    {
        if (!IsOwner || SettingUp) return;

        rotateDir = new Vector3(0f, context.ReadValue<Vector2>().x * Time.deltaTime * rotateSpeed.Value, 0f);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!IsOwner || SettingUp) return;

        if (alive.Value && context.started && jumpsLeft.Value > 0)
        {
            jumpsLeft.Value--;
            rb.AddForce(transform.up * jumpHeight.Value, ForceMode.Impulse);
        }
    }

    private void AddJump()
    {
        if (!IsOwner || SettingUp) return;

        if (alive.Value && jumpsLeft.Value < jumpsMax.Value)
        {
            jumpsLeft.Value++;
        }
    }

    private void Update()
    {
        if (!IsOwner) return;
        
        if (transform.position.y < -10f)
        {
            PlaceRandomly();
        }

        transform.Rotate(rotateDir);

        if (alive.Value)
        {
            transform.Translate(moveValue);
        }

    }
    private void PlaceRandomly()
    {
        transform.localPosition = new Vector3(
            Random.Range(-50f, 50f),
            5f,
            Random.Range(-50f, 50f)
        );

    }
    private void PlaceAt(float offset)
    {
        GameObject platform = Instantiate(setupPlatform, new Vector3(offset*10, -0.05f, 0f), Quaternion.identity);
        transform.localPosition = new Vector3(offset * 10f, 0f, 0f);
    }

    public override void OnNetworkSpawn()
    {
        if (SettingUp)
        {
            PlaceAt(OwnerClientId);
        }
        else
        {
            PlaceRandomly();
        }
            
        jumpsLeft.Value = jumpsMax.Value;
        InvokeRepeating(nameof(AddJump), 5f, 5f);
        if (IsOwner && !SettingUp) cam.SetActive(true);
    }
}