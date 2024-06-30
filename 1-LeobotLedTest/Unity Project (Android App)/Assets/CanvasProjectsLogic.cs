using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.UI;
using System.Net.NetworkInformation;
using System.Linq;
public class CanvasProjectsLogic : MonoBehaviour
{
    public InputField InputTest;
    // Start is called before the first frame update
    void Start()
    {
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



    }

    // Update is called once per frame
    void Update()
    {

    }
}
