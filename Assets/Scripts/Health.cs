using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField]
    private PlayerNetwork player;

    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    private void Update()
    {
        text.text = player.Health.ToString();
        if (player.Health <= 0) text.text = "You Died!!!!";
    }
}
