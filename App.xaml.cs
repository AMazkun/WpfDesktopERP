using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    /// 
    public partial class App : Application
    {
        public static string LocalIP { get; private set; } = GetLocalIPAddress();
        public static string GetLocalIPAddress()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "No adapters IPv4";
            } else { return "No network"; }
        }
    }
}
