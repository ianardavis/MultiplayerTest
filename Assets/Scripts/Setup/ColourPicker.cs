using Unity.Netcode;
using UnityEngine;

public class ColourPicker : NetworkBehaviour
{
    [SerializeField]
    private RectTransform texture;

    [SerializeField]
    private GameObject indicator;

    [SerializeField]
    private Texture2D refSprite;

    [SerializeField]
    private Material matColour;

    private void SetColour()
    {
        Vector3 imagePos = texture.position;
        Vector3 localPos = Input.mousePosition - imagePos;
        Debug.Log($"Mouse position: {Input.mousePosition} | image Position: {imagePos} | local position: {localPos}");

        Color c = refSprite.GetPixel((int)localPos.x, (int)localPos.y);
        matColour.color = c;
        PlayerSetup player = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerSetup>();
        if (player != null)
        {
            player.SetColour(c);
        }
    }

    public void SelectColour()
    {
        SetColour();
    }
}