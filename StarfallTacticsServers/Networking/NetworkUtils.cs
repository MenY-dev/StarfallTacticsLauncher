using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Networking
{
    public static class NetworkUtils
    {
        public class IPAddressData
        {
            public NetworkInterface NetworkInterface { get; }
            public UnicastIPAddressInformation Information { get; }

            public IPAddressData(NetworkInterface networkInterface, UnicastIPAddressInformation information)
            {
                NetworkInterface = networkInterface;
                Information = information;
            }
        }



        public static NetworkInterface GetInterface(IPAddress interfaceAddress)
        {
            if (interfaceAddress is null)
                return null;

            foreach (var item in GetUnicastAddressesIterator())
            {
                if (interfaceAddress.Equals(item.Information.Address))
                    return item.NetworkInterface;
            }

            return null;
        }

        public static UnicastIPAddressInformation GetInformation(IPAddress interfaceAddress)
        {
            if (interfaceAddress is null)
                return null;

            foreach (var item in GetUnicastAddressesIterator())
            {
                Debugging.ObjectLogExtension.Log(item, item.Information.Address);

                if (interfaceAddress.Equals(item.Information.Address))
                    return item.Information;
            }

            return null;
        }

        public static UnicastIPAddressInformation GetInformation(NetworkInterface networkInterface, IPAddress address)
        {
            if (networkInterface is null || address is null)
                return null;

             

            var properties = networkInterface.GetIPProperties();

            if (properties == null)
                return null;

            foreach (var information in properties.UnicastAddresses)
            {
                if (information?.Address?.Equals(address) == true)
                    return information;
            }

            return null;
        }

        public static IEnumerable<IPAddressData> GetUnicastAddressesIterator()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != OperationalStatus.Up ||
                    ni.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;

                var properties = ni.GetIPProperties();

                if (properties == null)
                    continue;

                foreach (var information in properties.UnicastAddresses)
                     yield return new IPAddressData(ni, information);
            }
        }

        public static IPAddress GetMaskedAddress(IPAddress address, IPAddress mask)
        {
            if (address is null)
                return null;

            if (mask is null)
                return address;

            byte[] adressBytes = address.GetAddressBytes();
            byte[] maskBytes = mask.GetAddressBytes();

            if (adressBytes.Length != adressBytes.Length)
                return null;

            byte[] maskedAddress = new byte[adressBytes.Length];

            for (int i = 0; i < maskedAddress.Length; i++)
                maskedAddress[i] = (byte)(adressBytes[i] & (maskBytes[i]));

            return new IPAddress(maskedAddress);
        }

        public static bool IsSubnetAddresses(IPAddress subnetMask, params IPAddress[] addresses)
        {
            if (subnetMask is null || addresses is null || addresses.Length < 1)
                return false;

            IPAddress targetAddress = GetMaskedAddress(addresses[0], subnetMask);

            if (targetAddress is null)
                return false;

            for (int i = 1; i < addresses.Length; i++)
            {
                if (targetAddress.Equals(GetMaskedAddress(addresses[i], subnetMask)) == false)
                    return false;
            }

            return true;
        }

        public static bool IsLocalClient(TcpClient client)
        {
            if (client is null)
                return false;

            Socket socket = client.Client;

            if (socket is null)
                return false;

            IPAddress localAddress = (socket.LocalEndPoint as IPEndPoint)?.Address;
            IPAddress remoteAddress = (socket.RemoteEndPoint as IPEndPoint)?.Address;
            IPAddress mask = GetInformation(localAddress).IPv4Mask;

            if (localAddress is null || remoteAddress is null || mask is null)
                return false;

            if (IPAddress.Loopback.Equals(localAddress) &&
                IPAddress.Loopback.Equals(remoteAddress))
                return true;

            return IsSubnetAddresses(mask, localAddress, remoteAddress);
        }
    }
}
