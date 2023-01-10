using UnityEngine;
using TMPro;

public class HealthLabel : MonoBehaviour
{
    [SerializeField]
    private Health health;

    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    private void Update()
    {
        text.text = health.Current.ToString();
        if (health.Current <= 0) text.text = "You Died!!!!";
    }
}
