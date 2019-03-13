using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace DNSWebMonitor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            typeof(TreeView).GetProperty("Sorted").SetValue(treeView1, true, null);
            
            UdpClient c = new UdpClient(53);
            c.BeginReceive(ClientUdpReceiveCallback, c);
        }

        private void ClientUdpReceiveCallback(IAsyncResult ar)
        {
            LogEntry entry = new LogEntry();
            entry.Timestamp = DateTime.Now;
            UdpClient client = ((UdpClient)ar.AsyncState);
            UdpClient server = new UdpClient();
            IPEndPoint epc = new IPEndPoint(IPAddress.Any, 53);
            IPEndPoint eps = new IPEndPoint(IPAddress.Any, 53);

            byte[] request = client.EndReceive(ar, ref epc);
            entry.ClientAddress = epc.Address;
            entry.DomainName = Utils.ParseDomainName(request);
                        
            server.Send(
                request,
                request.Length,
                new IPEndPoint(new IPAddress(new byte[] { 8, 8, 8, 8 }), 53));
            byte[] response = server.Receive(ref eps);
            server.Close();
            
            client.Send(response, response.Length, epc);
            client.Close();

            treeView1.Invoke((MethodInvoker)delegate
            {
                if (!treeView1.Nodes.ContainsKey(entry.ToString()))
                    treeView1.Nodes.Add(entry.ToString(), entry.ToString());

                TreeNode node = treeView1.Nodes[entry.ToString()];
                if (!node.Nodes.ContainsKey(entry.ClientAddress.ToString()))
                    node.Nodes.Add(entry.ClientAddress.ToString(), entry.ClientAddress.ToString());
                
                node.Nodes[entry.ClientAddress.ToString()].Nodes.Add(entry.Timestamp.ToString());
            });
            
            UdpClient c = new UdpClient(53);
            c.BeginReceive(ClientUdpReceiveCallback, c);
        }
    }
}
