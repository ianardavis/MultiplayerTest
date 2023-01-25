using System;
using System.Collections;
using UnityEngine;

public class IPAddress
{
    private string iPAddress;
    public string IpAddress { get { return iPAddress; } }

    private bool isValid = false;
    public bool IsValid { get { return isValid; } }

    private DateTime lastPing;
    public DateTime LastPing { get { return lastPing; } }

    private bool pingSuccessful = false;
    public bool PingSuccessful { get { return pingSuccessful; } }

    private bool Valid()
    {
        if (string.IsNullOrWhiteSpace(iPAddress))
        {
            return false;
        }

        string[] splitValues = iPAddress.Split('.');
        if (splitValues.Length != 4)
        {
            return false;
        }

        foreach (string octet in splitValues)
        {
            bool parsed_ok = int.TryParse(octet, out int parsed_octet);
            if (!parsed_ok)
            {
                return false;
            }

            if (parsed_octet > 255)
            {
                return false;
            }
        }
        return true;
    }
    public IEnumerator Ping()
    {
        float currentWait = 0f;
        Ping ping = new Ping(iPAddress);
        while (!ping.isDone)
        {
            currentWait += Time.deltaTime;
            if (currentWait > 5f)
            {
                break;
            }
            yield return null;
        }
        pingSuccessful = ping.isDone;
        lastPing = DateTime.Now;
        ping.DestroyPing();
    }

    public IPAddress(string ipAddress)
    {
        iPAddress = ipAddress.Trim();
        isValid = Valid();
    }
}
