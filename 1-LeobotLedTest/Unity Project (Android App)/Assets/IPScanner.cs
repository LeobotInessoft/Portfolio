using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.UI;
using System.Net.NetworkInformation;
using System.Linq;
public class IPScanner : MonoBehaviour
{
    System.Net.NetworkInformation.Ping pingSender = new System.Net.NetworkInformation.Ping();
    // Start is called before the first frame update
    void Start()
    {

        pingSender.PingCompleted += OnPingCompleted;

        StartCoroutine(DoAllPings());
    }

    // Update is called once per frame
    void Update()
    {

    }
    bool pingisDone = true;
    public IEnumerator DoAllPings()
    {
        pingisDone = true;
        IPAddress ipAddress = NetworkInterface
   .GetAllNetworkInterfaces()
   .SelectMany(ni => ni.GetIPProperties().UnicastAddresses)
   .Where(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
   .Select(a => a.Address)
   .FirstOrDefault();

        // Get the default gateway IP address
        IPAddress gatewayAddress = null;
        if (ipAddress != null)
        {
            gatewayAddress = NetworkInterface
                .GetAllNetworkInterfaces()
                .SelectMany(ni => ni.GetIPProperties().GatewayAddresses)
                .Where(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .FirstOrDefault()?.Address;
        }

        Debug.Log($"Gateway IP address: {gatewayAddress}");
        // string gatewayAddress = "192.168.8.1";
        IPAddress gatewayIP = gatewayAddress;//  IPAddress.Parse(gatewayAddress);
        IPAddress subnetMask = GetSubnetMask();// gatewayIP.GetSubnetMask();
        byte[] gatewayBytes = gatewayIP.GetAddressBytes();
        byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

        byte[] networkAddressBytes = new byte[4];
        for (int i = 0; i < 4; i++)
        {
            networkAddressBytes[i] = (byte)(gatewayBytes[i] & subnetMaskBytes[i]);
        }

        IPAddress networkAddress = new IPAddress(networkAddressBytes);
        Debug.Log($"Network address: {networkAddress}");

        for (int host = 1; host <= 254; host++)
        {

            networkAddressBytes[3] = (byte)host;
            IPAddress ipAddressScan = new IPAddress(networkAddressBytes);

            Debug.Log($"IP address: {ipAddressScan}");
            pingisDone = false;
            PingIPAddress(ipAddressScan.ToString()); ;
            while (pingisDone == false)
            {
                yield return new WaitForSeconds(1f);
            }


        }
    }

    public void PingIPAddress(string ipAddress)
    {

        // Send the ping request asynchronously
        pingSender.SendAsync(ipAddress, 10);
    }
    private void OnPingCompleted(object sender, PingCompletedEventArgs e)
    {
        if (e.Error != null)
        {
            // Handle ping error
            Debug.Log($"Ping failed: {e.Error.Message}");
            return;
        }

        if (e.Reply.Status == IPStatus.Success)
        {
            // Ping succeeded
            Debug.Log($"Ping successful. Roundtrip time: {e.Reply.RoundtripTime}ms");
        }
        else
        {
            // Ping failed
            Debug.Log($"Ping failed. Status: {e.Reply.Status}");
        }
        pingisDone = true;
    }
    public static IPAddress GetSubnetMask()
    {
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface adapter in interfaces)
        {
            IPInterfaceProperties properties = adapter.GetIPProperties();
            foreach (UnicastIPAddressInformation uni in properties.UnicastAddresses)
            {
                if (uni.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return uni.IPv4Mask;
                }
            }
        }
        return null;
    }




}
