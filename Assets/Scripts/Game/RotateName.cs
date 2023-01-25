using UnityEngine;

public class RotateName : MonoBehaviour
{
    [SerializeField]
    private float speed = 40f;
    private void Update()
    {
        transform.Rotate(new Vector3(0f, Time.deltaTime * speed, 0f));
    }
}
