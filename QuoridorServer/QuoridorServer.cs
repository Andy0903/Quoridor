using System;
using System.Windows.Forms;
using QuoridorNetwork;
using System.Collections.Generic;

namespace QuoridorServer
{
    public partial class QuoridorServerForm : Form
    {
        List<Tuple<string, long>> myClients = new List<Tuple<string, long>>();
        Game myGameboard;

        private bool ServerIsFull { get { return (myClients.Count == (RadioButton2Players.Checked ? 2 : 4)); } }

        public QuoridorServerForm()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            RunServer();
        }

        private void RunServer()
        {
            NetworkManager.InitializeServer(int.Parse(PortTextBox.Text));
            LogTextBox.AppendText("Server started!" + "\n");
            LogTextBox.AppendText("Waiting for connections.." + "\n\n");

            NumberOfPlayersGroup.Enabled = false;
            PortGroup.Enabled = false;
            StartButton.Enabled = false;

            NetworkManager.OnPlayerConnected += NetworkManager_OnPlayerConnected;
            NetworkManager.OnConnect += NetworkManager_OnConnect;

            timer1.Start();
        }

        private void NetworkManager_OnConnect(object sender, EventArgs e)
        {
            LogTextBox.AppendText("Player connected!" + "\n");
        }

        private void NetworkManager_OnPlayerConnected(object sender, PlayerConnectMessage e)
        {
            if (ServerIsFull)
            {
                e.Sender.Disconnect("Disconnect");
                return;
            }

            myClients.Add(new Tuple<string, long>(e.PlayerName, e.Sender.RemoteUniqueIdentifier));
            PlayerLabel.Text = "Players: " + myClients.Count;

            if (ServerIsFull)
            {
                myGameboard = new Game(myClients);
                myGameboard.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NetworkManager.Update();
        }
    }
}
