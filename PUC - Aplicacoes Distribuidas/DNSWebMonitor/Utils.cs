using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DNSWebMonitor
{
    public static class Utils
    {
        public static string[] ParseDomainName(byte[] request)
        {
            List<string> domainName = new List<string>();

            int i = 12;
            while (request[i] > 0)
            {
                domainName.Add(ASCIIEncoding.ASCII.GetString(request, i + 1, request[i]));
                i += request[i] + 1;
            }
            
            domainName.Reverse();
            return domainName.ToArray();
        }
    }
}
