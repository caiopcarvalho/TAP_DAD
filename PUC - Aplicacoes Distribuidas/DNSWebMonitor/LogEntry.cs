using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace DNSWebMonitor
{
    public class LogEntry
    {
        public string[] DomainName { get; set; }
        public IPAddress ClientAddress { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < DomainName.Length - 1; i++)
                s += DomainName[i] + "/";
            s += DomainName[DomainName.Length - 1];

            return s;
        }
    }
}
