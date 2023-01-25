using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField]
    private List<TMP_Text> nameTags;

    [SerializeField]
    private Renderer _renderer;

    public void SetName(string name)
    {
        foreach(TMP_Text text in nameTags)
        {
            text.text = name;
        }
    }
    public void SetColour(Color color)
    {
        _renderer.material.color = color;
    }
}
