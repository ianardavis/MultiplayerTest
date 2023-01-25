using UnityEngine;
using TMPro;

public class Jumps : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    private void Update()
    {
        text.text = player.JumpsLeft.ToString();
    }
}
