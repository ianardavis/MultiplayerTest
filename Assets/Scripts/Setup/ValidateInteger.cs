using UnityEngine;
using TMPro;

public class ValidateInteger : MonoBehaviour
{
    private TMP_InputField input;
    private void Start()
    {
        input = GetComponent<TMP_InputField>();
    }
    public void Check()
    {
        int.TryParse(input.text, out int result);
        input.text = result.ToString();
    }
}