using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Netcode.Transports.UTP;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField listenAddressInput;

    private string listenAddress;

    [SerializeField]
    private TMP_InputField ipAddressInput;

    private string ipAddress;

    [SerializeField]
    private UnityTransport transport;

    public void Close()
    {
        Application.Quit();
    }

    public void StartServer()
    {
        if (string.IsNullOrEmpty(listenAddressInput.text))
        {
            listenAddress = "127.0.0.1";
        }
        else
        {
            listenAddress = listenAddressInput.text;
        }
        transport.SetConnectionData(listenAddress, 7777, listenAddress);
        LoadGameScene();
        NetworkManager.Singleton.StartServer();
    }

    public void JoinServer()
    {
        if (string.IsNullOrEmpty(ipAddressInput.text))
        {
            ipAddress = "127.0.0.1";
        }
        else
        {
            ipAddress = ipAddressInput.text;
        }

        transport.SetConnectionData(ipAddress, 7777);// ConnectionData.Address = ipAddress;
        LoadGameScene();
        NetworkManager.Singleton.StartClient();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(sceneName: "Game");
    }
}