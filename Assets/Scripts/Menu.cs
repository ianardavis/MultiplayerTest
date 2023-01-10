using System.Collections;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Netcode.Transports.UTP;

public class Menu : NetworkBehaviour
{
    [SerializeField]
    private TMP_InputField ipAddressInput;

    [SerializeField]
    private UnityTransport transport;

    [SerializeField]
    private TMP_Text messageBox;

    private void Start()
    {
        if (PlayerPrefs.HasKey("ServerIP"))
        {
            ipAddressInput.text = PlayerPrefs.GetString("ServerIP");
        }
    }

    public void Close()
    {
        Application.Quit();
    }

    public void StartGame(bool asHost)
    {
        StartCoroutine(CheckIP(asHost));
    }

    private IEnumerator CheckIP(bool host)
    {
        IPAddress address = new IPAddress(ipAddressInput.text);

        if (!address.IsValid)
        {
            messageBox.text = "Invalid IP Address";

        }
        else
        {
            yield return StartCoroutine(address.Ping());
            if (!address.PingSuccessful)
            {
                messageBox.text = "Invalid Host";

            }
            else
            {
                PlayerPrefs.SetString("ServerIP", address.IpAddress);
                transport.SetConnectionData(address.IpAddress, 7777);
                if (host)
                {
                    NetworkManager.Singleton.StartHost();
                    NetworkManager.SceneManager.LoadScene("Game", LoadSceneMode.Single);
                }
                else
                {
                    NetworkManager.Singleton.StartClient();
                }
            }
        }
    }
}