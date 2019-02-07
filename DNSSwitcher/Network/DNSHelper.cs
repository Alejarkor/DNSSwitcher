using System.Linq;
using System.Management;
using System.Net.NetworkInformation;

namespace DNSSwitcher.Network
{
    /// <summary>
    /// Class with helper functions to deal with DNS thingies.
    /// Huge credits to Mohamad Rashidi at https://stackoverflow.com/questions/40291375/how-to-change-dns-with-c-sharp-on-windows-10
    /// </summary>
    public static class DnsHelper
    {
        /// <summary>
        /// Are we connected to an ethernet network?
        /// </summary>
        public static bool Connected => GetActiveEthernetNetworkInterface() != null;

        /// <summary>
        /// Are we using the default DNS?
        /// </summary>
        public static bool UsingDefault;

        /// <summary>
        /// Sets the Google's DNS'.
        /// </summary>
        public static void SetGoogleDns() => SetDns(GetActiveEthernetNetworkInterface());

        /// <summary>
        /// Sets the default DNS'.
        /// </summary>
        public static void SetDefaultDns() => SetDns(GetActiveEthernetNetworkInterface(), useDefault: true);

        /// <summary>
        /// Gets the active Ethernet network interface.
        /// </summary>
        /// <returns>The interface.</returns>
        private static NetworkInterface GetActiveEthernetNetworkInterface() =>
            NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(a =>
                a.OperationalStatus == OperationalStatus.Up &&
                a.NetworkInterfaceType == NetworkInterfaceType.Ethernet && a.GetIPProperties().GatewayAddresses
                    .Any(g => g.Address.AddressFamily.ToString() == "InterNetwork"));

        /// <summary>
        /// Set the given DNS on the given interface.
        /// </summary>
        /// <param name="currentInterface">The interface to change.</param>
        /// <param name="primaryDns">The DNS to set. Google's by default.</param>
        /// <param name="secondaryDns">The secondary DNS to set. Google's by default.</param>
        /// <param name="useDefault">Use default dns?</param>
        private static void SetDns(NetworkInterface currentInterface, string primaryDns = "8.8.8.8",
            string secondaryDns = "8.8.4.4", bool useDefault = false)
        {
            string[] dns = {primaryDns, secondaryDns};
            if (currentInterface == null) return;

            var objMc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            var objMoc = objMc.GetInstances();
            foreach (var o in objMoc)
            {
                var objMo = (ManagementObject) o;
                if (!(bool) objMo["IPEnabled"]) continue;
                if (!objMo["Caption"].ToString().Contains(currentInterface.Description)) continue;
                var objDns = objMo.GetMethodParameters("SetDNSServerSearchOrder");
                if (objDns == null) continue;
                objDns["DNSServerSearchOrder"] = useDefault ? null : dns;
                objMo.InvokeMethod("SetDNSServerSearchOrder", objDns, null);
            }

            UsingDefault = useDefault;
        }
    }
}