using System;
using System.Windows.Forms;
using QuoridorNetwork;
using System.Collections.Generic;
using Lidgren.Network;

namespace QuoridorServer
{
    public partial class QuoridorServerForm : Form
    {
        List<Tuple<string, NetConnection>> clients = new List<Tuple<string, NetConnection>>();
        Game game;

        private bool ServerIsFull { get { return (clients.Count == (RadioButton2Players.Checked ? 2 : 4)); } }

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

            clients.Add(new Tuple<string, NetConnection>(e.PlayerName, e.Sender));
            PlayerLabel.Text = "Players: " + clients.Count;

            if (ServerIsFull)
            {
                game = new Game(clients);
                game.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NetworkManager.Update();
        }
    }
}
