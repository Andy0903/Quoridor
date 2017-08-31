using System;
using System.Windows.Forms;
using QuoridorNetwork;
using System.Collections.Generic;
using System.Linq;

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
            if (StartButton.Text == "Start")
            {
                RunServer();
            }
            else
            {
                StopServer();
            }
        }

        private void RunServer()
        {
            NetworkManager.InitializeServer(int.Parse(PortTextBox.Text));
            LogTextBox.AppendText("Server started!" + "\n");
            LogTextBox.AppendText("Waiting for connections.." + "\n\n");

            NumberOfPlayersGroup.Enabled = false;
            PortGroup.Enabled = false;

            StartButton.Text = "Disconnect";

            NetworkManager.OnPlayerConnected += NetworkManager_OnPlayerConnected;
        }

        private void NetworkManager_OnPlayerConnected(object sender, PlayerConnectMessage e)
        {
            if (ServerIsFull)
            {
                e.Sender.Disconnect("Disconnect");
                return;
            }
            
            myClients.Add(new Tuple<string, long>(e.PlayerName, e.Sender.RemoteUniqueIdentifier));

            if (ServerIsFull)
            {
                myGameboard = new Game(myClients);
                myGameboard.Start();
            }
        }

        private void StopServer()
        {
            NetworkManager.Deinitialize();

            LogTextBox.AppendText("Server closed!" + "\n\n");
            NumberOfPlayersGroup.Enabled = true;
            PortGroup.Enabled = true;
            PlayerLabel.Text = "Players: 0";

            StartButton.Text = "Start";
        }
    }
}
