using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
        if (!PlayerPrefs.HasKey("ServerIP"))
        {
            PlayerPrefs.SetString("ServerIP", "");
        }
        ipAddressInput.text = PlayerPrefs.GetString("ServerIP");
    }

    public void Close()
    {
        Application.Quit();
    }

    public void StartServer()
    {
        StartCoroutine(CheckIP(ipAddressInput.text, true));
    }

    public void JoinServer()
    {
        StartCoroutine(CheckIP(ipAddressInput.text, false));
    }

    public IEnumerator CheckIP(string ip, bool host)
    {
        if (string.IsNullOrEmpty(ip))
        {
            ip = "127.0.0.1";
        }
        float currentWait = 0f;
        Ping p = new Ping(ip);
        while (p.isDone == false)
        {
            currentWait += Time.deltaTime;
            if (currentWait > 5f)
            {
                break;
            }
            yield return null;
        }
        if (p.isDone)
        {
            if (ip != "127.0.0.1")
            {
                PlayerPrefs.SetString("ServerIP", ip);
            }
            transport.SetConnectionData(ip, 7777);
            OnPingComplete(host);
        }
        else
        {
            messageBox.text = "Invalid IP Address/Host";
            p.DestroyPing();
        }
    }
    private void OnPingComplete(bool host)
    {
        if (host)
        {
            NetworkManager.Singleton.StartHost();
            LoadGameScene();
        }
        else
        {
            NetworkManager.Singleton.StartClient();
        }
    }

    private void LoadGameScene()
    {
        NetworkManager.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}